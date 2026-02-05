using BusinessLayer.Helpers;
using BusinessLayer.Repositories.Interfaces;
using BusinessLayer.Services.Interfaces;
using BusinessLayer.ViewModels;
using DataAccessLayer.External.Repos;
using DataAccessLayer.Models;
using Enums.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
  public class WorkRequestService : IWorkRequestService
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<WorkRequestService> _logger;
    private readonly IWorkRequestRepository _workRequestRepository;
    private readonly ILabelRepository _labelRepository;
    private readonly GitHubApiRepository _gitHubApiRepository;
    private readonly ITrialRepositoryInfoRepository _trialRepositoryInfoRepository;
    private readonly GitHubService _gitHubService;
    private readonly SubTaskService _subTaskService;
    private readonly ISubTaskRepository _subTaskRepository;
    private readonly INotificationService _notificationService;
    private readonly IWorkRequestSubscriptionRepository _workRequestSubscriptionRepository;
    private readonly WorkRequestSubscriptionService _workRequestSubscriptionService;

    public WorkRequestService(UserManager<ApplicationUser> userManager, ILogger<WorkRequestService> logger, IWorkRequestRepository workRequestRepository,
        ILabelRepository labelRepository, GitHubApiRepository gitHubApiRepository, GitHubService gitHubService, ITrialRepositoryInfoRepository trialRepositoryInfoRepository,
        SubTaskService subTaskService, ISubTaskRepository subTaskRepository, INotificationService notificationService, IWorkRequestSubscriptionRepository workRequestSubscriptionRepository,
        WorkRequestSubscriptionService workRequestSubscriptionService)
    {
      _userManager = userManager;
      _logger = logger;
      _workRequestRepository = workRequestRepository;
      _labelRepository = labelRepository;
      _gitHubApiRepository = gitHubApiRepository;
      _gitHubService = gitHubService;
      _trialRepositoryInfoRepository = trialRepositoryInfoRepository;
      _subTaskService = subTaskService;
      _subTaskRepository = subTaskRepository;
      _notificationService = notificationService;
      _workRequestSubscriptionRepository = workRequestSubscriptionRepository;
      _workRequestSubscriptionService = workRequestSubscriptionService;
    }


    public async Task<WorkRequestDetailsViewModel> GetWorkRequestById(int workRequestId, string userEmail)
    {
      WorkRequest workRequest = _workRequestRepository.GetWorkRequest(workRequestId);
      IEnumerable<FileUpload> supportingFiles = _workRequestRepository.GetFileUploadsByWorkRequestId(workRequestId);
      IEnumerable<WorkRequestEvent> workRequestEvents = _workRequestRepository.GetWorkRequestEvents(workRequestId);
      IEnumerable<SubTaskEvent> subTaskEvents = _subTaskRepository.GetAllSubTaskEventsForWorkRequest(workRequestId);

      if (workRequest == null)
        return null;

      WorkRequestDetailsViewModel workRequestDetailsViewModel = new(workRequest);
      workRequestDetailsViewModel.LabelsEditorViewModel.AllLabels = new(_labelRepository.GetUnarchivedLabels().OrderBy(o => o.LabelShort));
      workRequestDetailsViewModel.AssigneesViewModel = new()
      {
        WorkRequestId = workRequestId,
        AssignedUsers = GetAssignedUsers(workRequest)
      };
      workRequestDetailsViewModel.SubTaskAddViewModel = new()
      {
        WorkRequestId = workRequestId,
        AssignableUsers = GetAssignableUsers().ToList()
      };
      workRequestDetailsViewModel.SubscribeOtherUsersViewModel = new()
      {
        WorkRequestId = workRequestId,
        SubscribableUsers = await GetSubscribableUsers(workRequestId)
      };
      workRequestDetailsViewModel.SubTaskEvents = subTaskEvents.Select(x => new SubTaskEventViewModel()
      {
        Content = x.Content,
        CreatedAt = x.CreatedAt,
        SubTaskEventType = x.EventType.SubTaskEventTypeId,
        CreatedByEmail = x.CreatedBy.Email,
        SubTaskEventId = x.SubTaskEventId,
        SubTaskId = x.SubTask.SubTaskId
      })?.ToList();
      workRequestDetailsViewModel.SubTaskViewModel.WorkRequestId = workRequestId;
      workRequestDetailsViewModel.SubTaskViewModel.SubTasks = workRequest.SubTasks.Select(subTask => new SubTaskAccordionViewModel
      {
        SubTask = subTask,
        SubTaskEvents = workRequestDetailsViewModel.SubTaskEvents
                  .Where(e => e.SubTaskId == subTask.SubTaskId)
                  .ToList()
      }).ToList();
      workRequestDetailsViewModel
          .AssigneesViewModel.AssignableUsers = GetAuthorisers(isActiveOnly: true)
                                                  .Where(q =>
                                                      !workRequestDetailsViewModel
                                                      .AssigneesViewModel.AssignedUsers.Any(x => x.UserId == q.UserId))
                                                  ?.OrderBy(o => o.Username)?.ToList() ?? new();

      workRequestDetailsViewModel.WorkRequestEvents = workRequestEvents.Select(x => new WorkRequestEventViewModel()
      {
        Content = x.Content,
        CreatedAt = x.CreatedAt,
        WorkRequestEventType = x.EventType.WorkRequestEventTypeId,
        DurationDays = x.DurationDays,
        CreatedByEmail = x.CreatedBy.Email,
        WorkRequestEventId = x.WorkRequestEventId,
        WorkRequestId = x.WorkRequest.WorkRequestId
      })?.ToList();

      workRequestDetailsViewModel.Trial = workRequest.Trial.TrialName;
      workRequestDetailsViewModel.TrialEmail = workRequest.Trial?.TrialEmail;
      workRequestDetailsViewModel.SupportingFiles = supportingFiles;
      workRequestDetailsViewModel.ProcessDeviationReason = workRequest?.ProcessDeviationReason;

      CheckUserMultipleAuthorisations(workRequestDetailsViewModel, userEmail);

      // GitHub
      workRequestDetailsViewModel.GitHubViewModel = new()
      {
        RepositoryId = workRequest.AssignedTrialRepositoryId,
        WorkRequestId = workRequest.WorkRequestId,
        TrialRepositoryInfos = await _gitHubService.GetRepositories(_trialRepositoryInfoRepository.GetByTrialId(workRequest.Trial.TrialId).Select(e => e.GitHubRepositoryId).ToArray())
      };

      if (workRequest.GitHubIssueNumber is not null && workRequest.AssignedTrialRepositoryId is not null)
      {
        try
        {
          workRequestDetailsViewModel.GitHubViewModel.Issue =
              await _gitHubApiRepository.GetIssueByIssueNumber(
                  workRequest.AssignedTrialRepositoryId.Value,
                  workRequestDetailsViewModel.GitHubIssueNumber.GetValueOrDefault());
        }
        catch (Octokit.NotFoundException ex)
        {
          _logger.LogInformation(ex,
              $"Failed to retrieve GitHub issue for WorkRequestId: {workRequest.WorkRequestId}, RepositoryId: {workRequest.AssignedTrialRepositoryId}");
        }
        catch (Octokit.ApiException ex) when (ex.StatusCode == HttpStatusCode.ServiceUnavailable)
        {
          workRequestDetailsViewModel.GitHubViewModel.ErrorMessage = "Could not load GitHub issue details. GitHub might be temporarily unavailable.";
        }
        catch (System.Net.Http.HttpRequestException)
        {
          workRequestDetailsViewModel.GitHubViewModel.ErrorMessage = "Could not load GitHub issue details. GitHub might be temporarily unavailable.";
        }
        catch (Exception ex)
        {
          _logger.LogError(ex, $"An unexpected error occurred while retrieving GitHub issue for WorkRequestId: {workRequestId}");
          workRequestDetailsViewModel.GitHubViewModel.ErrorMessage = "An unexpected error occurred while loading GitHub issue details.";
        }

      }

      List<ConversationViewModel> combinedItems = new();

      combinedItems.AddRange(workRequestDetailsViewModel.WorkRequestEvents.Select(e => new ConversationViewModel
      {
        CreatedAt = e.CreatedAt,
        WorkRequestEvent = e
      }));


      foreach (SubTaskAccordionViewModel subTask in workRequestDetailsViewModel.SubTaskViewModel.SubTasks)
      {
        combinedItems.Add(new ConversationViewModel
        {
          CreatedAt = subTask.SubTask.CreationDateTime,
          SubTasks = new SubTaskAccordionViewModel
          {
            SubTask = subTask.SubTask,
            SubTaskEvents = subTask.SubTaskEvents,
            AssignableUsers = GetAssignableUsers()
          }
        });
      }

      workRequestDetailsViewModel.ConversationViewItems = combinedItems.OrderBy(i => i.CreatedAt).ToList();

      return workRequestDetailsViewModel;
    }

    public List<UserSimpleViewModel> GetAssignedUsers(WorkRequest workRequest)
    {
      List<ApplicationUser> users = workRequest.Assignees;
      List<UserSimpleViewModel> userSimpleViewModels = new();
      if (users == null || users.Count == 0)
        return userSimpleViewModels;

      foreach (ApplicationUser identityUser in users)
      {
        userSimpleViewModels.Add(new UserSimpleViewModel
        {
          UserId = identityUser.Id,
          Email = identityUser.Email,
          Username = identityUser.UserName,
        });
      }
      return userSimpleViewModels.OrderBy(o => o.Username).ToList();
    }

    // Get assignable users for sub tasks
    public List<UserSimpleViewModel> GetAssignableUsers()
    {
      List<ApplicationUser> users = _userManager.Users.ToList();

      IEnumerable<ApplicationUser> activeUsers = users.Where(q => ((q.LockoutEnabled && (q.LockoutEnd ?? DateTimeOffset.MinValue) < DateTimeOffset.Now) || !q.LockoutEnabled) && !q.isSystemAccount);

      List<UserSimpleViewModel> userSimpleViewModels = new();
      if (activeUsers != null)
        foreach (ApplicationUser identityUser in activeUsers)
        {
          userSimpleViewModels.Add(new UserSimpleViewModel
          {
            UserId = identityUser.Id,
            Email = identityUser.Email,
            Username = identityUser.UserName
          });
        }
      return userSimpleViewModels.OrderBy(o => o.Email).ToList();
    }

    // Get subscribable users for a work request
    public async Task<List<UserSimpleViewModel>> GetSubscribableUsers(int? workRequestId)
    {
      if (workRequestId is null) return null;

      List<ApplicationUser> users = _userManager.Users.ToList();

      List<ApplicationUser> activeUsers = users.Where(q => ((q.LockoutEnabled && (q.LockoutEnd ?? DateTimeOffset.MinValue) < DateTimeOffset.Now) || !q.LockoutEnabled) && !q.isSystemAccount).ToList();

      List<WorkRequestSubscription> subscribedUsers = await _workRequestSubscriptionRepository.GetWorkRequestActiveSubscribers((int)workRequestId);

      List<UserSimpleViewModel> userSimpleViewModels = new();

      if (activeUsers != null)
        foreach (ApplicationUser identityUser in activeUsers)
        {
          bool alreadySubscribed = subscribedUsers.Any(s =>
            s.ApplicationUser.Id == identityUser.Id);

          if (alreadySubscribed)
            continue;

          userSimpleViewModels.Add(new UserSimpleViewModel
          {
            UserId = identityUser.Id,
            Email = identityUser.Email,
            Username = identityUser.UserName
          });
        }
      return userSimpleViewModels.OrderBy(o => o.Email).ToList();
    }

    /// <summary>
    /// Gets users with 'Authoriser' role.
    /// </summary>
    /// <param name="isActiveOnly">If true, returns only active users. False, returns all users.</param>
    /// <returns></returns>
    public List<UserSimpleViewModel> GetAuthorisers(bool isActiveOnly = true)
    {
      IEnumerable<ApplicationUser> authoriserIdentityUsers = _userManager.GetUsersInRoleAsync("Authoriser").Result;

      if (isActiveOnly)
      {
        // .Where( (lockout enabled AND lockout expired) OR Lockout disabled )
        authoriserIdentityUsers = authoriserIdentityUsers.Where(q => ((q.LockoutEnabled && (q.LockoutEnd ?? DateTimeOffset.MinValue) < DateTimeOffset.Now) || !q.LockoutEnabled) && !q.isSystemAccount);
      }

      List<ApplicationUser> identityUsers = new List<ApplicationUser>();
      if (authoriserIdentityUsers != null)
        foreach (ApplicationUser identityUser in authoriserIdentityUsers)
        {
          if (identityUsers.Any(x => x.Id == identityUser.Id))
            continue; // User has multiple roles, skip double addition.

          identityUsers.Add(identityUser);
        }


      List<UserSimpleViewModel> userSimpleViewModels = new();
      if (identityUsers != null)
        foreach (ApplicationUser identityUser in identityUsers)
        {
          userSimpleViewModels.Add(new UserSimpleViewModel
          {
            UserId = identityUser.Id,
            Email = identityUser.Email,
            Username = identityUser.UserName,
          });
        }
      return userSimpleViewModels.OrderBy(o => o.Username).ToList();
    }

    /// <summary>
    /// If the logged in user keeps approving different stages of the Work Request process this could be problematic as who is watching the watchmen.
    /// This method provides a warning message that the user has already been involved in two stages of the process.
    /// It then asks them to justify why they're doing multiple stages of the process.
    /// 20211015 jsk505
    /// </summary>
    /// <param name="workRequestDetailsViewModel">Work Request Details page's View Model</param>
    /// <param name="userEmail">The logged in user's email.</param>
    private static void CheckUserMultipleAuthorisations(WorkRequestDetailsViewModel workRequestDetailsViewModel, string userEmail)
    {
      // User can't be involved in two stages if there has only be one stage so far
      if (workRequestDetailsViewModel.Status <= (int)WorkRequestStatusEnum.PendingInitialApproval)
        return;

      int eventCount = 0;
      int keyStepsCount = 0;

      if (workRequestDetailsViewModel.CreatedBy.ToUpperInvariant() == userEmail.ToUpperInvariant())
      {
        eventCount += 1;
        keyStepsCount += 1;
      }

      eventCount += workRequestDetailsViewModel.WorkRequestEvents
                                          .Count(x => x.CreatedByEmail.ToUpperInvariant() == userEmail.ToUpperInvariant());

      keyStepsCount += workRequestDetailsViewModel.WorkRequestEvents
                                          .Where(x => x.CreatedByEmail.ToUpperInvariant() == userEmail.ToUpperInvariant())
                                          .Count(x =>
                                          {
                                            return new List<WorkRequestEventTypeEnum>
                                              {
                                                        WorkRequestEventTypeEnum.Approve,
                                                        WorkRequestEventTypeEnum.Complete,
                                                        WorkRequestEventTypeEnum.Closed
                                              }.Contains(x.WorkRequestEventType);
                                          });

      switch ((WorkRequestStatusEnum)workRequestDetailsViewModel.Status)
      {
        case WorkRequestStatusEnum.Completed:
          break;
        case WorkRequestStatusEnum.PendingCompletion:
          if (keyStepsCount >= 2)
          {
            workRequestDetailsViewModel.MultipleAuthorisationsWarning = $"Note: You have already been involved in this request {eventCount} times, and key steps {keyStepsCount} times.";

            // If no process deviation reason exists create one. Check prevents editing or viewing wiping existing reason left.
            if (workRequestDetailsViewModel.ProcessDeviationReason == null)
              workRequestDetailsViewModel.ProcessDeviationReason = new();
          }
          break;
        default:
          eventCount = 0;
          break;
      }
    }

    public EditWorkRequestViewModel GetEditWorkRequestViewModelFromWorkRequest(WorkRequest workRequest)
    {
      if (workRequest == null)
        return null;

      return new EditWorkRequestViewModel(workRequest);
    }

    public bool UpdateWorkRequest(WorkRequest updateWorkRequest)
    {
      // Validation
      if (updateWorkRequest == null || updateWorkRequest.WorkRequestId == default)
      {
        _logger.LogWarning("User attempted to update a non-existent record.");
        return false;
      }

      _workRequestRepository.Update(updateWorkRequest);

      try
      {
        _workRequestRepository.Save();
      }
      catch (DbUpdateException ex)
      {
        _logger.LogError("UpdateWorkRequest: Error updating database!", ex);
        return false;
      }
      catch (Exception ex)
      {
        _logger.LogError("UpdateWorkRequest: Unexpected error occurred!", ex);
        return false;
      }

      return true;
    }

    public IEnumerable<WorkRequestEventTypeEnum> GetAllowedWorkrequestEventTypes(WorkRequestStatusEnum workRequestStatusEnum)
    {
      IEnumerable<WorkRequestEventTypeEnum> allowedEventTypes;
      switch (workRequestStatusEnum)
      {
        case WorkRequestStatusEnum.PendingInitialApproval:
        case WorkRequestStatusEnum.PendingRequester:
          allowedEventTypes = new List<WorkRequestEventTypeEnum> { WorkRequestEventTypeEnum.Approve, WorkRequestEventTypeEnum.Enquiry, WorkRequestEventTypeEnum.Closed, WorkRequestEventTypeEnum.None };
          break;
        case WorkRequestStatusEnum.PendingCompletion:
          allowedEventTypes = new List<WorkRequestEventTypeEnum> { WorkRequestEventTypeEnum.Complete, WorkRequestEventTypeEnum.Closed, WorkRequestEventTypeEnum.None };
          break;
        case WorkRequestStatusEnum.Completed:
        case WorkRequestStatusEnum.Abandoned:
        default:
          allowedEventTypes = new List<WorkRequestEventTypeEnum>() { WorkRequestEventTypeEnum.None }; // TODO: Is this right?
          break;
      }
      return allowedEventTypes;
    }

    public async Task<(bool Success, string ErrorMessage)> AssignUserToWorkRequestAsync(int workRequestId, string assigneeEmail, string executorEmail, string linkUrl, bool isAutoAssign)
    {
      // Perform initial dumb checks.
      if (workRequestId <= 0 || string.IsNullOrWhiteSpace(assigneeEmail))
        throw new ArgumentException("Invalid request details.");

      WorkRequest? workRequest = _workRequestRepository.GetWorkRequest(workRequestId);
      if (workRequest == null) return (false, "Invalid workRequestId");

      ApplicationUser? assignee = await _userManager.FindByEmailAsync(assigneeEmail);
      if (assignee == null) return (false, "Invalid assigneeEmail");

      // Assignee is locked out, don't add to request and return error.
      if (assignee.LockoutEnabled && (assignee.LockoutEnd ?? DateTimeOffset.MinValue) > DateTimeOffset.Now)
        return (false, "Invalid assignee.");

      // Assignee already exists on requests
      if (workRequest.Assignees != null && workRequest.Assignees.Any(x => x.Id == assignee.Id))
        return (false, "User is already assigned");

      // Select assignee and save to request.
      workRequest.Assignees ??= new List<ApplicationUser>();
      workRequest.Assignees.Add(assignee);
      _workRequestRepository.Save();

      // Send notifications and subscribe
      await _notificationService.CreateAssignedToRequestNotifications(
          workRequest.WorkRequestId,
          assignee.Email,
          isAutoAssign,
          true,
          linkUrl
      );

      ApplicationUser systemUser = await _userManager.FindByEmailAsync("system@york.ac.uk");
      ApplicationUser? executor = !isAutoAssign ? await _userManager.FindByEmailAsync(executorEmail) : systemUser;
      if (executor == null) return (false, "Executor not found.");

      //Create assignment workRequest event for the action

      WorkRequestEvent workRequestEvent = new WorkRequestEvent
      {
        WorkRequest = workRequest,
        Content = $"{CommonHelpers.RemoveDomainFromEmail(executor.Email)} - assigned - {CommonHelpers.RemoveDomainFromEmail(assignee.Email)} at {DateTime.Now:HH:mm}",
        CreatedAt = DateTime.Now,
        CreatedBy = executor,
        EventType = _workRequestRepository.GetWorkRequestEventType(WorkRequestEventTypeEnum.Assignment)
      };

      _workRequestRepository.InsertWorkRequestEvent(workRequestEvent);
      _workRequestRepository.Save();

      // manual assignment triggers automatic subscription
      await _workRequestSubscriptionRepository.CreateWorkRequestSubscription(systemUser.Id, assignee.Id, workRequestId);

      //Auto Assign Label if Required
      string? autoLabelName = AutoAssigneeLabels.GetValueOrDefault(assigneeEmail);
      if (autoLabelName != null)
      {
        Label? autoLabel = _labelRepository.GetUnarchivedLabels()
                                .FirstOrDefault(l => l.LabelShort.Equals(autoLabelName, StringComparison.OrdinalIgnoreCase));

        await AddLabelToWorkRequestAsync(workRequestId, autoLabel.LabelId, systemUser.Email);
      }


      return (true, null);
    }

    public async Task<(bool Success, string ErrorMessage)> UnassignUserToWorkRequestAsync(int workRequestId, string assigneeEmail, string executorEmail, bool isAutoUnassign)
    {
      // Perform initial dumb checks.
      if (workRequestId <= 0 || string.IsNullOrWhiteSpace(assigneeEmail))
        throw new ArgumentException("Invalid request details.");

      WorkRequest? workRequest = _workRequestRepository.GetWorkRequest(workRequestId);
      if (workRequest == null) return (false, "Invalid workRequestId");

      ApplicationUser? assignee = await _userManager.FindByEmailAsync(assigneeEmail);
      if (assignee == null) return (false, "Invalid assigneeEmail");

      // Assignee already doesn't exist on requests.      
      if (workRequest.Assignees == null || !workRequest.Assignees.Any(x => x.Id == assignee.Id))
        return (false, "User is not assigned");


      // Remove assignee and save to request.
      workRequest.Assignees.RemoveAll(q => q.Id == assignee.Id);
      _workRequestRepository.Save();

      ApplicationUser systemUser = await _userManager.FindByEmailAsync("system@york.ac.uk");
      ApplicationUser? executor = !isAutoUnassign ? await _userManager.FindByEmailAsync(executorEmail) : systemUser;
      if (executor == null) return (false, "Executor not found.");

      //Create assignment workRequest event for the action
      WorkRequestEvent workRequestEvent = new()
      {
        WorkRequest = workRequest,
        Content = $"{CommonHelpers.RemoveDomainFromEmail(executor.Email)} - unassigned - {CommonHelpers.RemoveDomainFromEmail(assignee.Email)} at {DateTime.Now:HH:mm}",
        CreatedAt = DateTime.Now,
        CreatedBy = executor,
        EventType = _workRequestRepository.GetWorkRequestEventType(WorkRequestEventTypeEnum.Assignment)
      };
      _workRequestRepository.InsertWorkRequestEvent(workRequestEvent);
      _workRequestRepository.Save();


      // manual unassignment triggers automatic unsubscription
      await _workRequestSubscriptionService.UnsubscribeUserFromWorkRequest(systemUser.Id, assignee.Id, workRequestId);

      return (true, null);
    }

    public async Task<(bool Success, string ErrorMessage)> AddLabelToWorkRequestAsync(int workRequestId, int labelId, string executorEmail)
    {
      // Perform initial dumb checks.
      if (workRequestId <= 0 || labelId <= 0)
        throw new ArgumentException("Invalid request details.");


      // Get work request and labels and check they are allowed.
      WorkRequest? workRequest = _workRequestRepository.GetWorkRequest(workRequestId);
      if (workRequest == null) return (false, "Invalid workRequestId");

      IEnumerable<Label> allLabels = _labelRepository.GetUnarchivedLabels();
      if (allLabels != null && !allLabels.Any(x => x.LabelId == labelId))
        return (false, "Invalid labelId");


      // Label already exists on requests.      
      if (workRequest.Labels != null && workRequest.Labels.Any(x => x.LabelId == labelId))
        return (true, null);


      // Select label and save to request.
      Label label = allLabels.Single(q => q.LabelId == labelId);
      if (workRequest.Labels == null)
        workRequest.Labels = new();

      workRequest.Labels.Add(label);
      _workRequestRepository.Save();

      //Create assignment workRequest event for the action
      ApplicationUser? executor = await _userManager.FindByEmailAsync(executorEmail);
      if (executor == null) return (false, "Executor not found.");

      WorkRequestEvent workRequestEvent = new WorkRequestEvent
      {
        WorkRequest = workRequest,
        Content = $"{CommonHelpers.RemoveDomainFromEmail(executor.Email)} - added label - {label.LabelShort} at {DateTime.Now:HH:mm}",
        CreatedAt = DateTime.Now,
        CreatedBy = executor,
        EventType = _workRequestRepository.GetWorkRequestEventType(WorkRequestEventTypeEnum.Label)
      };

      _workRequestRepository.InsertWorkRequestEvent(workRequestEvent);
      _workRequestRepository.Save();


      return (true, null);
    }

    public async Task<(bool Success, string ErrorMessage)> RemoveLabelToWorkRequestAsync(int workRequestId, int labelId, string executorEmail)
    {
      // Perform initial dumb checks.
      if (workRequestId <= 0 || labelId <= 0)
        throw new ArgumentException("Invalid request details.");


      // Get work request and labels and check they are allowed.
      WorkRequest? workRequest = _workRequestRepository.GetWorkRequest(workRequestId);
      if (workRequest == null) return (false, "Invalid workRequestId");

      IEnumerable<Label> allLabels = _labelRepository.GetUnarchivedLabels();
      if (allLabels != null && !allLabels.Any(x => x.LabelId == labelId))
        return (false, "Invalid labelId");


      // Label already exists on requests.      
      if (workRequest.Labels == null || !workRequest.Labels.Any(x => x.LabelId == labelId))
        return (true, null);


      // Remove label and save to request.
      workRequest.Labels.RemoveAll(q => q.LabelId == labelId);
      _workRequestRepository.Save();

      //Create assignment workRequest event for the action
      ApplicationUser? executor = await _userManager.FindByEmailAsync(executorEmail);
      if (executor == null) return (false, "Executor not found.");

      Label label = allLabels.Where(x => x.LabelId == labelId).FirstOrDefault();

      WorkRequestEvent workRequestEvent = new WorkRequestEvent
      {
        WorkRequest = workRequest,
        Content = $"{CommonHelpers.RemoveDomainFromEmail(executor.Email)} - removed label - {label.LabelShort} at {DateTime.Now:HH:mm}",
        CreatedAt = DateTime.Now,
        CreatedBy = executor,
        EventType = _workRequestRepository.GetWorkRequestEventType(WorkRequestEventTypeEnum.Label)
      };

      _workRequestRepository.InsertWorkRequestEvent(workRequestEvent);
      _workRequestRepository.Save();

      return (true, null);
    }

    private static readonly Dictionary<string, string> AutoAssigneeLabels = new(StringComparer.OrdinalIgnoreCase)
        {
            {"ytu-redcap-group@york.ac.uk", "redcap"},
            {"ytu-datavalidation-group@york.ac.uk", "validation"},
            {"ytu-developers-group@york.ac.uk", "dev"}
        };
  }
}
