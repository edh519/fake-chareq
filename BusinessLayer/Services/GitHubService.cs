using BusinessLayer.Helpers;
using BusinessLayer.Repositories.Interfaces;
using BusinessLayer.Services.EmailProcessing.EmailHelpers;
using BusinessLayer.Services.Models;
using DataAccessLayer.External.Repos;
using DataAccessLayer.Models;
using Enums;
using Enums.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BusinessLayer.Services;

public class GitHubService
{
  private readonly IWorkRequestRepository _workRequestRepository;
  private readonly RazorToHtmlParser _razorParser;
  private readonly ILogger<GitHubService> _logger;
  private readonly GitHubApiRepository _gitHubApiRepository;
  private readonly UserManager<ApplicationUser> _userManager;

  public GitHubService(IWorkRequestRepository workRequestRepository, RazorToHtmlParser razorParse,
      ILogger<GitHubService> logger, GitHubApiRepository gitHubApiRepository, UserManager<ApplicationUser> userManager)
  {
    _workRequestRepository = workRequestRepository;
    _razorParser = razorParse;
    _logger = logger;
    _gitHubApiRepository = gitHubApiRepository;
    _userManager = userManager;
  }

  public async Task<Issue> CreateWorkRequestGitHubIssue(int workRequestId, long? repositoryId, string chaReqUrl, string executorEmail)
  {
    WorkRequest workRequest = _workRequestRepository.GetWorkRequest(workRequestId);

    if (workRequest == null)
    {
      _logger.LogError("WorkRequest with ID {WorkRequestId} not found.", workRequestId);
      return null;
    }

    if (workRequest.GitHubIssueNumber.HasValue && workRequest.AssignedTrialRepositoryId.HasValue)
    {
      // An issue already exists for this work request.
      _logger.LogInformation("GitHub issue {IssueNumber} already exists for WorkRequest {WorkRequestId}.",
          workRequest.GitHubIssueNumber.Value, workRequest.WorkRequestId);

      try
      {
        Issue existingIssue = await _gitHubApiRepository.GetIssueByIssueNumber(workRequest.AssignedTrialRepositoryId.Value, workRequest.GitHubIssueNumber.Value);
        return existingIssue;
      }
      catch (NotFoundException)
      {
        _logger.LogWarning("Existing GitHub issue {IssueNumber} for WorkRequest {WorkRequestId} not found on GitHub. Attempting to re-create.",
            workRequest.GitHubIssueNumber.Value, workRequest.WorkRequestId);
        // If the issue was recorded in the DB but not found on GitHub, clear the references 
        workRequest.GitHubIssueNumber = null;
        workRequest.AssignedTrialRepositoryId = null;
        await _workRequestRepository.SaveAsync();
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error retrieving existing GitHub issue {IssueNumber} for WorkRequest {WorkRequestId}.",
            workRequest.GitHubIssueNumber.Value, workRequest.WorkRequestId);
        return null;
      }
    }

    if (!repositoryId.HasValue)
    {
      _logger.LogError("GitHub repositoryId null for {TrialName}. Unable to create issue.", workRequest.Trial.TrialName);
      return null;
    }

    Repository repository = await GetRepository(repositoryId.ToString());

    if (repository.Archived)
    {
      _logger.LogError("GitHub repository is archived for {TrialName}. Unable to create issue.", workRequest.Trial.TrialName);
      return null;
    }

    // Remove html elements
    string issueDescription = HtmlHelper.RemoveHtmlElementsFromString(workRequest.DetailDescription,
        replaceBrWithNewLine: true,
        keepImageElements: false);

    // Create issue body using the razor template
    ChangeRequestIssueTemplateModel model = new()
    {
      Approval = workRequest.WorkRequestEvents
                    .FirstOrDefault(e => e.EventType.WorkRequestEventTypeId == WorkRequestEventTypeEnum.Approve)
                    ?.Content is { } approvalContent
                    ? HtmlHelper.RemoveHtmlElementsFromString(approvalContent)
                    : "Awaiting decision.",
      Details = issueDescription,
      ChaReqUrl = chaReqUrl
    };

    string newIssueText = await _razorParser.RenderHtmlStringAsync("ChangeRequestIssueTemplate", model);

    List<GitHubLabelEnum> issueLabels = [GitHubLabelEnum.ToDiscuss, GitHubLabelEnum.ChaReq];

    if (workRequest.Status.WorkRequestStatusId == WorkRequestStatusEnum.PendingInitialApproval)
    {
      issueLabels.Add(GitHubLabelEnum.ApprovalNeeded);
    }

    NewIssue issue = new(workRequest.Reference)
    {
      Body = newIssueText
    };

    foreach (GitHubLabelEnum issueLabel in issueLabels)
    {
      issue.Labels.Add(EnumHelpers.GetDisplayName(issueLabel));
    }

    // Double check it hasn't been created in the meantime
    if (await _workRequestRepository.WorkRequestHasGitHubIssue(workRequestId))
    {
      return await _gitHubApiRepository.GetIssueByIssueNumber(workRequest.AssignedTrialRepositoryId.Value, workRequest.GitHubIssueNumber.Value);
    }

    Issue createdIssue = await _gitHubApiRepository.CreateIssue(repositoryId: repositoryId.Value, issue);


    workRequest.GitHubIssueNumber = createdIssue.Number;
    workRequest.AssignedTrialRepositoryId = repositoryId.Value;

    ApplicationUser executor = await _userManager.FindByEmailAsync(executorEmail);

    WorkRequestEvent workRequestEventAddition = new WorkRequestEvent
    {
      WorkRequest = workRequest,
      Content = $"{CommonHelpers.RemoveDomainFromEmail(executor.Email)} - created GitHub issue - #{workRequest.GitHubIssueNumber} ({repository.Name}) at {DateTime.Now:HH:mm}",
      CreatedAt = DateTime.Now,
      CreatedBy = executor,
      EventType = _workRequestRepository.GetWorkRequestEventType(WorkRequestEventTypeEnum.GHIssueAttachment)
    };

    _workRequestRepository.InsertWorkRequestEvent(workRequestEventAddition);

    _workRequestRepository.Save();

    _logger.LogInformation("Successfully created GitHub issue {IssueNumber} for WorkRequest {WorkRequestId}.",
        createdIssue.Number, workRequest.WorkRequestId);

    return createdIssue;
  }
  public async Task AddAssigneeToIssue(long repositoryId, int issueNumber, string userEmail)
  {
    Repository repo = await _gitHubApiRepository.GetRepository(repositoryId);

    if (repo.Archived)
    {
      _logger.LogWarning("GitHub repository is archived. Unable to add assignee to issue.");
      return;
    }

    IReadOnlyList<Octokit.User> possibleAssignees =
        await _gitHubApiRepository.GetAllPossibleAssigneesForRepository(repositoryId);

    Octokit.User userToAssign =
        possibleAssignees.FirstOrDefault(e =>
            string.Equals(userEmail, e.Email, StringComparison.OrdinalIgnoreCase));

    if (userToAssign is not null)
    {
      await _gitHubApiRepository.AddAssigneesToIssue(repo.Name, issueNumber,
          new AssigneesUpdate(new List<string>() { userToAssign.Login }));
    }
  }

  public async Task UpdateGitHubIssueWithAnalysis(WorkRequest workRequest)
  {
    if (workRequest.GitHubIssueNumber is null || workRequest.AssignedTrialRepositoryId is null)
    {
      return;
    }

    Repository repo = await _gitHubApiRepository.GetRepository((long)workRequest.AssignedTrialRepositoryId);

    if (repo.Archived)
    {
      _logger.LogWarning("GitHub repository is archived. Unable to add assignee to issue.");
      return;
    }


    WorkRequestEvent approvalEvent = workRequest.WorkRequestEvents
        .Where(e => e.EventType.WorkRequestEventTypeId == WorkRequestEventTypeEnum.Approve)
        .OrderByDescending(e => e.CreatedAt)
        .FirstOrDefault();

    if (approvalEvent is null) return;

    string comment = HtmlHelper.RemoveHtmlElementsFromString(approvalEvent.Content,
        replaceBrWithNewLine: true,
        keepImageElements: false);

    GitHubCommentTemplateModel model = new()
    {
      Details = comment,
      ActionedBy = approvalEvent.CreatedBy.Email,
    };

    string commentContent = await _razorParser.RenderHtmlStringAsync("GitHubCommentTemplate", model);


    await _gitHubApiRepository.AddCommentToIssue(workRequest.AssignedTrialRepositoryId.Value,
        workRequest.GitHubIssueNumber!.Value, commentContent);
  }

  public async Task UpdateWorkRequestWithGitHubIssue(int workRequestId, long repositoryId, int githubIssueNumber, string executorEmail)
  {

    WorkRequest workRequest = _workRequestRepository.GetWorkRequest(workRequestId);

    Repository repository = await GetRepository(repositoryId.ToString());

    if (repository.Archived)
    {
      _logger.LogWarning("GitHub repository is archived for {TrialName}. Unable to update issue.", workRequest.Trial.TrialName);
      return;
    }

    ApplicationUser executor = await _userManager.FindByEmailAsync(executorEmail);

    workRequest.AssignedTrialRepositoryId = repositoryId;

    if (workRequest.GitHubIssueNumber > 0)
    {
      WorkRequestEvent workRequestEventRemoval = new WorkRequestEvent
      {
        WorkRequest = workRequest,
        Content = $"{CommonHelpers.RemoveDomainFromEmail(executor.Email)} - removed GitHub issue - #{workRequest.GitHubIssueNumber} ({repository.Name}) at {DateTime.Now:HH:mm}",
        CreatedAt = DateTime.Now,
        CreatedBy = executor,
        EventType = _workRequestRepository.GetWorkRequestEventType(WorkRequestEventTypeEnum.GHIssueAttachment)
      };

      _workRequestRepository.InsertWorkRequestEvent(workRequestEventRemoval);
    }

    WorkRequestEvent workRequestEventAddition = new WorkRequestEvent
    {
      WorkRequest = workRequest,
      Content = $"{CommonHelpers.RemoveDomainFromEmail(executor.Email)} - added GitHub issue - #{githubIssueNumber} ({repository.Name}) at {DateTime.Now:HH:mm}",
      CreatedAt = DateTime.Now,
      CreatedBy = executor,
      EventType = _workRequestRepository.GetWorkRequestEventType(WorkRequestEventTypeEnum.GHIssueAttachment)
    };

    _workRequestRepository.InsertWorkRequestEvent(workRequestEventAddition);

    workRequest.GitHubIssueNumber = githubIssueNumber;

    _workRequestRepository.Save();
    return;
  }

  public async Task RemoveLabelFromIssue(long? repositoryId, int? issueNumber, params string[] labels)
  {
    if (repositoryId is null || issueNumber is null || labels is not { })
    {
      return;
    }

    // remove the label from the issue
    foreach (string label in labels)
    {
      try
      {
        await _gitHubApiRepository.RemoveLabelFromIssue(repositoryId.GetValueOrDefault(),
            issueNumber.GetValueOrDefault(), label);
      }

      catch (NotFoundException ex)
      {
        // Label not on the issue, continue.
      }
    }
  }

  public async Task<List<Repository>> GetRepositories(params long[] repositoryIds)
  {
    List<Repository> repositories = new();
    foreach (long repositoryId in repositoryIds)
    {
      try
      {
        Repository repository = await _gitHubApiRepository.GetRepository(repositoryId);
        repositories.Add(repository);
      }
      catch (NotFoundException ex)
      {
        _logger.LogInformation(ex, $"Repository with id {repositoryId} not found.");
      }
      catch (ApiException ex)
      {
        _logger.LogError(ex, $"GitHub API error fetching repository with id {repositoryId}. Status Code: {ex.StatusCode}");
      }
      catch (HttpRequestException ex)
      {
        _logger.LogError(ex, $"Network error fetching GitHub repository with id {repositoryId}.");
      }
      catch (Exception ex) // General catch-all for unexpected errors
      {
        _logger.LogError(ex, $"An unexpected error occurred while fetching GitHub repository with id {repositoryId}.");
      }
    }
    return repositories;
  }

  public async Task<Repository> GetRepository(string repoIdOrName)
  {
    if (string.IsNullOrEmpty(repoIdOrName))
    {
      return default;
    }
    // Check if the repoIdOrName is a long or a string
    bool isLongId = long.TryParse(repoIdOrName, out long repositoryId);

    if (!isLongId)
    {
      repoIdOrName = repoIdOrName.Contains("/") ? GetRepoNameFromUrl(repoIdOrName) : repoIdOrName;
    }
    try
    {
      Repository repository = isLongId ? await _gitHubApiRepository.GetRepository(repositoryId) : await _gitHubApiRepository.GetRepository(repoIdOrName);
      return repository;
    }
    catch (NotFoundException)
    {
      return default;
    }
  }
  private string GetRepoNameFromUrl(string url)
  {
    string[] urlParts = url.Split('/');
    return urlParts.Last();
  }

  public async Task ResetGitHubInfo(int workRequestId, string executorEmail)
  {
    WorkRequest workRequest = _workRequestRepository.GetByID(workRequestId);

    if (workRequest is not null)
    {
      ApplicationUser executor = await _userManager.FindByEmailAsync(executorEmail);
      Repository repository = await GetRepository(workRequest.AssignedTrialRepositoryId.ToString());

      WorkRequestEvent workRequestEventRemoval = new WorkRequestEvent
      {
        WorkRequest = workRequest,
        Content = $"{CommonHelpers.RemoveDomainFromEmail(executor.Email)} - removed GitHub issue - #{workRequest.GitHubIssueNumber} ({repository.Name}) at {DateTime.Now:HH:mm}",
        CreatedAt = DateTime.Now,
        CreatedBy = executor,
        EventType = _workRequestRepository.GetWorkRequestEventType(WorkRequestEventTypeEnum.GHIssueAttachment)
      };

      workRequest.AssignedTrialRepositoryId = null;
      workRequest.GitHubIssueNumber = null;

      _workRequestRepository.InsertWorkRequestEvent(workRequestEventRemoval);
      await _workRequestRepository.SaveAsync();
    }
  }
}