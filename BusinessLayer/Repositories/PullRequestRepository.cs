using DataAccessLayer;
using DataAccessLayer.Models;
using Enums.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Octokit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Repositories
{
  public class PullRequestRepository
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public PullRequestRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
      _userManager = userManager;
      _context = context;
    }
    public async Task<string?> GetEmailFromGitHubLogin(string login)
    {
      string userId = login.Split("-")[0];
      ApplicationUser? user = await _userManager.FindByNameAsync(userId);

      if (user == null)
      {
        return string.Empty;
      }

      return user.Email;
    }

    public List<WorkRequest> GetWRAssociatedWithIssue(long repositoryId, Issue issue)
    {
      List<WorkRequest> workRequests = _context.WorkRequests
          .Include(i => i.Status)
          .Include(i => i.Trial)
          .Include(i => i.SupportingFiles)
          .Include(i => i.Labels)
          .Include(i => i.Assignees)
          .Include(i => i.WorkRequestEvents).ThenInclude(i => i.EventType)
          .Include(i => i.ProcessDeviationReason)
          .AsSingleQuery()
          .Where(wr => wr.GitHubIssueNumber == issue.Number && wr.AssignedTrialRepositoryId == repositoryId && !(wr.Status.WorkRequestStatusId == WorkRequestStatusEnum.Completed || wr.Status.WorkRequestStatusId == WorkRequestStatusEnum.Abandoned))
          .ToList();
      return workRequests;
    }
  }
}
