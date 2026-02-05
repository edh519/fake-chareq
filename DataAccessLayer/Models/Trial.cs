using Enums.Enums;
using System.Collections.Generic;

namespace DataAccessLayer.Models
{
    public class Trial
    {
        public int TrialId { get; set; }
        public string TrialName { get; set; }
        public bool IsActive { get; set; }
        public TrialAttribution? TrialAttribution { get; set; }
        public TrialAttribution? TrialOwner { get; set; }
        public ApplicationUser? TrialEmail { get; set; }
        public string? TrialEmailId { get; set; }
        public ICollection<TrialRepositoryInfo> TrialRepositoryInfos { get; } = new List<TrialRepositoryInfo>();
    }
}
