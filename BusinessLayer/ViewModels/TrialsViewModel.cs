using DataAccessLayer.Models;
using Enums.Enums;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.ViewModels
{
    public class TrialsViewModel
    {
        public TrialsViewModel() { }
        public TrialsViewModel(IEnumerable<Trial> trials)
        {
            Trials = new();
            foreach (Trial trial in trials)
            {
                Trials.Add(new(trial));
            }
        }
        public TrialsViewModel(IEnumerable<TrialViewModel> trials)
        {
            Trials = new();
            foreach (TrialViewModel trial in trials)
            {
                Trials.Add(trial);
            }
        }

        public List<TrialViewModel> Trials { get; set; }
    }

    public class TrialViewModel
    {
        public TrialViewModel() { }
        public TrialViewModel(Trial trial)
        {
            TrialId = trial.TrialId;
            TrialName = trial.TrialName;
            IsActive = trial.IsActive;
            TrialAttribution = trial.TrialAttribution;
            TrialOwner = trial.TrialOwner;
            TrialEmail = trial.TrialEmail?.Email;
            
            ConfiguredRepositories = new();
        }

        public int TrialId { get; set; }
        [Display(Name = "Trial Name"), Required, MaxLength(25)]
        public string TrialName { get; set; }
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = false;
        [DisplayName("GitHub Repository Id")]
        public long? GitHubRepositoryId { get; set; }
        [DisplayName("Trial Email (Optional)"), EmailAddress]
        public string TrialEmail { get; set; }
        [DisplayName("Team Attribution (Optional)")]
        public TrialAttribution? TrialAttribution { get; set; }
        [DisplayName("Team Owner (Optional)")]
        public TrialAttribution? TrialOwner { get; set; }
        public List<ConfiguredTrialRepositoyViewModel> ConfiguredRepositories { get; set; }
    }
}
