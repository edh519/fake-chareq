using DataAccessLayer.Models;
using Enums.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DataAccessLayer.External.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using DataAccessLayer.External.Repos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Octokit;
using Label = DataAccessLayer.Models.Label;

namespace DataAccessLayer
{
    public class SeedDataService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly GitHubApiRepository _gitHubApiRepository;
        private readonly ILogger<SeedDataService> _logger;
        private readonly OctokitConfigOptions _octokitConfigOptions;

        public SeedDataService(ApplicationDbContext applicationDbContext,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment webHostEnvironment,
            GitHubApiRepository gitHubApiRepository,
            ILogger<SeedDataService> logger,
            IOptions<OctokitConfigOptions> octokitConfigOptions)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _gitHubApiRepository = gitHubApiRepository;
            _logger = logger;
            _octokitConfigOptions = octokitConfigOptions.Value;
            _applicationDbContext = applicationDbContext;
        }
        public async Task Initialise()
        {
            List<SeedUser> seedUsers = await GetSeedUsers();

            await AddIdentityRoles();
            await AddUsersAndTheirRolesAndClaims(seedUsers);
            await AddLabelsAsync();
            await AddTrialsAsync();
        }

        private async Task SeedRotaDays()
        {
            List<TriageInfoRota> rotaDays = new()
            {
                new TriageInfoRota() { Day = WeekdayEnum.Monday },
                new TriageInfoRota() { Day = WeekdayEnum.Tuesday },
                new TriageInfoRota() { Day = WeekdayEnum.Wednesday },
                new TriageInfoRota() { Day = WeekdayEnum.Thursday },
                new TriageInfoRota() { Day = WeekdayEnum.Friday }
            };

            for (int i = 0; i < rotaDays.Count; i++)
            {
                if (!_applicationDbContext.TriageInfoRotas.Select(x => x.Day).Contains(rotaDays[i].Day))
                {
                    await _applicationDbContext.TriageInfoRotas.AddAsync(rotaDays[i]);
                }
            }
            await _applicationDbContext.SaveChangesAsync();
        }

        private async Task AddTrialsAsync()
        {
            if (_webHostEnvironment.IsDevelopment()) // add in some fake trials to pad out/test functionality more
            {
                List<ApplicationUser> usersToCreate = new List<ApplicationUser>
                {
                    new ApplicationUser
                    {
                        UserName = "github-testing-trial",
                        NormalizedUserName = "GITHUB-TESTING-TRIAL",
                        Email = "github-testing-trial@york.ac.uk",
                        NormalizedEmail = "GITHUB-TESTING-TRIAL@YORK.AC.UK",
                        isSystemAccount = true
                    },
                    new ApplicationUser
                    {
                        UserName = "soap-trial",
                        NormalizedUserName = "SOAP-TRIAL",
                        Email = "soap-trial@york.ac.uk",
                        NormalizedEmail = "SOAP-TRIAL@YORK.AC.UK",
                        isSystemAccount = true
                    },
                    new ApplicationUser
                    {
                        UserName = "test-trial",
                        NormalizedUserName = "TEST-TRIAL",
                        Email = "test-trial@york.ac.uk",
                        NormalizedEmail = "TEST-TRIAL@YORK.AC.UK",
                        isSystemAccount = true
                    },
                    new ApplicationUser
                    {
                        UserName = "darling-trial",
                        NormalizedUserName = "DARLING-TRIAL",
                        Email = "darling-trial@york.ac.uk",
                        NormalizedEmail = "DARLING-TRIAL@YORK.AC.UK",
                        isSystemAccount = true
                    }
                };

                foreach (ApplicationUser user in usersToCreate)
                {
                    ApplicationUser existingUser = await _userManager.FindByEmailAsync(user.Email);
                    if (existingUser == null)
                    {
                        await _userManager.CreateAsync(user);
                    }
                }

                List<ApplicationUser> users = new List<ApplicationUser>();

                foreach (ApplicationUser u in usersToCreate)
                {
                    ApplicationUser user = await _userManager.FindByEmailAsync(u.Email);
                    users.Add(user);
                }

                List<Trial> trials = new()
                {
                    new Trial { TrialName = "github-testing-githubapi", TrialEmail = users.First(u => u.Email == "github-testing-trial@york.ac.uk"), IsActive = true },
                    new Trial { TrialName = "SOAP (no github)", TrialEmail = users.First(u => u.Email == "soap-trial@york.ac.uk"), IsActive = true },
                    new Trial { TrialName = "Test (no github)", TrialEmail = users.First(u => u.Email == "test-trial@york.ac.uk"), IsActive = true },
                    new Trial { TrialName = "DaRLinG (no github)", TrialEmail = users.First(u => u.Email == "darling-trial@york.ac.uk"), IsActive = true }
                };

                for (int i = 0; i < trials.Count; i++)
                {
                    if (!_applicationDbContext.Trials.Select(x => x.TrialName).Contains(trials[i].TrialName))
                    {
                        _ = await _applicationDbContext.Trials.AddAsync(trials[i]);
                    }
                }
                await _applicationDbContext.SaveChangesAsync();
            }
        }

        private async Task AddLabelsAsync()
        {
            List<Label> labels = new()
            {
                new Label() { LabelShort = "github", LabelDescription = "Added to GitHub", HexColor = "#104a66" },
                new Label() { LabelShort = "TMS", LabelDescription = "TMS query for the dev team", HexColor = "#447ffd" },
                new Label() { LabelShort = "data request", LabelDescription = "A DatReq or TMS data request", HexColor = "#131129" },
            };

            for (int i = 0; i < labels.Count; i++)
            {
                if (!_applicationDbContext.Label.Select(x => x.LabelShort).Contains(labels[i].LabelShort))
                {
                    await _applicationDbContext.Label.AddAsync(labels[i]);
                }
            }
            await _applicationDbContext.SaveChangesAsync();

        }

        private async Task<List<SeedUser>> GetSeedUsers()
        {
            List<SeedUser> seedUsers = new() {
                new() { Username = "ytu-datavalidation-group", Email = "ytu-datavalidation-group@york.ac.uk", IsLocked = false },
                new() { Username = "ytu-developers-group", Email = "ytu-developers-group@york.ac.uk", IsLocked = false },
                new() { Username = "ytu-redcap-group", Email = "ytu-redcap-group@york.ac.uk", IsLocked = false },
                new() { Username = "system", Email = "system@york.ac.uk", IsLocked = false, IsSystemAccount = true }
            };

            if (!_webHostEnvironment.IsDevelopment()) return seedUsers;

            // Dev environment. seed users via github
            try
            {
                IReadOnlyList<Team> allOrgTeams = await _gitHubApiRepository.GetAllOrgTeams();

                if (!allOrgTeams.Any())
                {
                    _logger.LogError("Unable to seed users from github org. No teams found.");
                    return seedUsers;
                }

                Team devTeam = allOrgTeams.FirstOrDefault(x => string.Equals(x.Name, _octokitConfigOptions.DevelopersTeamName, StringComparison.OrdinalIgnoreCase));
                if (devTeam is null)
                {
                    _logger.LogError($"Unable to seed users from github org team: {nameof(_octokitConfigOptions.DevelopersTeamName)}. Team not found.");
                    return seedUsers;
                }

                IReadOnlyList<Octokit.User> allDevTeamMembers = await _gitHubApiRepository.GetAllTeamMembers(devTeam.Id);

                // Exclude bot accounts from seeding.
                allDevTeamMembers = allDevTeamMembers.Where(e => 
                        !_octokitConfigOptions.OrgBotAccounts.Any(m => string.Equals(m, e.Login, StringComparison.OrdinalIgnoreCase)))
                    .ToList();

                foreach (Octokit.User allUoyOrgMember in allDevTeamMembers)
                {
                    Octokit.User userDetails = await _gitHubApiRepository.GetGitHubUser(allUoyOrgMember.Login);
                    if (userDetails.Name is not null)
                    {
                        seedUsers.Add(new()
                        {
                            Username = userDetails.Login,
                            Email = !string.IsNullOrWhiteSpace(userDetails.Email)
                                ? userDetails.Email
                                : $"{(string.Join(".", userDetails.Name.ToLower().Split(" ")))}@york.ac.uk"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return seedUsers;
        }

        private async Task AddIdentityRoles()
        {
            RoleStore<IdentityRole> roleStore = new(_applicationDbContext);

            foreach (IdentityRoleEnum role in Enum.GetValues(typeof(IdentityRoleEnum)))
            {
                string roleAsString = role.ToString();
                if (!_applicationDbContext.Roles.Any(r => r.NormalizedName == roleAsString.ToUpper()))
                {
                    Enum.TryParse(roleAsString, out IdentityRoleEnum roleEnum);
                    await roleStore.CreateAsync(new IdentityRole { Id = ((int)roleEnum).ToString(), Name = roleAsString, NormalizedName = roleAsString.ToUpper() });
                }
            }
        }

        private async Task AddUsersAndTheirRolesAndClaims(List<SeedUser> seedUsers)
        {
            foreach (SeedUser seedUser in seedUsers)
            {
                await AddUser(seedUser);
                await AddRole(seedUser.Email, IdentityRoleEnum.Authoriser.ToString());
                await AddClaims(seedUser);
            }
        }

        private async Task AddClaims(SeedUser seedUser)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(seedUser.Email);
            IList<Claim> userClaims = await _userManager.GetClaimsAsync(user);

            if (!userClaims.Any(x => x.Type == QuasiClaim.Username))
            {
                await _userManager.AddClaimAsync(user, new Claim(QuasiClaim.Username, seedUser.Username));
            }
        }

        private async Task AddRole(string email, string role)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            if (!await _userManager.IsInRoleAsync(user, role))
            {
                await _userManager.UpdateSecurityStampAsync(user);
                await _userManager.UpdateNormalizedEmailAsync(user);
                await _userManager.UpdateNormalizedUserNameAsync(user);
                await _userManager.SetLockoutEnabledAsync(user, true);

                await _userManager.AddToRoleAsync(user, role);
            }
        }

        private async Task AddUser(SeedUser seedUser)
        {
            UserStore<ApplicationUser> userStore = new(_applicationDbContext);

            ApplicationUser existingUser = userStore.Users.FirstOrDefault(e =>
                e.NormalizedEmail == seedUser.Email.ToUpper() ||
               e.NormalizedUserName.ToUpper() == seedUser.Username.ToUpper());

            // If user doesn't already exist create and return.
            if (existingUser is null)
            {
                ApplicationUser user = new()
                {
                    Email = seedUser.Email,
                    NormalizedEmail = seedUser.Email.ToUpper(),
                    UserName = seedUser.Username,
                    NormalizedUserName = seedUser.Username.ToUpper(),
                };

                if (seedUser.IsLocked) // If no login lockout user by default.
                {
                    user.LockoutEnd = DateTime.MaxValue;
                    user.LockoutEnabled = true;
                }

                await userStore.CreateAsync(user);
                return;
            }


            // User already exists, check for updated email/username.
            if (!string.Equals(existingUser.Email, seedUser.Email, StringComparison.OrdinalIgnoreCase))
            {
                existingUser.Email = seedUser.Email;
                existingUser.NormalizedEmail = seedUser.Email.ToUpper();
                await _userManager.UpdateAsync(existingUser);
            }

            if (!string.Equals(existingUser.UserName, seedUser.Username, StringComparison.OrdinalIgnoreCase))
            {
                existingUser.UserName = seedUser.Username;
                existingUser.NormalizedUserName = seedUser.Username.ToUpper();
                await _userManager.UpdateAsync(existingUser);
            }
        }

        internal struct SeedUser
        {
            public string Email { get; set; }
            public string Username { get; set; }
            public bool IsLocked { get; set; }
            public bool IsSystemAccount { get; set; }
        }
    }
}
