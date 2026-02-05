using DataAccessLayer.Models;
using Enums;
using Enums.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq;

namespace DataAccessLayer
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

            //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetIsTemporal(true);
            }

            // Seed data added here!
            #region Seed Data for Types
            modelBuilder
                .Entity<ApplicationUser>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
            modelBuilder
                .Entity<IdentityRole>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();


            // Work Request Status
            modelBuilder
                .Entity<WorkRequestStatus>()
                .Property(e => e.WorkRequestStatusId)
                .HasConversion<int>();
            modelBuilder
                .Entity<WorkRequestStatus>().HasData(
                    Enum.GetValues(typeof(WorkRequestStatusEnum))
                        .Cast<WorkRequestStatusEnum>()
                        .Select(e => new WorkRequestStatus()
                        {
                            WorkRequestStatusId = e,
                            WorkRequestStatusName = EnumHelpers.GetDisplayName(e),
                            IsActive = EnumHelpers.GetIsActive(e)
                        })
                );

            // Work Request Event Type
            modelBuilder
                .Entity<WorkRequestEventType>()
                .Property(e => e.WorkRequestEventTypeId)
                .HasConversion<int>();
            modelBuilder
                .Entity<WorkRequestEventType>().HasData(
                    Enum.GetValues(typeof(WorkRequestEventTypeEnum))
                        .Cast<WorkRequestEventTypeEnum>()
                        .Select(e => new WorkRequestEventType()
                        {
                            WorkRequestEventTypeId = e,
                            WorkRequestEventTypeName = EnumHelpers.GetDisplayName(e),
                            IsActive = EnumHelpers.GetIsActive(e)
                        })
                );


            // Email Type
            modelBuilder
                .Entity<EmailType>()
                .Property(e => e.EmailTypeId)
                .HasConversion<int>();
            modelBuilder
                .Entity<EmailType>().HasData(
                    Enum.GetValues(typeof(EmailTypeEnum))
                        .Cast<EmailTypeEnum>()
                        .Select(e => new EmailType()
                        {
                            EmailTypeId = e,
                            EmailTypeName = e.ToString(), // These are internal only, no need for display name.
                            IsActive = EnumHelpers.GetIsActive(e)
                        })
                );

            // Notification Type
            modelBuilder
                .Entity<NotificationType>()
                .Property(e => e.NotificationTypeId)
                .HasConversion<int>();
            modelBuilder
                .Entity<NotificationType>().HasData(
                    Enum.GetValues(typeof(NotificationTypeEnum))
                        .Cast<NotificationTypeEnum>()
                        .Select(e => new NotificationType()
                        {
                            NotificationTypeId = e,
                            NotificationTypeName = EnumHelpers.GetDisplayName(e),
                            IsActive = EnumHelpers.GetIsActive(e)
                        })
                );

            // Sub Task Status
            modelBuilder
                .Entity<SubTaskStatus>()
                .Property(e => e.SubTaskStatusId)
                .HasConversion<int>();
            modelBuilder
                .Entity<SubTaskStatus>().HasData(
                    Enum.GetValues(typeof(SubTaskStatusEnum))
                        .Cast<SubTaskStatusEnum>()
                        .Select(e => new SubTaskStatus()
                        {
                            SubTaskStatusId = e,
                            SubTaskStatusName = EnumHelpers.GetDisplayName(e),
                            IsActive = EnumHelpers.GetIsActive(e)
                        })
                );

            modelBuilder
                .Entity<SubTaskEventType>()
                .Property(e => e.SubTaskEventTypeId)
                .HasConversion<int>();
            modelBuilder
                .Entity<SubTaskEventType>().HasData(
                    Enum.GetValues(typeof(SubTaskEventTypeEnum))
                        .Cast<SubTaskEventTypeEnum>()
                        .Select(e => new SubTaskEventType()
                        {
                            SubTaskEventTypeId = e,
                            SubTaskEventTypeName = EnumHelpers.GetDisplayName(e),
                            IsActive = EnumHelpers.GetIsActive(e)
                        })
                );

            modelBuilder
                .Entity<DataExportStatus>()
                .Property(e => e.DataExportStatusId)
                .HasConversion<int>();
            modelBuilder
                .Entity<DataExportStatus>().HasData(
                    Enum.GetValues(typeof(DataExportStatusEnum))
                        .Cast<DataExportStatusEnum>()
                        .Select(e => new DataExportStatus()
                        {
                            DataExportStatusId = e,
                            DataExportStatusName = e.ToString(),
                            IsActive = true
                        })
                );


            // Trials
            // Trials that are selectable in the trials drop-down.
            modelBuilder.Entity<Trial>().HasData(
                // Inactive/Old trials
                //new Trial { TrialId = 2, TrialName = "AVURT", IsActive = true },
                //new Trial { TrialId = 2, TrialName = "Breathe", IsActive = true },
                //new Trial { TrialId = 2, TrialName = "CHAMP", IsActive = true },
                //new Trial { TrialId = 2, TrialName = "CHEMIST", IsActive = true },
                //new Trial { TrialId = 2, TrialName = "FARSTER", IsActive = true },
                //new Trial { TrialId = 2, TrialName = "KReBS", IsActive = true },
                //new Trial { TrialId = 2, TrialName = "MiQuit", IsActive = true },
                //new Trial { TrialId = 2, TrialName = "MyYTU", IsActive = true },
                //new Trial { TrialId = 2, TrialName = "OTIS", IsActive = true },
                //new Trial { TrialId = 2, TrialName = "PACTM", IsActive = true },
                //new Trial { TrialId = 2, TrialName = "PRESTO", IsActive = true },
                //new Trial { TrialId = 2, TrialName = "PROMOTE", IsActive = true },
                //new Trial { TrialId = 2, TrialName = "PROTECT", IsActive = true },
                //new Trial { TrialId = 2, TrialName = "REFORM", IsActive = true },
                //new Trial { TrialId = 2, TrialName = "RESPECT", IsActive = true },
                //new Trial { TrialId = 2, TrialName = "SCIMITARPlus", IsActive = true },
                //new Trial { TrialId = 2, TrialName = "SSHeW", IsActive = true },
                //new Trial { TrialId = 2, TrialName = "StandaloneRand", IsActive = true },
                //new Trial { TrialId = 2, TrialName = "SWIFFT", IsActive = true },
                //new Trial { TrialId = 2, TrialName = "TBTobacco", IsActive = true },
                //new Trial { TrialId = 2, TrialName = "TrialTemplate", IsActive = true },
                //new Trial { TrialId = 2, TrialName = "TrialTemplateMVC", IsActive = true },
                //new Trial { TrialId = 2, TrialName = "UKFROST", IsActive = true },

                // Active/Relevant trials
                new Trial { TrialId = 1, TrialName = "ACTIVE", IsActive = true },
                new Trial { TrialId = 2, TrialName = "ASSSIST-2", IsActive = true },
                new Trial { TrialId = 3, TrialName = "BASIL", IsActive = true },
                new Trial { TrialId = 4, TrialName = "BATH-OUT-2", IsActive = true },
                new Trial { TrialId = 5, TrialName = "BRIGHT", IsActive = true },
                new Trial { TrialId = 6, TrialName = "_ChangeRequest", IsActive = true },
                new Trial { TrialId = 7, TrialName = "DIAMONDS", IsActive = true },
                new Trial { TrialId = 8, TrialName = "DISC [CTIMP]", IsActive = true },
                new Trial { TrialId = 9, TrialName = "_ETMA", IsActive = true },
                new Trial { TrialId = 10, TrialName = "Firefli", IsActive = true },
                new Trial { TrialId = 11, TrialName = "GYY", IsActive = true },
                new Trial { TrialId = 12, TrialName = "L1FE", IsActive = true },
                new Trial { TrialId = 13, TrialName = "MODS", IsActive = true },
                new Trial { TrialId = 14, TrialName = "OSTRICH", IsActive = true },
                new Trial { TrialId = 15, TrialName = "PACT", IsActive = true },
                new Trial { TrialId = 16, TrialName = "ProFHER-2", IsActive = true },
                new Trial { TrialId = 17, TrialName = "SHEDSSc", IsActive = true },
                new Trial { TrialId = 18, TrialName = "SNAP2", IsActive = true },
                new Trial { TrialId = 19, TrialName = "SOFFT", IsActive = true },
                new Trial { TrialId = 20, TrialName = "_SOPManager", IsActive = true },
                new Trial { TrialId = 21, TrialName = "STEPFORWARD", IsActive = true },
                new Trial { TrialId = 22, TrialName = "SWHSI-II", IsActive = true },
                new Trial { TrialId = 23, TrialName = "VenUS6", IsActive = true },
                new Trial { TrialId = 1000, TrialName = "Other", IsActive = true }
                );
            #endregion
        }

        #region Core Work Request

        public DbSet<WorkRequest> WorkRequests { get; set; }
        public DbSet<WorkRequestEvent> WorkRequestEvents { get; set; }
        public DbSet<InitialAuthorisation> InitialAuthorisations { get; set; }
        public DbSet<FinalAuthorisation> FinalAuthorisations { get; set; }
        public DbSet<ProcessDeviationReason> ProcessDeviationReasons { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<IdentityRole> IdentityRoles { get; set; }
        public DbSet<Label> Label { get; set; }
        public DbSet<Template> Templates { get; set; }
        #endregion

        #region File Uploads

        public DbSet<FileUpload> FileUploads { get; set; }

        #endregion

        #region Types

        public DbSet<Trial> Trials { get; set; }
        public DbSet<WorkRequestStatus> WorkRequestStatuses { get; set; }
        public DbSet<WorkRequestEventType> WorkRequestEventTypes { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<EmailType> EmailTypes { get; set; }

        #endregion

        #region Notifications, Emails, Subscriptions and Information
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<SentEmail> SentEmails { get; set; }
        public DbSet<WorkRequestSubscription> WorkRequestSubscriptions { get; set; }
        public DbSet<TriageInfoRota> TriageInfoRotas { get; set; }
        public DbSet<TrialRepositoryInfo> TrialRepositoryInfos { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
    

        #endregion

        public DbSet<DataExportLog> DataExportLogs { get; set; }
        public DbSet<DataExportJob> DataExportJobs { get; set; }
        public DbSet<DataExportStatus> DataExportStatuses { get; set; }

        #region Sub Tasks
        public DbSet<SubTask> SubTasks { get; set; }
        public DbSet<SubTaskStatus> SubTaskStatuses { get; set; }
        public DbSet<SubTaskEvent> SubTaskEvents { get; set; }
        public DbSet<SubTaskEventType> SubTaskEventTypes { get; set; }
        #endregion
    }
}
