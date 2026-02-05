using BusinessLayer.Helpers;
using BusinessLayer.Repositories.Interfaces;
using BusinessLayer.Services;
using BusinessLayer.Services.Interfaces;
using BusinessLayer.ViewModels;
using DataAccessLayer.Models;
using Enums;
using Enums.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
  [Authorize(Roles = "User, Authoriser")]
  public class WorkRequestController : Controller
  {
    private readonly ILogger<WorkRequestController> _logger;
    private readonly IWorkRequestRepository _workRequestRepository;
    private readonly IWorkRequestStatusRepository _workRequestStatusRepository;
    private readonly IFileRepository _fileRepository;
    private readonly ITrialRepository _trialRepository;
    private readonly ILabelRepository _labelRepository;
    private readonly IWorkRequestService _workRequestService;
    private readonly INotificationService _notificationService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly GitHubService _gitHubService;
    private readonly ITemplateRepository _templateRepository;
    private readonly WorkRequestSubscriptionService _workRequestSubscriptionService;
    private readonly IWorkRequestSubscriptionRepository _workRequestSubscriptionRepository;
    private readonly SubTaskService _subTaskService;
    private readonly ISubTaskStatusRepository _subTaskStatusRepository;
    private readonly ISubTaskRepository _subTaskRepository;
    private readonly IDataExportService _dataExportService;

    public WorkRequestController(
        ILogger<WorkRequestController> logger,
        IWorkRequestRepository workRequestRepository,
        IWorkRequestStatusRepository workRequestStatusRepository,
        IFileRepository fileRepository,
        ITrialRepository trialRepository,
        ILabelRepository labelRepository,
        IWorkRequestService workRequestService,
        INotificationService notificationService,
        UserManager<ApplicationUser> userManager,
        GitHubService gitHubService,
        ITemplateRepository templateRepository,
        WorkRequestSubscriptionService workRequestSubscriptionService,
        IWorkRequestSubscriptionRepository workRequestSubscriptionRepository,
        SubTaskService subTaskService,
        ISubTaskStatusRepository subTaskStatusRepository,
        ISubTaskRepository subTaskRepository,
        IDataExportService dataExportService)
    {
      _logger = logger;
      _workRequestRepository = workRequestRepository;
      _workRequestStatusRepository = workRequestStatusRepository;
      _fileRepository = fileRepository;
      _trialRepository = trialRepository;
      _labelRepository = labelRepository;
      _workRequestService = workRequestService;
      _notificationService = notificationService;
      _userManager = userManager;
      _gitHubService = gitHubService;
      _templateRepository = templateRepository;
      _workRequestSubscriptionService = workRequestSubscriptionService;
      _workRequestSubscriptionRepository = workRequestSubscriptionRepository;
      _subTaskService = subTaskService;
      _subTaskStatusRepository = subTaskStatusRepository;
      _subTaskRepository = subTaskRepository;
      _dataExportService = dataExportService;
    }

    #region Private Methods
    private string GetCurrentUserEmail()
    {
      return _userManager.GetEmailAsync(_userManager.GetUserAsync(User).Result).Result;
    }

    private async Task<string> GetCurrentUserId()
    {
      ApplicationUser user = await _userManager.GetUserAsync(User);
      return user is null ? string.Empty : user.Id;
    }

    private List<int> GetTrialsIdsToCreateFor(int trialId, List<CheckboxViewModel> adminCheckMultiTrials)
    {
      List<int> trialIds = new();
      if (User.IsInRole(IdentityRoleEnum.Authoriser.ToString()))
        trialIds = adminCheckMultiTrials.Where(x => x.IsChecked)
                                        .Select(x => x.Id).ToList();
      else
        trialIds.Add(trialId);

      return trialIds;
    }

    private string GetLinkUrl(int workRequestId)
    {
      string systemLinkUrl = Url.Action(nameof(WorkRequestController.WorkRequestDetails),
                                            ControllerHelpers.GetControllerName<WorkRequestController>(),
                                            new { workRequestId },
                                            protocol: Request.Scheme, host: Request.Host.Value);
      return systemLinkUrl;
    }

    #endregion

    #region Create Work Requets

    /// <summary>
    /// Pageload for Add New Request
    /// </summary>
    /// <returns>Initiated WorkRequestViewModel containing list of selectable Trials.</returns>
    [HttpGet]
    public IActionResult CreateWorkRequest()
    {
      // Create view model
      WorkRequestViewModel workRequestViewModel = new() { Trials = new List<Trial>() };

      // Get data from database using EF Core for dropdowns
      IEnumerable<Trial> trialNameList = _trialRepository.GetActiveTrials().OrderBy(x => x.TrialId == 1000).ThenBy(x => x.TrialName); // OrderBy TrialId == 1000 to make sure "other" at bottom always.

      if (trialNameList == null || !trialNameList.Any(x => x.IsActive))
      {
        _logger.LogError($"{nameof(CreateWorkRequest)}: No trials found.");
        TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "No trials found."));
        return View(workRequestViewModel);
      }

      // Assign trials list to view model
      workRequestViewModel.Trials = trialNameList;

      return View(workRequestViewModel);
    }

    /// <summary>
    /// Post for Add New Request
    /// </summary>
    /// <param name="createWorkRequestViewModel">Data from the New Request form</param>
    /// <param name="SupportingFiles">Uploaded files</param>
    /// <returns>Redirect to new request details and TempData["PopupMessage"].</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateWorkRequest(WorkRequestViewModel createWorkRequestViewModel, IEnumerable<IFormFile> SupportingFiles, List<CheckboxViewModel> adminCheckMultiTrials)
    {
      //added custom validation as client side has been switched off to allow for the two types of way of selecting trials (ddl & checkbox)
      if (User.IsInRole(IdentityRoleEnum.Authoriser.ToString()))
      {
        if (!createWorkRequestViewModel.SelectedTrialIds.Any())
        {
          ModelState.AddModelError("Trial", "You must select at least one system.");
        }
      }
      else if (createWorkRequestViewModel.Trial == 0)
      {
        ModelState.AddModelError("Trial", "Trial/system name is required.");
      }

      if (!ModelState.IsValid)
      {
        _logger.LogError($"{nameof(CreateWorkRequest)}: Invalid model state.");

        IEnumerable<Trial> trialNameList = _trialRepository.GetActiveTrials().OrderBy(x => x.TrialId == 1000).ThenBy(x => x.TrialName); // OrderBy TrialId == 1000 to make sure "other" at bottom always.
        if (trialNameList == null || !trialNameList.Any(x => x.IsActive))
        {
          _logger.LogError($"{nameof(CreateWorkRequest)}: No trials found.");
          TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "No trials found."));
          return View(createWorkRequestViewModel);
        }

        // Assign trials list to view model
        createWorkRequestViewModel.Trials = trialNameList;

        return View(createWorkRequestViewModel);
      }

      List<int> trialsToCreateFor = createWorkRequestViewModel.SelectedTrialIds.Any()
          ? createWorkRequestViewModel.SelectedTrialIds
          : [createWorkRequestViewModel.Trial];

      WorkRequest workRequest = new();
      string currentUserId = await GetCurrentUserId();
      string currentUserEmail = GetCurrentUserEmail();

      for (int i = 0; i < trialsToCreateFor.Count; i++)
      {
        // Set field values within work request to values in Work Request View Model
        workRequest = new();
        WorkRequestStatus status = _workRequestStatusRepository.GetByID(WorkRequestStatusEnum.PendingInitialApproval);
        if (status == null)
        {
          _logger.LogError($"{nameof(CreateWorkRequest)}: Unable to get status. StatusId: {(int)WorkRequestStatusEnum.PendingInitialApproval}");
          TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "Invalid work request status."));
          return View();
        }
        workRequest.Status = status;
        Trial trial = _trialRepository.GetByID(trialsToCreateFor[i]);
        if (status == null)
        {
          _logger.LogError($"{nameof(CreateWorkRequest)}: Unable to get trial. TrialId: {trialsToCreateFor[i]}");
          TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "Trial not found."));
          return View();
        }
        workRequest.Trial = trial;
        workRequest.TrialOther = createWorkRequestViewModel.TrialOther;
        workRequest.CreationDateTime = DateTime.Now;
        workRequest.Reference = createWorkRequestViewModel.Reference;
        workRequest.DetailDescription = createWorkRequestViewModel.DetailDescription;
        workRequest.CreatedBy = currentUserEmail;
        workRequest.LastEditedBy = workRequest.CreatedBy;

        _workRequestRepository.Insert(workRequest);
        _workRequestRepository.Save();

        // Upload Supporting Files Method
        if (SupportingFiles != null && SupportingFiles.Any())
        {
          List<FileUpload> fileUploads = FileHelpers.PopulateFileUpload(workRequest.WorkRequestId, SupportingFiles, workRequest.CreatedBy);
          foreach (FileUpload fileUpload in fileUploads)
          {
            if (workRequest.SupportingFiles == null || !workRequest.SupportingFiles.Any())
              workRequest.SupportingFiles = new List<FileUpload>();

            _fileRepository.Insert(fileUpload);
            _fileRepository.Save();
          }
        }
        // Save the FileUploads to the work request.
        _workRequestRepository.Save();

        // Sent only to the requester.
        _ = await _notificationService.CreateInitialSubmissionNotificationsAsync(workRequest.WorkRequestId, GetLinkUrl(workRequest.WorkRequestId));

        // Subscribe user creating the request
        ApplicationUser systemUser = await _userManager.FindByEmailAsync("system@york.ac.uk");
        await _workRequestSubscriptionRepository.CreateWorkRequestSubscription(systemUser.Id, currentUserId, workRequest.WorkRequestId);

        // Auto triage if TrialOwner exists
        if (workRequest.Trial.TrialOwner != null)
        {
          ApplicationUser executor = await _userManager.FindByNameAsync(User.Identity?.Name);
          TrialAttribution? trialOwner = workRequest.Trial.TrialOwner;
          string? trialOwnerEmail = EnumHelpers.GetEmail(trialOwner);

          await _workRequestService.AssignUserToWorkRequestAsync(workRequest.WorkRequestId, trialOwnerEmail, executor.Email, GetLinkUrl(workRequest.WorkRequestId), true);
        }
      }

      TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: true, message: "Work request submitted for approval."));

      if (trialsToCreateFor.Count > 1)
      {
        return RedirectToAction("Index", "ViewAllWorkRequests");
      }

      return RedirectToAction("WorkRequestDetails", "WorkRequest", new { workRequestId = workRequest.WorkRequestId });
    }

    #endregion

    #region Work Request Details

    /// <summary>
    /// Pageload for Work Request Details.
    /// Gets the Details and Events for a Work Request. Pre-populates AllowedEventTypes.
    /// </summary>
    /// <param name="workRequestId"></param>
    /// <returns>WorkRequestDetailsViewModel populated with Work Request with <paramref name="workRequestId"/></returns>
    [HttpGet]
    public async Task<IActionResult> WorkRequestDetails(int workRequestId)
    {
      WorkRequestDetailsViewModel workRequest = await _workRequestService.GetWorkRequestById(workRequestId, GetCurrentUserEmail());
      if (workRequest == null)
      {
        string message = $"{nameof(WorkRequestDetails)}: Work Request not found/does not exist for Work Request ID '{workRequestId}'.";
        TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "Work request could not be found."));
        _logger.LogInformation(message);
        return RedirectToAction("Index", "ViewAllWorkRequests");
      }

      ApplicationUser userViewingRequest = await _userManager.FindByNameAsync(User.Identity.Name);

      bool isExported = false;

      if (workRequest.WorkRequestEvents.Any(x => x.WorkRequestEventType == WorkRequestEventTypeEnum.Export))
      {
        isExported = true;
      }

      ViewBag.IsExported = isExported;

      bool isLoggedInUserUser = User.IsInRole("User");
      workRequest.IsLoggedInUserOfRoleTypeUser = isLoggedInUserUser;
      workRequest.LabelsEditorViewModel.IsLoggedInUserOfRoleTypeUser = isLoggedInUserUser;
      workRequest.AssigneesViewModel.IsLoggedInUserOfRoleTypeUser = isLoggedInUserUser;
      foreach (ConversationViewModel conversation in workRequest.ConversationViewItems)
      {
        if (conversation.SubTasks != null)
        {
          conversation.SubTasks.IsLoggedInUserOfRoleTypeUser = isLoggedInUserUser;
        }
      }

      workRequest.IsLoggedInUserRequester = workRequest.CreatedBy.ToUpper() == userViewingRequest.NormalizedEmail;
      workRequest.AllowedEventTypes = _workRequestService.GetAllowedWorkrequestEventTypes((WorkRequestStatusEnum)workRequest.Status);
      workRequest.ViewerIsSubscribed = await _workRequestSubscriptionRepository.GetUserIsSubscribedToWorkRequest(userViewingRequest.Id, workRequestId);
      workRequest.TrialEmailIsSubscribed = workRequest.TrialEmail != null ? await _workRequestSubscriptionRepository.GetUserIsSubscribedToWorkRequest(workRequest.TrialEmail.Id, workRequestId) : false;
      workRequest.Trials = _trialRepository.GetActiveTrials().OrderBy(x => x.TrialId == 1000).ThenBy(x => x.TrialName); // OrderBy TrialId == 1000 to make sure "other" at bottom always.

      return View(workRequest);
    }

    /// <summary>
    /// Post for adding new event to an existing Work Request.
    /// This will trigger the status changes associated with the event type selected.
    /// </summary>
    /// <param name="workRequestDetailsViewModel"></param>
    /// <returns>Redirect to newly updated request. (Refresh)</returns>
    [HttpPost]
    public async Task<IActionResult> CreateWorkRequestEvent(WorkRequestDetailsViewModel workRequestDetailsViewModel)
    {
      WorkRequest workRequest = _workRequestRepository.GetWorkRequest(workRequestDetailsViewModel.WorkRequestId);
      if (workRequest == null)
      {
        string message = $"{nameof(CreateWorkRequestEvent)}: Work Request not found/does not exist for Work Request ID '{workRequestDetailsViewModel.WorkRequestId}'.";
        TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "Work request could not be found."));
        _logger.LogError(message);
        if (workRequestDetailsViewModel.WorkRequestId > 0)
        {
          return RedirectToAction("WorkRequestDetails", "WorkRequest", new { workRequestId = workRequestDetailsViewModel.WorkRequestId });
        }
        else
        {
          return RedirectToAction("Index", "ViewAllWorkRequests");
        }
      }

      // Check user permissions sufficient for event type
      ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
      IEnumerable<string> userRoles = await _userManager.GetRolesAsync(user);
      IEnumerable<string> elevatedRoles = new List<string>() { "Authoriser" };
      if (!userRoles.Any(x => elevatedRoles.Contains(x)))
      {
        // User has no elevated permissions beyond "User".

        IEnumerable<WorkRequestEventTypeEnum> alwaysAllowedEventTypes = new List<WorkRequestEventTypeEnum>()
                {
                    WorkRequestEventTypeEnum.None
                };

        // If the event type is not on the alwaysAllowedEventTypes list, disallow it.
        if (!alwaysAllowedEventTypes.Any(x => x == workRequestDetailsViewModel.NewWorkRequestEvent.EventType.WorkRequestEventTypeId))
        {
          string message = $"{nameof(CreateWorkRequestEvent)}: Work Request Event could not be added due to permissions levels for Work Request ID '{workRequestDetailsViewModel.WorkRequestId}' and user '{GetCurrentUserEmail()}'.";
          TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "You do not have the permissions to add that type of event."));
          _logger.LogError(message);
          return RedirectToAction("WorkRequestDetails", "WorkRequest", new { workRequestId = workRequest.WorkRequestId });
        }
      }

      // Check status of work request is compatible with the new event being attempted.
      IEnumerable<WorkRequestEventTypeEnum> allowedEventTypes = _workRequestService.GetAllowedWorkrequestEventTypes(workRequest.Status.WorkRequestStatusId);

      // If not in allowed event types list, disallow.
      if (!allowedEventTypes.Any(x => x == workRequestDetailsViewModel.NewWorkRequestEvent.EventType.WorkRequestEventTypeId))
      {
        string message = $"{nameof(CreateWorkRequestEvent)}: Work Request Event could not be added due to status for Work Request ID '{workRequestDetailsViewModel.WorkRequestId}' and status '{workRequest.Status.WorkRequestStatusName}'.";
        TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: $"You cannot add that type of event to a work request with Status: '{workRequest.Status.WorkRequestStatusName}'."));
        _logger.LogError(message);
        return RedirectToAction("WorkRequestDetails", "WorkRequest", new { workRequestId = workRequest.WorkRequestId });
      }

      // Empty descriptions are disallowed.
      if (string.IsNullOrWhiteSpace(workRequestDetailsViewModel.NewWorkRequestEvent.Content))
      {
        string message = $"{nameof(CreateWorkRequestEvent)}: Work Request Event could not be added due to an empty description for Work Request ID '{workRequestDetailsViewModel.WorkRequestId}' and status '{workRequest.Status.WorkRequestStatusName}'.";
        TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: $"You cannot submit a blank message."));
        _logger.LogError(message);
        return RedirectToAction("WorkRequestDetails", "WorkRequest", new { workRequestId = workRequest.WorkRequestId });
      }

      // Cannot close work request if there are outstanding sub tasks
      IEnumerable<SubTask> subTasks = _subTaskRepository.GetAllSubTasksForWorkRequest(workRequestDetailsViewModel.WorkRequestId);

      if ((workRequestDetailsViewModel.NewWorkRequestEvent.EventType.WorkRequestEventTypeId == WorkRequestEventTypeEnum.Complete
          || workRequestDetailsViewModel.NewWorkRequestEvent.EventType.WorkRequestEventTypeId == WorkRequestEventTypeEnum.Closed)
          && subTasks.Any(st => st.Status.SubTaskStatusId == SubTaskStatusEnum.Open))
      {
        string message = $"{nameof(CreateWorkRequestEvent)}: Work Request Event could not be added due to an outstanding sub task for Work Request ID '{workRequestDetailsViewModel.WorkRequestId}' and status '{workRequest.Status.WorkRequestStatusName}'.";
        TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: $"You cannot close a work request with outstanding sub tasks."));
        _logger.LogInformation(message);
        return RedirectToAction("WorkRequestDetails", "WorkRequest", new { workRequestId = workRequest.WorkRequestId });
      }

      // Upload Supporting Files Method
      List<IFormFile> newFiles = workRequestDetailsViewModel.NewSupportingFiles;
      if (newFiles != null && newFiles.Any())
      {
        IEnumerable<FileUpload> existingFiles = _fileRepository.GetFileUploadsByWorkRequestId(workRequest.WorkRequestId); // Used to check for duplicate uploads and duplicate names.
        List<FileUpload> fileUploads = FileHelpers.PopulateFileUpload(workRequest.WorkRequestId, newFiles, workRequest.LastEditedBy, out IEnumerable<string> failedUploads, existingFiles);
        if (failedUploads != null && failedUploads.Any())
        {
          _logger.LogWarning($"User attempted to upload a duplicate file(s) for workRequestId {workRequest.WorkRequestId} called {string.Join(',', failedUploads)}.");
          TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, $"Changes were saved but duplicate files were not uploaded. Files not uploaded: {string.Join(',', failedUploads)}."));
        }

        foreach (FileUpload fileUpload in fileUploads)
        {
          if (workRequest.SupportingFiles == null || !workRequest.SupportingFiles.Any())
            workRequest.SupportingFiles = new List<FileUpload>();

          _fileRepository.Insert(fileUpload);
          _fileRepository.Save();

          WorkRequestEvent workRequestEventFileUpload = new()
          {
            WorkRequest = workRequest,
            Content = $"{CommonHelpers.RemoveDomainFromEmail(user.Email)} - added file - {fileUpload.ReadableFileName} at {DateTime.Now:HH:mm}",
            CreatedAt = DateTime.Now,
            CreatedBy = user,
            EventType = _workRequestRepository.GetWorkRequestEventType(WorkRequestEventTypeEnum.FileManagement)
          };

          _workRequestRepository.InsertWorkRequestEvent(workRequestEventFileUpload);
          _workRequestRepository.Save();
        }
      }
      // Save the FileUploads to the work request.          
      _workRequestRepository.Save();


      // Insert new record and redirect
      WorkRequestEvent workRequestEvent = new()
      {
        WorkRequest = workRequest,
        Content = workRequestDetailsViewModel.NewWorkRequestEvent.Content,
        DurationDays = workRequestDetailsViewModel.NewWorkRequestEvent.DurationDays,
        CreatedAt = DateTime.Now,
        CreatedBy = user,
        EventType = _workRequestRepository.GetWorkRequestEventType(workRequestDetailsViewModel.NewWorkRequestEvent.EventType.WorkRequestEventTypeId)
      };
      if (workRequestDetailsViewModel.NewWorkRequestEvent.EventType.WorkRequestEventTypeId == WorkRequestEventTypeEnum.Complete
          && workRequestDetailsViewModel.ProcessDeviationReason != null)
      {
        _workRequestRepository.InsertProcessDeviationReason(workRequestDetailsViewModel.ProcessDeviationReason, workRequest);
      }

      _workRequestRepository.InsertWorkRequestEvent(workRequestEvent);

      workRequest.LastEditedDateTime = DateTime.Now;
      workRequest.LastEditedBy = user.Email;
      // Change status of request and send notifications where appropriate
      // Sent only to the requester.
      switch (workRequestDetailsViewModel.NewWorkRequestEvent.EventType.WorkRequestEventTypeId)
      {
        case (WorkRequestEventTypeEnum.Approve):
          workRequest.Status = _workRequestStatusRepository.Get(q => q.WorkRequestStatusId == WorkRequestStatusEnum.PendingCompletion).Single();
          _ = await _notificationService.CreateInitialApprovalNotificationsAsync(workRequest.WorkRequestId, GetLinkUrl(workRequest.WorkRequestId), workRequestDetailsViewModel.NewWorkRequestEvent.Content);
          await _gitHubService.RemoveLabelFromIssue(workRequest.AssignedTrialRepositoryId, workRequest.GitHubIssueNumber, EnumHelpers.GetDisplayName(GitHubLabelEnum.ApprovalNeeded));
          break;
        case (WorkRequestEventTypeEnum.Enquiry):
          workRequest.Status = _workRequestStatusRepository.Get(q => q.WorkRequestStatusId == WorkRequestStatusEnum.PendingRequester).Single();
          _ = await _notificationService.CreateRejectedWithAmendmentsNotificationsAsync(workRequest.WorkRequestId, GetLinkUrl(workRequest.WorkRequestId), workRequestDetailsViewModel.NewWorkRequestEvent.Content);
          break;
        case (WorkRequestEventTypeEnum.Complete):
          workRequest.Status = _workRequestStatusRepository.Get(q => q.WorkRequestStatusId == WorkRequestStatusEnum.Completed).Single();
          _ = await _notificationService.CreateCompletedNotificationsAsync(workRequest.WorkRequestId, GetLinkUrl(workRequest.WorkRequestId), workRequestDetailsViewModel.NewWorkRequestEvent.Content);
          break;
        case (WorkRequestEventTypeEnum.Closed):
          workRequest.Status = _workRequestStatusRepository.Get(q => q.WorkRequestStatusId == WorkRequestStatusEnum.Abandoned).Single();
          _ = await _notificationService.CreateAbandonedNotificationsAsync(workRequest.WorkRequestId, GetLinkUrl(workRequest.WorkRequestId), workRequestDetailsViewModel.NewWorkRequestEvent.Content);
          break;
        case (WorkRequestEventTypeEnum.None):
          if (workRequest.Status.WorkRequestStatusId == WorkRequestStatusEnum.PendingRequester
              && workRequest.CreatedBy.ToUpperInvariant() == GetCurrentUserEmail().ToUpperInvariant())
          {
            workRequest.Status = _workRequestStatusRepository.Get(q => q.WorkRequestStatusId == WorkRequestStatusEnum.PendingInitialApproval).Single();
          }
          _ = await _notificationService.CreateMessageNotifications(workRequest.WorkRequestId, GetLinkUrl(workRequest.WorkRequestId), workRequestEvent);
          break;
        default:
          _logger.LogError("Invalid Work Request Event Type! Type: '" + workRequestDetailsViewModel.NewWorkRequestEvent.EventType.WorkRequestEventTypeId + "'.");
          break;
      }
      _workRequestRepository.Update(workRequest);
      _workRequestRepository.Save();

      // If WorkRequestEventType = Approve add a comment to the GitHub issue with the details
      if (workRequestDetailsViewModel.NewWorkRequestEvent.EventType.WorkRequestEventTypeId == WorkRequestEventTypeEnum.Approve && !string.IsNullOrEmpty(workRequestDetailsViewModel.NewWorkRequestEvent.Content))
      {
        await _gitHubService.UpdateGitHubIssueWithAnalysis(workRequest);
      }
      return RedirectToAction("WorkRequestDetails", "WorkRequest", new { workRequestId = workRequest.WorkRequestId });
    }

    #endregion

    #region Update Work Requests

    /// <summary>
    /// Saves a specified project/trial Id against a work request.
    /// </summary>
    /// <param name="workRequestId"></param>
    /// <param name="projectId"></param>
    /// <returns></returns>
    [HttpPut]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Authoriser")]
    public async Task<IActionResult> PutProjectIdOnWorkRequest(int workRequestId, int projectId)
    {
      WorkRequest workRequest = _workRequestRepository.GetWorkRequest(workRequestId);
      List<Trial> trials = _trialRepository.GetActiveTrials();
      if (workRequest == null)
      {
        string message = $"{nameof(PutProjectIdOnWorkRequest)}: Work Request not found/does not exist for Work Request ID '{workRequestId}'.";
        TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "Work request could not be found."));
        _logger.LogError(message);
        return BadRequest(message);
      }
      else if (!trials.Exists(q => q.TrialId == projectId))
      {
        string message = $"{nameof(PutProjectIdOnWorkRequest)}: Project/Trial not found/does not exist for Project/Trial ID '{projectId}'.";
        TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "Project could not be found."));
        _logger.LogError(message);
        return BadRequest(message);
      }

      string newProjectName = trials.FirstOrDefault(c => c.TrialId == projectId).TrialName;

      ApplicationUser executor = _userManager.FindByNameAsync(User.Identity.Name).Result;

      //Create change of project workRequest event for the action
      WorkRequestEvent workRequestEvent = new()
      {
        WorkRequest = workRequest,
        Content = $"{CommonHelpers.RemoveDomainFromEmail(executor.Email)} - changed project - from {workRequest.Trial.TrialName} to {newProjectName} at {DateTime.Now:HH:mm}",
        CreatedAt = DateTime.Now,
        CreatedBy = executor,
        EventType = _workRequestRepository.GetWorkRequestEventType(WorkRequestEventTypeEnum.Assignment)
      };
      _workRequestRepository.InsertWorkRequestEvent(workRequestEvent);

      workRequest.Trial = trials.Single(q => q.TrialId == projectId);

      _workRequestRepository.Save();


      TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: true, message: "Work request project updated"));
      return Ok();
    }

    #endregion

    #region File Uploads

    /// <summary>
    /// Get a list of supporting files by workRequestId.
    /// This is called in the FileUploadFeature.js file for the datatable.
    /// </summary>
    /// <param name="workRequestId">Id of the work request to get files for</param>
    /// <returns>Returns jsonstring in success. Returns BadRequest on error.</returns>
    [HttpGet]
    public IActionResult FileUploads(int? workRequestId)
    {
      if (workRequestId == null || workRequestId < 0)
      {
        string message = workRequestId is null ? "Work Request Id is null" : $"Work Request Id is not valid: {workRequestId}";
        _logger.LogError(message);
        TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "Work request for file not found."));
        return BadRequest();
      }

        WorkRequest workRequest = _workRequestRepository.GetWorkRequest((int)workRequestId);
        if (workRequest == null)
        {
            string message = $"No work request found with Id: {workRequestId}";
            _logger.LogError(message);
            TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: message));
            return BadRequest();
        }


      IEnumerable<FileUpload> supportingFiles = _workRequestRepository.GetFileUploadsByWorkRequestId(workRequestId);

      if (supportingFiles == null || !supportingFiles.Any())
      {
        // Not a fatal error as not all Requests have files.
        return NoContent();
      }

      string jsonString = JsonConvert.SerializeObject(new SupportingFilesTableViewModel(supportingFiles));
      return Ok(jsonString);
    }

    /// <summary>
    /// Get a supporting file by it's fileId.
    /// </summary>
    /// <param name="fileId">Id of the file to download</param>
    /// <returns>Returns FileContentResult in success. Returns BadRequest on error.</returns>
    [HttpGet]
    public IActionResult DownloadSupportingFile(int? fileId)
    {
      // Check the file Id supplied is 'physical'.
      if (fileId == null || fileId < 0)
      {
        string message = fileId is null ? "File Id is null" : $"File Id is not valid: {fileId}";
        _logger.LogError(message);
        TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "Invalid file Id."));
        return BadRequest();
      }

      // Get the file from the Repository.
      FileUpload supportingFile = _fileRepository.GetByID(fileId);

      // If not file, this file is missing, this is an error.
      if (supportingFile == null)
      {
        _logger.LogError($"No supporting file found for file Id: {fileId}");
        TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "No file found."));
        return BadRequest();
      }

      // Return the FileContentResult.
      return File(supportingFile.File, System.Net.Mime.MediaTypeNames.Application.Octet, supportingFile.FileName);
    }

    /// <summary>
    /// Performs deletion of file from DB.
    /// </summary>
    /// </summary>
    /// <param name="fileId"></param>
    /// <returns>Ok when deletion successful, otherwise BadRequest with TempDate["PopupMessage"].</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteSupportingFileAsync(int? fileId, int workRequestId)
    {
      WorkRequest workRequest = _workRequestRepository.GetWorkRequest(workRequestId);

      // Dummy check for workRequestId
      if (workRequest is null)
      {
        string message = $"Work Request ID is not valid: {workRequestId}";
        _logger.LogError(message);
        TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "Invalid Work Request Id."));
        return RedirectToAction("Index", "ViewAllWorkRequests");
      }

      string redirectUrl = Url.Action("WorkRequestDetails", "WorkRequest", new { workRequestId = workRequestId });

      // Dummy check for fileId
      if (fileId == null || fileId < 0)
      {
        string message = fileId is null ? "File Id is null" : $"File Id is not valid: {fileId}";
        _logger.LogError(message);
        TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "Invalid file Id."));
        return Redirect(redirectUrl);
      }

      FileUpload supportingFile = _fileRepository.GetByID(fileId);

      // Check if file exists for Id
      if (supportingFile is null)
      {
        string message = $"File cannot be found with Id: {fileId}";
        _logger.LogError(message);
        TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "File cannot be found."));
        return Redirect(redirectUrl);
      }


      // Delete the file from the Repository.
      try
      {
        _fileRepository.Delete(fileId);
        _fileRepository.Save();

        ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);

        WorkRequestEvent workRequestEvent = new()
        {
          WorkRequest = workRequest,
          Content = $"{CommonHelpers.RemoveDomainFromEmail(user.Email)} - deleted file - {supportingFile.ReadableFileName} at {DateTime.Now:HH:mm}",
          CreatedAt = DateTime.Now,
          CreatedBy = user,
          EventType = _workRequestRepository.GetWorkRequestEventType(WorkRequestEventTypeEnum.FileManagement)
        };

        _workRequestRepository.InsertWorkRequestEvent(workRequestEvent);
        _workRequestRepository.Save();

        string jsonString = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: true, message: "File deleted."));
        return Redirect(redirectUrl);

      }
      catch
      {
        _logger.LogError($"Deletion failed for file Id: {fileId}");
        TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "Could not delete file."));
        return Redirect(redirectUrl);
      }

    }

    #endregion

    #region Labels
    /// <summary>
    /// Fetches a list of Labels with the option to include only those available to be added to the given work request.
    /// </summary>
    /// <param name="workRequestId">Id of the work request to get labels for</param>
    /// <param name="includeOnlyAvailableToAdd">Optional: true returns only Labels available to add. false returns only Labels already added.</param>
    /// <returns>LabelsViewModel</returns>
    [HttpGet]
    public IActionResult GetLabelsOnWorkRequest(int workRequestId, bool includeOnlyAvailableToAdd = false)
    {
      if (workRequestId <= 0)
        return BadRequest("Invalid workRequestId");

      IEnumerable<DataAccessLayer.Models.Label> labels = _workRequestRepository.GetLabelsByWorkRequestId(workRequestId)?.OrderBy(o => o.LabelShort);
      if (labels == null && !includeOnlyAvailableToAdd)
        return NoContent();

      if (includeOnlyAvailableToAdd)
      {
        // Get all labels that are unarchived and do not already exist on the request.
        IEnumerable<DataAccessLayer.Models.Label> availableLabels = _labelRepository.GetUnarchivedLabels().Where(q => !labels.Any(x => x.LabelId == q.LabelId)).OrderBy(o => o.LabelShort);
        LabelsViewModel availableLabelsViewModel = new LabelsViewModel(availableLabels);
        availableLabelsViewModel.Labels.ForEach(x => x.WorkRequestId = workRequestId);
        return Ok(JsonConvert.SerializeObject(availableLabelsViewModel));
      }

      LabelsViewModel labelsViewModel = new LabelsViewModel(labels);
      labelsViewModel.Labels.ForEach(x => x.WorkRequestId = workRequestId);
      return Ok(JsonConvert.SerializeObject(labelsViewModel));
    }

    /// <summary>
    /// Fetches a list of all Labels that are either Active or all Labels (Archived and Active).
    /// </summary>
    /// <param name="includeArchived">Optional: true returns all Labels. false returns Active Labels only.</param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult GetLabels(bool includeArchived = false)
    {
      IEnumerable<DataAccessLayer.Models.Label> labels = null;
      if (includeArchived)
        labels = _labelRepository.GetLabels()?.OrderBy(o => o.LabelShort);
      else
        labels = _labelRepository.GetUnarchivedLabels()?.OrderBy(o => o.LabelShort);

      if (labels == null)
        return NoContent();

      LabelsViewModel labelsViewModel = new(labels);
      return Ok(JsonConvert.SerializeObject(labelsViewModel));
    }

    /// <summary>
    /// Adds a Label to a Work Request.
    /// </summary>
    /// <param name="workRequestId">Id of Work Request</param>
    /// <param name="labelId">Id of Label to add to Work Request</param>
    /// <returns></returns>
    [HttpPut]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Authoriser")]
    public async Task<IActionResult> PutLabelOnWorkRequestAsync(int workRequestId, int labelId, string executorEmail)
    {
      return await AddLabelOnWorkRequestAsync(workRequestId, labelId, executorEmail);
    }

    private async Task<IActionResult> AddLabelOnWorkRequestAsync(int workRequestId, int labelId, string executorEmail)
    {
      try
      {
        if (executorEmail == null)
        {
          ApplicationUser executor = await _userManager.FindByNameAsync(User.Identity?.Name);
          executorEmail = executor.Email;
        }
        await _workRequestService.AddLabelToWorkRequestAsync(workRequestId, labelId, executorEmail);
        return Ok();
      }
      catch (ArgumentException ex)
      {
        return BadRequest(ex.Message);
      }
      catch (InvalidOperationException ex)
      {
        return BadRequest(ex.Message);
      }
    }

    /// <summary>
    /// Removes a specified Label from a Work Request.
    /// </summary>
    /// <param name="workRequestId">Id of Work Request</param>
    /// <param name="labelId">Id of Label to remove from Work Request</param>
    /// <returns></returns>
    [HttpPut]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Authoriser")]
    public async Task<IActionResult> PutRemoveLabelOnWorkRequestApiAsync(int workRequestId, int labelId)
    {
      try
      {
        ApplicationUser executor = await _userManager.FindByNameAsync(User.Identity?.Name);
        await _workRequestService.RemoveLabelToWorkRequestAsync(workRequestId, labelId, executor.Email);
        return Ok();
      }
      catch (ArgumentException ex)
      {
        return BadRequest(ex.Message);
      }
      catch (InvalidOperationException ex)
      {
        return BadRequest(ex.Message);
      }
    }

    #endregion

    #region Assignees

    /// <summary>
    /// Gets a list of Assigned Users and a list of Assignable Users for a given <paramref name="workRequestId"/>.
    /// </summary>
    /// <param name="workRequestId">Id of the Work Request</param>
    /// <returns>AssigneesViewModel</returns>
    [HttpGet]
    public IActionResult GetAssigneesOnWorkRequest(int workRequestId)
    {
      if (workRequestId <= 0)
        return BadRequest("Invalid workRequestId");


      AssigneesViewModel assigneesViewModel = new()
      {
        AssignedUsers = _workRequestRepository
                              .GetAssigneesByWorkRequestId(workRequestId)
                              .OrderBy(o => o.UserName)
                              ?.Select(x => new UserSimpleViewModel()
                              {
                                UserId = x.Id,
                                Email = x.Email,
                                Username = x.UserName
                              })
                              ?.ToList()
                              ?? new(),
        WorkRequestId = workRequestId
      };

      // Get all assignable users then filter out any already assigned.
      // Expect that assigned users is << assignable users. 
      assigneesViewModel.AssignableUsers = _workRequestService
                                                      .GetAuthorisers(isActiveOnly: true)
                                                      ?.OrderBy(o => o.Username)
                                                      ?.ToList()
                                                      ?? new();
      if (assigneesViewModel.AssignedUsers.Any())
      {
        foreach (UserSimpleViewModel user in assigneesViewModel.AssignedUsers)
        {
          assigneesViewModel.AssignableUsers.RemoveAll(q => q.UserId == user.UserId);
        }

      }

      return Ok(JsonConvert.SerializeObject(assigneesViewModel));
    }

    /// <summary>
    /// Assigns a user onto a Work Request
    /// </summary>
    /// <param name="workRequestId">Id of the Work Request to assign to</param>
    /// <param name="assigneeEmail">Email of user to assign</param>
    /// <returns></returns>
    [HttpPut]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Authoriser")]
    public async Task<IActionResult> PutAssigneeOnWorkRequest(int workRequestId, string assigneeEmail)
    {
      try
      {
        ApplicationUser executor = await _userManager.FindByNameAsync(User.Identity?.Name);
        await _workRequestService.AssignUserToWorkRequestAsync(workRequestId, assigneeEmail, executor.Email, GetLinkUrl(workRequestId), false);
        return Ok();
      }
      catch (ArgumentException ex)
      {
        return BadRequest(ex.Message);
      }
      catch (InvalidOperationException ex)
      {
        return BadRequest(ex.Message);
      }
    }

    /// <summary>
    /// Unassign a user from a Work Request
    /// </summary>
    /// <param name="workRequestId">Id of the work request to unassign a user from</param>
    /// <param name="assigneeEmail">Email address of user to unassign from work request</param>
    /// <returns></returns>
    [HttpPut]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Authoriser")]
    public async Task<IActionResult> PutRemoveAssigneeOnWorkRequest(int workRequestId, string assigneeEmail)
    {
      try
      {
        ApplicationUser executor = await _userManager.FindByNameAsync(User.Identity?.Name);
        await _workRequestService.UnassignUserToWorkRequestAsync(workRequestId, assigneeEmail, executor.Email, false);
        return Ok();
      }
      catch (ArgumentException ex)
      {
        return BadRequest(ex.Message);
      }
      catch (InvalidOperationException ex)
      {
        return BadRequest(ex.Message);
      }
    }

    #endregion

    #region Subscriptions
    /// <summary>
    /// Subscribes a user to a Work Request
    /// </summary>
    /// <param name="workRequestId">Id of the Work Request to subscribe to</param>
    /// <param name="userId">Identity user id</param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubscribeUserToRequest(int? workRequestId, string currentUserId, string targetUserId)
    {
      if (workRequestId is null)
        return BadRequest("Invalid workRequestId");
      if (string.IsNullOrWhiteSpace(currentUserId))
        return BadRequest("Invalid currentUserId");
      if (string.IsNullOrWhiteSpace(targetUserId))
        return BadRequest("Invalid targetUserId");

      WorkRequest workRequest = _workRequestRepository.GetWorkRequest(workRequestId.Value);
      if (workRequest == null)
        return BadRequest("Invalid workRequestId");

      ApplicationUser? currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
      ApplicationUser? targetUser = await _userManager.FindByIdAsync(targetUserId);
      if (currentUser is null || targetUser is null)
        return BadRequest("Invalid userId");

      await _workRequestSubscriptionRepository.CreateWorkRequestSubscription(currentUser.Id, targetUser.Id, workRequestId.Value);

      return Ok();
    }


    /// <summary>
    /// Unsubscribes a user from a Work Request
    /// </summary>
    /// <param name="workRequestId">Id of the Work Request to unsubscribe from</param>
    /// <param name="userId">Identity user ide</param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UnsubscribeUserFromRequest(int? workRequestId, string currentUserId, string targetUserId)
    {
      if (workRequestId is null)
        return BadRequest("Invalid workRequestId");
      if (string.IsNullOrWhiteSpace(currentUserId))
        return BadRequest("Invalid currentUserId");
      if (string.IsNullOrWhiteSpace(targetUserId))
        return BadRequest("Invalid targetUserId");

      WorkRequest workRequest = _workRequestRepository.GetWorkRequest(workRequestId.Value);
      if (workRequest == null)
        return BadRequest("Invalid workRequestId");

      ApplicationUser? currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
      ApplicationUser? targetUser = await _userManager.FindByIdAsync(targetUserId);
      if (currentUser is null || targetUser is null)
        return BadRequest("Invalid userId");

      await _workRequestSubscriptionService.UnsubscribeUserFromWorkRequest(currentUser.Id, targetUser.Id, workRequest.WorkRequestId);

      return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> SubscribeMultipleUsersToRequest(SubscribeOtherUsersViewModel model)
    {
      if (model.UsersToBeSubscribed == null || !model.UsersToBeSubscribed.Any())
      {
        TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "No users selected"));

        return RedirectToAction("WorkRequestDetails", new { workRequestId = model.WorkRequestId });
      }

      foreach (string targetUserId in model.UsersToBeSubscribed)
      {
        // Reuse the same logic
        IActionResult result = await SubscribeUserToRequest(model.WorkRequestId, model.CurrentUserId, targetUserId);

        if (result is not OkResult)
        {
          continue;
        }
      }

      return RedirectToAction("WorkRequestDetails", new { workRequestId = model.WorkRequestId });
    }

    #endregion

    #region GitHub
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Authoriser")]
    public async Task<IActionResult> CreateGitHubIssue(int? workRequestId, long? repositoryId)
    {
      if (workRequestId is null || repositoryId is null)
      {
        return BadRequest();
      }
      try
      {
        ApplicationUser executor = _userManager.FindByNameAsync(User.Identity.Name).Result;

        Octokit.Issue issueResult = await _gitHubService.CreateWorkRequestGitHubIssue(workRequestId.Value, repositoryId, GetLinkUrl(workRequestId.Value), executor.Email);

        if (issueResult == null)
        {
          TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "An error occurred while creating a GitHub issue."));
          return StatusCode(500);
        }

        int labelId = _labelRepository.GetLabels().FirstOrDefault(l => string.Equals(l.LabelShort, "github", StringComparison.OrdinalIgnoreCase)).LabelId;
        IActionResult githubLabelResult = await AddLabelOnWorkRequestAsync(workRequestId.Value, labelId, "system@york.ac.uk");
        return Ok(issueResult.HtmlUrl);

      }
      catch (Exception ex)
      {
        _logger.LogError(ex, ex.Message);
        TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "An error occurred while creating a GitHub issue."));
        return StatusCode(500);
      }
    }

    [HttpPut]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Authoriser")]
    public async Task<IActionResult> UpdateGitHubIssueOnRequest(int? workRequestId, long? repositoryId, string githubIssueNumberOrUrl)
    {
      int githubIssueNumberCleaned = 0;

      // Validate parameters and clean
      if (workRequestId is null)
        return BadRequest();

      if (repositoryId is null)
        return BadRequest();
      
      if (githubIssueNumberOrUrl is null)
        return BadRequest();


      else if (Regex.IsMatch(githubIssueNumberOrUrl, "^(https:\\/\\/github.com\\/uoy-trials\\/)+\\S+(\\/issues\\/)+\\d+$")) // url like : "https://github.com/uoy-trials/" + some non-whitespace + "/issues/" + some digits
        githubIssueNumberCleaned = Convert.ToInt32(githubIssueNumberOrUrl.Split('/').Last());
      else if (Regex.IsMatch(githubIssueNumberOrUrl, "^\\d+$")) // just a number
        githubIssueNumberCleaned = Convert.ToInt32(githubIssueNumberOrUrl);

      // Update work request
      ApplicationUser executor = _userManager.FindByNameAsync(User.Identity.Name).Result;

      await _gitHubService.UpdateWorkRequestWithGitHubIssue(workRequestId.Value, repositoryId.Value, githubIssueNumberCleaned, executor.Email);

      // Add github label to request if not already
      int labelId = _labelRepository.GetLabels().FirstOrDefault(l => string.Equals(l.LabelShort, "github", StringComparison.OrdinalIgnoreCase)).LabelId;
      IActionResult githubLabelResult = await AddLabelOnWorkRequestAsync(workRequestId.Value, labelId, "system@york.ac.uk");

      return Ok();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Authoriser")]
    public async Task<IActionResult> ResetGitHubInfo(int? workRequestId)
    {
      if (workRequestId is null)
      {
        return BadRequest();
      }
      try
      {
        ApplicationUser executor = _userManager.FindByNameAsync(User.Identity.Name).Result;

        await _gitHubService.ResetGitHubInfo(workRequestId.Value, executor.Email);

        _logger.LogInformation($"GitHub information reset for WorkRequestId: {workRequestId}");

        return Ok(new { message = "GitHub information has been reset." });
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"Error resetting GitHub information for WorkRequestId: {workRequestId}");
        TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "An error occurred while resetting GitHub information."));
        return StatusCode(500);
      }
    }

    #endregion

    #region SubTasks

    [HttpPost]
    public async Task<IActionResult> AddSubTask(SubTaskAddViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return RedirectToAction("WorkRequestDetails", "WorkRequest", new { workRequestId = model.WorkRequestId });
      }

      SubTask subTask = _subTaskService.CreateSubTask(new SubTask
      {
        SubTaskTitle = model.SubTaskTitle,
        Assignee = await _userManager.FindByEmailAsync(model.AssignedUserEmail),
        WorkRequestId = model.WorkRequestId,
        Status = _subTaskStatusRepository.GetByID(SubTaskStatusEnum.Open),
        CreatedBy = GetCurrentUserEmail(),
        CreationDateTime = DateTime.Now,
        LastEditedBy = GetCurrentUserEmail(),
        LastEditedDateTime = DateTime.Now
      });

      // Send initial creation email only to the requester.
      await _notificationService.CreateNewSubTaskNotificationAsync(subTask.SubTaskId, GetLinkUrl(subTask.WorkRequestId));

      // Send assigned email to the assignee.
      await _notificationService.CreateAssignedToSubTaskNotificationsAsync(subTask.SubTaskId, GetLinkUrl(subTask.WorkRequestId));

      TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: true, message: "Sub task created."));

      return RedirectToAction("WorkRequestDetails", new { workRequestId = model.WorkRequestId });
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubTaskEvent(WorkRequestDetailsViewModel model)
    {
      WorkRequest? workRequest = _workRequestRepository.GetWorkRequestBySubTask(model.NewSubTaskEvent.SubTask.SubTaskId);

      if (workRequest == null)
      {
        TempData["PopupMessage"] = JsonConvert.SerializeObject(
            new PopupMessageViewModel(false, "Work Request not found.")
        );
        return RedirectToAction("Index", "ViewAllWorkRequests");
      }

      if (!ModelState.IsValid)
      {
        TempData["PopupMessage"] = JsonConvert.SerializeObject(
            new PopupMessageViewModel(false, "Form submission was invalid.")
        );
        return RedirectToAction("WorkRequestDetails", "WorkRequest", new { workRequestId = workRequest.WorkRequestId });
      }

      SubTask? subTask = _subTaskRepository.GetSubTask(model.NewSubTaskEvent.SubTask.SubTaskId);

      if (subTask == null)
      {
        TempData["PopupMessage"] = JsonConvert.SerializeObject(
            new PopupMessageViewModel(false, "Sub-task not found.")
        );
        return RedirectToAction("WorkRequestDetails", "WorkRequest", new { workRequestId = workRequest.WorkRequestId });
      }

      // Empty descriptions on Approvals/Rejections are disallowed.
      if (string.IsNullOrWhiteSpace(model.NewSubTaskEvent.Content))
      {
        TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: $"Please provide an event description."));
        return RedirectToAction("WorkRequestDetails", "WorkRequest", new { workRequestId = workRequest.WorkRequestId });
      }

      ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);

      SubTaskEvent newSubTaskEvent = new SubTaskEvent
      {
        SubTask = subTask,
        Content = model.NewSubTaskEvent.Content,
        CreatedAt = DateTime.Now,
        CreatedBy = user,
        EventType = _subTaskRepository.GetSubTaskEventsType(model.NewSubTaskEvent.EventType.SubTaskEventTypeId)
      };

      _subTaskRepository.InsertSubTaskEvent(newSubTaskEvent);

      switch (newSubTaskEvent.EventType.SubTaskEventTypeId)
      {
        case (SubTaskEventTypeEnum.Approve):
          subTask.Status = _subTaskStatusRepository.Get(q => q.SubTaskStatusId == SubTaskStatusEnum.Approved).Single();
          await _notificationService.CreateApprovedNotificationsForSubTaskAsync(subTask.SubTaskId, GetLinkUrl(subTask.WorkRequestId), newSubTaskEvent.Content);
          break;
        case (SubTaskEventTypeEnum.Reject):
          subTask.Status = _subTaskStatusRepository.Get(q => q.SubTaskStatusId == SubTaskStatusEnum.Rejected).Single();
          await _notificationService.CreateRejectionNotificationsForSubTaskAsync(subTask.SubTaskId, GetLinkUrl(subTask.WorkRequestId), newSubTaskEvent.Content);
          break;
        case (SubTaskEventTypeEnum.Abandon):
          subTask.Status = _subTaskStatusRepository.Get(q => q.SubTaskStatusId == SubTaskStatusEnum.Abandoned).Single();
          await _notificationService.CreateAbandonedNotificationsForSubTasksAsync(subTask.SubTaskId, GetLinkUrl(subTask.WorkRequestId), newSubTaskEvent.Content);
          break;
        case (SubTaskEventTypeEnum.None):
          await _notificationService.CreateMessageNotificationsForSubTasks(subTask.SubTaskId, GetLinkUrl(subTask.WorkRequestId), newSubTaskEvent);
          break;
        default:
          _logger.LogError("Invalid Work Request Event Type! Type: '" + newSubTaskEvent.EventType.SubTaskEventTypeId + "'.");
          break;
      }

      subTask.LastEditedDateTime = DateTime.Now;
      subTask.LastEditedBy = user.Email;

      _subTaskRepository.Update(subTask);
      _subTaskRepository.Save();

      return RedirectToAction("WorkRequestDetails", "WorkRequest", new { workRequestId = workRequest.WorkRequestId });
    }

    /// <summary>
    /// Saves a new assignee against a sub task
    /// </summary>
    /// <param name="subTaskId"></param>
    /// <param name="assigneeEmail"></param>
    /// <returns></returns>
    [HttpPut]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PutAssigneeEmailOnSubTask(int subTaskId, string assigneeEmail)
    {
      SubTask? subTask = _subTaskRepository.GetSubTask(subTaskId);
      ApplicationUser? newAssignee = await _userManager.FindByEmailAsync(assigneeEmail);
      if (subTask == null)
      {
        string message = $"{nameof(PutAssigneeEmailOnSubTask)}: Sub Task not found/does not exist for Sub Task ID '{subTaskId}'.";
        TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "Sub task could not be found."));
        _logger.LogError(message);
        return BadRequest(message);
      }
      else if (newAssignee == null)
      {
        string message = $"{nameof(PutAssigneeEmailOnSubTask)}: Assignee not found/does not exist for email '{assigneeEmail}'.";
        TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "Assignee could not be found."));
        _logger.LogError(message);
        return BadRequest(message);
      }

      ApplicationUser executor = _userManager.FindByNameAsync(User.Identity.Name).Result;

      string executorName = CommonHelpers.RemoveDomainFromEmail(executor.Email);
      string previousAssigneeName = CommonHelpers.RemoveDomainFromEmail(subTask.Assignee.Email);
      string newAssigneeName = CommonHelpers.RemoveDomainFromEmail(newAssignee.Email);

      //Create change of assigned user subTaskEvent event for the action
      SubTaskEvent subTaskEvent = new()
      {
        SubTask = subTask,
        Content = $"{executorName} - changed assignee - from {previousAssigneeName} to {newAssigneeName} at {DateTime.Now:HH:mm}",
        CreatedAt = DateTime.Now,
        CreatedBy = executor,
        EventType = _subTaskRepository.GetSubTaskEventsType(SubTaskEventTypeEnum.Assignment)
      };
      _subTaskRepository.InsertSubTaskEvent(subTaskEvent);

      subTask.Assignee = newAssignee;
      subTask.LastEditedBy = executor.Email;
      subTask.LastEditedDateTime = subTaskEvent.CreatedAt;

      _subTaskRepository.Save();

      // Send assigned email to the new assignee.
      await _notificationService.CreateAssignedToSubTaskNotificationsAsync(subTask.SubTaskId, GetLinkUrl(subTask.WorkRequestId));

      TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: true, message: "Sub task assignee updated"));
      return Ok();
    }

    #endregion

    #region Exports

    [HttpPost]
    public async Task<IActionResult> ExportWorkRequestAsync(int? workRequestId)
    {
      if (workRequestId is null)
        return BadRequest();
      WorkRequest workRequest = _workRequestRepository.GetWorkRequest(workRequestId.Value);
      if (workRequest == null)
        return BadRequest("Invalid workRequestId");
      if (workRequest.Status.WorkRequestStatusId != WorkRequestStatusEnum.Completed
          && workRequest.Status.WorkRequestStatusId != WorkRequestStatusEnum.Abandoned)
        return BadRequest("Work Request is not closed");

      try
      {
        string fileName = _dataExportService.GetExportName(workRequestId.Value, workRequest.Trial.TrialName);

        byte[] file = await _dataExportService.GenerateExport(workRequestId.Value, User.Identity.Name);

        FileContentResult fileForDownload = File(file, "application/pdf", fileName);

        return fileForDownload;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"Export failed for work request Id {workRequestId}");
        TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "Export failed, please try again."));
        return RedirectToAction("WorkRequestDetails", "WorkRequest", new { workRequestId = workRequest.WorkRequestId });
      }
    }

    //[HttpPost]
    //public async Task<IActionResult> ExportAllWorkRequestForTrial(int? trialId, int? originatingRequestId)
    //{
    //    if (trialId is null)
    //    {
    //        return BadRequest();
    //    }

    //    try
    //    {
    //        FileContentResult export = await _workRequestExportService.GenerateExport(trialId.Value, User.Identity?.Name);
    //        if (export is null)
    //        {

    //        }
    //    }

    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, $"Export failed for work request Id {workRequestId}");
    //        TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "Export failed, please try again."));
    //        return RedirectToAction("WorkRequestDetails", "WorkRequest", new { workRequestId = workRequest.WorkRequestId });
    //    }
    //    return RedirectToAction();
    //}

    #endregion

    }
}