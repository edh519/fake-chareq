using BusinessLayer.Repositories;
using BusinessLayer.Repositories.Interfaces;
using BusinessLayer.Services;
using BusinessLayer.Services.EmailProcessing;
using BusinessLayer.Services.EmailProcessing.EmailHelpers;
using BusinessLayer.Services.Interfaces;
using DataAccessLayer;
using DataAccessLayer.External.Models;
using DataAccessLayer.External.Repos;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using QuestPDF.Infrastructure;
using System;
using System.Diagnostics;
using UoN.ExpressiveAnnotations.NetCore.DependencyInjection;
using WebApp.Configuration.Subscriptions;
using WebApp.Jobs;
using YTU.EmailService;
using YTULib;

namespace WebApp;

public class Startup
{
  public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
  {
    Configuration = configuration;
    WebHostEnvironment = webHostEnvironment;
  }

  public IConfiguration Configuration { get; }
  public IWebHostEnvironment WebHostEnvironment { get; }

  // This method gets called by the runtime. Use this method to add services to the container.
  public void ConfigureServices(IServiceCollection services)
  {
    services.AddMvc(options =>
    {
      options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    });

    services.AddDatabaseDeveloperPageExceptionFilter();
    ConfigureDatabaseConnections(services);
    ConfigureAPISettings(services);

    ConfigureAuth(services);

    services.AddControllersWithViews().AddRazorRuntimeCompilation();
    services.AddRazorPages();


    services.AddScoped<DataAccessLayer.SeedDataService>();

    services.AddScoped<EmailHandlerService>();

    services.AddHttpContextAccessor();

    services.Configure<SubscriptionServiceConfig>(
        Configuration.GetSection(SubscriptionServiceConfig.SubscriptionService));
    services.Configure<StaleRequestsConfig>(
        Configuration.GetSection(StaleRequestsConfig.StaleRequests));
    services.Configure<AssignGHIssuesConfig>(
        Configuration.GetSection(AssignGHIssuesConfig.AssignGHIssues));
    ConfigureGitHubOctokitSettings(services);
    services.AddScoped<GitHubApiRepository>();
    services.AddScoped<GitHubService>();
    services.AddScoped<RazorToHtmlParser>();

    services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
    services.AddScoped<IEmailSender, EmailSender>();



    #region Repositories
    services.AddScoped<IWorkRequestRepository, WorkRequestRepository>();
    services.AddScoped<IWorkRequestStatusRepository, WorkRequestStatusRepository>();
    services.AddScoped<INotificationRepository, NotificationRepository>();
    services.AddScoped<INotificationTypeRepository, NotificationTypeRepository>();
    services.AddScoped<IFileRepository, FileRepository>();
    services.AddScoped<BusinessLayer.Repositories.Interfaces.ITrialRepository, BusinessLayer.Repositories.TrialRepository>();
    services.AddScoped<IDashboardRepository, DashboardRepository>();
    services.AddScoped<YTULibServices>();
    services.AddScoped<ILabelRepository, LabelRepository>();
    services.AddScoped<ITemplateRepository, TemplateRepository>();
    services.AddScoped<IWorkRequestSubscriptionRepository, WorkRequestSubscriptionRepository>();
    services.AddScoped<IWorkRequestStaleRepository, WorkRequestStaleRepository>();
    services.AddScoped<ITriageInfoRotaRepository, TriageInfoRotaRepository>();
    services.AddScoped<ITrialRepositoryInfoRepository, TrialRepositoryInfoRepository>();
    services.AddScoped<PullRequestRepository>();
    services.AddScoped<IEmailRepository, EmailRepository>();
    services.AddScoped<ISubTaskStatusRepository, SubTaskStatusRepository>();
    services.AddScoped<ISubTaskRepository, SubTaskRepository>();
    services.AddScoped<IContactUsRepository, ContactUsRepository>();
    services.AddScoped<IDataExportRepository, DataExportRepository>();
    #endregion

    #region Services
    services.AddScoped<IWorkRequestService, WorkRequestService>();
    services.AddScoped<IViewAllWorkRequestsService, ViewAllWorkRequestsService>();
    services.AddScoped<INotificationService, NotificationService>();
    services.AddScoped<WorkRequestSubscriptionService>();
    services.AddScoped<WorkRequestSubscriptionUpdateService>();
    services.AddScoped<StaleRequestsService>();
    services.AddScoped<ITrialManagementService, TrialManagementService>();
    services.AddScoped<PullRequestService>();
    services.AddScoped<IUserUpdateService, UserUpdateService>();
    services.AddScoped<IContactUsService, ContactUsService>();
    services.AddScoped<SubTaskService>();
    services.AddScoped<IViewAllSubTasksService, ViewAllSubTasksService>();
    services.AddScoped<IDataExportService, DataExportService>();
    services.AddScoped<AssignGHIssuesService>();

        #endregion

        #region QuartzScheduler
        services.AddScoped<SubscriptionUpdateJob>();
    services.AddScoped<StaleRequestsJob>();
    services.AddScoped<AssignGHIssuesJob>();
    services.AddScoped<ProcessBulkExportJob>();
    services.AddScoped<CleanupBulkExportsJob>();

        // Add the required Quartz.NET services
        services.AddQuartz(q =>
    {

      q.UseSimpleTypeLoader();
      q.UseInMemoryStore();
      q.UseDefaultThreadPool(tp =>
          {
            tp.MaxConcurrency = 10;
          });

      JobKey subscriptionsJobKey = new("SubscriptionsUpdateJob");

      q.AddJob<SubscriptionUpdateJob>(subscriptionsJobKey, c => c
              .WithIdentity(subscriptionsJobKey)
          );

      q.AddTrigger(t => t
              .ForJob(subscriptionsJobKey)
              .WithIdentity("SubscriptionsUpdateJob-trigger")
              .WithCronSchedule("0 0 9 ? * * *")
          );


      JobKey staleRequestsJobKey = new("StaleRequestsJob");

      q.AddJob<StaleRequestsJob>(staleRequestsJobKey, c => c.WithIdentity(staleRequestsJobKey));

      q.AddTrigger(t => t
              .ForJob(staleRequestsJobKey)
              .WithIdentity("StaleRequestsJob-trigger")
              .WithCronSchedule("0 0 12 ? * WED")
          );

      JobKey assignGHIssuesJobKey = new("AssignGHIssuesJob");

      q.AddJob<AssignGHIssuesJob>(assignGHIssuesJobKey, c => c.WithIdentity(assignGHIssuesJobKey));

      q.AddTrigger(t => t
          .ForJob(assignGHIssuesJobKey)
          .WithIdentity("AssignGHIssuesJob-trigger")
          .WithCronSchedule("0 0 12 ? * *")
      );

      JobKey jobKey = new("ProcessBulkExportJob");

      q.AddJob<ProcessBulkExportJob>(opts => opts.WithIdentity(jobKey));

      q.AddTrigger(opts => opts
          .ForJob(jobKey)
          .WithIdentity("ProcessBulkExportJob-trigger")
          .WithSimpleSchedule(x => x
              .WithIntervalInMinutes(5)
              .RepeatForever()));

      JobKey cleanupBulkExportsJobKey = new("CleanupBulkExportsJob");

      q.AddJob<CleanupBulkExportsJob>(opts => opts.WithIdentity(cleanupBulkExportsJobKey));

      q.AddTrigger(opts => opts
              .ForJob(cleanupBulkExportsJobKey)
              .WithIdentity("CleanupBulkExportsJob-trigger")
              .WithCronSchedule("0 0 1 ? * * *") // Run daily at 1 AM
      );

    });


    services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    #endregion


    services.AddExpressiveAnnotations();

    QuestPDF.Settings.License = LicenseType.Community;
  }

  private void ConfigureAPISettings(IServiceCollection services)
  {
    string? apiToken = Configuration["ApiTokenConfig:ChaReqApiToken"];

    if (string.IsNullOrEmpty(apiToken))
    {
      throw new ArgumentNullException(nameof(apiToken), "API token not found.");
    }

    services.Configure<ApiTokenConfig>(options =>
    {
      options.ChaReqApiToken = apiToken;
    });
  }


  private void ConfigureDatabaseConnections(IServiceCollection services)
  {
    string? connectionString = WebHostEnvironment.IsDevelopment()
        ? Configuration.GetConnectionString("DefaultConnection")
        : !WebHostEnvironment.IsProduction()
            ? Configuration.GetConnectionString("TrainingDatabase")
            : Configuration.GetConnectionString("ProductionDatabase");

    string? ytuLibConnectionString = Configuration.GetConnectionString("YTULibraryConnection");

    if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(ytuLibConnectionString))
    {
      throw new Exception("Database connection string not found");
    }

    services.AddDbContextPool<ApplicationDbContext>(options =>
    {
      options.UseSqlServer(connectionString).ConfigureWarnings(w => w.Ignore(RelationalEventId.MultipleCollectionIncludeWarning))
              .EnableSensitiveDataLogging(Debugger.IsAttached); // Enable ONLY when run in VS
    });

    services.AddDbContextPool<YTULibDbContext>(t =>
        t.UseSqlServer(ytuLibConnectionString).ConfigureWarnings(w => w.Ignore(RelationalEventId.MultipleCollectionIncludeWarning)));
  }

  private void ConfigureGitHubOctokitSettings(IServiceCollection services)
  {
    services.Configure<OctokitConfigOptions>(Configuration.GetSection(OctokitConfigOptions.OctokitConfig));
  }

  // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
  public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
  {
    if (env.IsDevelopment())
    {
      app.UseDeveloperExceptionPage();
      app.UseMigrationsEndPoint();
    }
    else
    {
      app.UseExceptionHandler("/Home/Error");
      // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
      app.UseHsts();
    }
    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.Use(async (context, next) =>
    {

      context.Response.Headers.Append("X-Frame-Options", "DENY");
      context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
      context.Response.Headers.Append("Referrer-Policy", "no-referrer");

      string csp = "default-src 'self'; " +
                       "script-src 'self' 'unsafe-inline' https://cdn.datatables.net https://cdn.jsdelivr.net; " +
                       "style-src 'self' 'unsafe-inline' https://fonts.googleapis.com https://cdn.datatables.net https://cdn.jsdelivr.net https://code.jquery.com; " +
                       "font-src 'self' https://fonts.gstatic.com https://cdn.jsdelivr.net; " +
                       "connect-src 'self' https://fonts.googleapis.com https://cdn.jsdelivr.net; " +
                       "img-src 'self' data: blob:; " +
                       "object-src 'none'; " +
                       "frame-ancestors 'none'; " +
                       "form-action 'self' https://accounts.google.com; " +
                       "base-uri 'self';";

      if (env.IsDevelopment())
      {
        csp = csp.Replace("script-src 'self'", "script-src 'self' http://localhost:*");
        csp = csp.Replace("connect-src 'self'", "connect-src 'self' http://localhost:* ws://localhost:* wss://localhost:*");
      }

      context.Response.Headers.Append("Content-Security-Policy", csp);
      await next();
    });

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
      endpoints.MapControllerRoute(
              name: "default",
              pattern: "{controller=Home}/{action=Index}/{id?}");
      endpoints.MapRazorPages();
    });

    IServiceScopeFactory scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
    using IServiceScope scope = scopeFactory.CreateScope();
    DataAccessLayer.SeedDataService seedData = scope.ServiceProvider.GetService<DataAccessLayer.SeedDataService>();
    seedData.Initialise().GetAwaiter().GetResult();

  }

  public void ConfigureAuth(IServiceCollection services)
  {
    services.AddDefaultIdentity<ApplicationUser>(o =>
        {
          o.Password.RequiredLength = 9;
        }).AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();

    string? googleClientId = Configuration["GoogleConfig:ClientId"];
    string? googleClientSecret = Configuration["GoogleConfig:ClientSecret"];

    if (string.IsNullOrEmpty(googleClientId) || string.IsNullOrEmpty(googleClientSecret))
    {
      throw new Exception("Google client id or secret not found");
    }

    services.AddAuthentication()
        .AddGoogle(options =>
        {
          options.ClientId = googleClientId;
          options.ClientSecret = googleClientSecret;
        });

    services.ConfigureApplicationCookie(options =>
    {
      options.AccessDeniedPath = "/Identity/Account/AccessDenied";
      options.Cookie.Name = "AspNetCore.Identity";
      options.Cookie.HttpOnly = true;
      options.ExpireTimeSpan = TimeSpan.FromHours(2);
      options.LoginPath = "/Identity/Account/Login";
      options.SlidingExpiration = true;
    });

    services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
    {
      options.ValueCountLimit = 4096;
    });

    services.AddAuthorization(options =>
    {
      options.FallbackPolicy = new AuthorizationPolicyBuilder()
              .RequireAuthenticatedUser()
              .Build();
    });
  }

}