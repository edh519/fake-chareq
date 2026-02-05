using System.Collections.Generic;
using Octokit;

namespace BusinessLayer.ViewModels;

public class GitHubViewModel
{
    public int WorkRequestId { get; set; }
    public long? RepositoryId { get; set; }
    public Issue Issue { get; set; }
    public List<Repository> TrialRepositoryInfos { get; set; }
    public string? ErrorMessage { get; set; }
}