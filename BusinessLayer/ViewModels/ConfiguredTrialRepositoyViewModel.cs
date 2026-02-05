using Octokit;

namespace BusinessLayer.ViewModels;

public class ConfiguredTrialRepositoyViewModel
{
    public Repository Repository { get; set; }
    public bool HasAssociatedWorkRequests { get; set; }    
}