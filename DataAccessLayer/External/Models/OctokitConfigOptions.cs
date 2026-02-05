using System.Collections.Generic;

namespace DataAccessLayer.External.Models
{
    public class OctokitConfigOptions
    {
        public const string OctokitConfig = "OctokitConfig";
        public string GitHubToken { get; set; }
        public string ProductHeaderValue { get; set; }
        public string UoYTrialsOrgName { get; set; }
        public string DevelopersTeamName { get; set; }
        public List<string> OrgBotAccounts { get; set; } = new();
    }
}