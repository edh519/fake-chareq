using BusinessLayer.Repositories;
using BusinessLayer.Repositories.Interfaces;
using BusinessLayer.Services.Interfaces;
using DataAccessLayer.External.Repos;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Octokit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
  public class AssignGHIssuesService
  {
    private readonly IWorkRequestRepository _workRequestRepository;
    private readonly GitHubApiRepository _gitHubApiRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWorkRequestService _workRequestService;
    private readonly PullRequestRepository _pullRequestRepository;
    public AssignGHIssuesService(IWorkRequestRepository workRequestRepository, GitHubApiRepository gitHubApiRepository, UserManager<ApplicationUser> userManager
      , IWorkRequestService workRequestService, PullRequestRepository pullRequestRepository)
    {
      _workRequestRepository = workRequestRepository;
      _gitHubApiRepository = gitHubApiRepository;
      _userManager = userManager;
      _workRequestService = workRequestService;
      _pullRequestRepository = pullRequestRepository;
    }
    public async Task RunAssignGHIssuesService(string viewWorkRequestUrl)
    {
      List<WorkRequest>? developerAssignedOpenWRs = _workRequestRepository.GetAllOpenWRsAssignedToDevelopers();
      if (developerAssignedOpenWRs == null) return;

      List<WorkRequest> workRequestsWithGHIssues = developerAssignedOpenWRs
        .Where(x => x.GitHubIssueNumber != null).ToList();
      if (workRequestsWithGHIssues == null) return;

      foreach (WorkRequest workRequest in workRequestsWithGHIssues)
      {
        // get gh issue info
        Issue ghIssue = await _gitHubApiRepository.GetIssueByIssueNumber((long)workRequest.AssignedTrialRepositoryId, (int)workRequest.GitHubIssueNumber);

        // get assignee(s) from gh
        List<Octokit.User> assignees = ghIssue.Assignees.ToList();
        if (assignees.Count == 0) continue;
        // assign user(s)
        int? assignedCounter = 0;
        foreach (Octokit.User assignee in assignees)
        {
          // parse the userId as sometimes it ends -uoy
          string? ghUserEmail = await _pullRequestRepository.GetEmailFromGitHubLogin(assignee.Login);

          if (ghUserEmail == null || string.IsNullOrEmpty(ghUserEmail)) continue;

          ApplicationUser user = await _userManager.FindByEmailAsync(ghUserEmail);
          // if you can't find the user, skip to the next assignee
          if (user == null) continue;
          // if already assigned, skip to the next assignee
          if (workRequest.Assignees.Any(a => a.NormalizedEmail == user.NormalizedEmail))
          {
            assignedCounter++;
            continue;
          }

          string linkUrl = $"{viewWorkRequestUrl}?workRequestId={workRequest.WorkRequestId}";

          await _workRequestService.AssignUserToWorkRequestAsync(workRequest.WorkRequestId, user.Email, null, linkUrl, true);
          assignedCounter++;
        }
        // unassign ytu devs

        // if there are no assignees on GH (or no successful assignment on ChaReq), keep ytu-developers assigned
        if (assignedCounter == 0) return;


        await _workRequestService.UnassignUserToWorkRequestAsync(workRequest.WorkRequestId, "ytu-developers-group@york.ac.uk", null, true);
      }
    }
  }
}
