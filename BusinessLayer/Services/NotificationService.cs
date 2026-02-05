using BusinessLayer.Helpers;
using BusinessLayer.Repositories.Interfaces;
using BusinessLayer.Services.EmailProcessing;
using BusinessLayer.Services.EmailProcessing.EmailHelpers;
using BusinessLayer.Services.Interfaces;
using BusinessLayer.ViewModels;
using DataAccessLayer.Models;
using Enums.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using YTU.EmailService;

namespace BusinessLayer.Services
{
  public class NotificationService : INotificationService
  {
    private readonly ILogger<NotificationService> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationTypeRepository _notificationTypeRepository;
    private readonly IWorkRequestRepository _workRequestRepository;
    private readonly EmailHandlerService _emailHandlerService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWorkRequestSubscriptionRepository _workRequestSubscriptionRepository;
    private readonly ISubTaskRepository _subTaskRepository;

    public NotificationService(
        ILogger<NotificationService> logger,
        UserManager<ApplicationUser> userManager,
        INotificationRepository notificationRepository,
        INotificationTypeRepository notificationTypeRepository,
        IWorkRequestRepository workRequestRepository,
        EmailHandlerService emailHandlerService,
        IHttpContextAccessor httpContextAccessor,
        IWorkRequestSubscriptionRepository workRequestSubscriptionRepository,
        ISubTaskRepository subTaskRepository)
    {
      _logger = logger;
      _userManager = userManager;
      _notificationRepository = notificationRepository;
      _notificationTypeRepository = notificationTypeRepository;
      _workRequestRepository = workRequestRepository;
      _emailHandlerService = emailHandlerService;
      _httpContextAccessor = httpContextAccessor;
      _workRequestSubscriptionRepository = workRequestSubscriptionRepository;
      _subTaskRepository = subTaskRepository;
    }

    #region Private helpers
    /// <summary>
    /// Gets a work request using an Id.
    /// </summary>
    /// <param name="workRequestId"></param>
    /// <param name="callingMethodName"></param>
    /// <returns>Returns EF work request object. Returns null if error.</returns>
    private WorkRequest GetWorkRequest(int workRequestId, [CallerMemberName] string callingMethodName = "")
    {
      if (workRequestId <= 0)
      {
        _logger.LogWarning($"{callingMethodName}: Invalid parameter value. {nameof(workRequestId)}: {workRequestId}");
        return null;
      }

      return _workRequestRepository.GetByID(workRequestId);
    }
    /// <summary>
    /// Gets a notification type using an NotificationTypeEnum value.
    /// </summary>
    /// <param name="notificationTypeEnum"></param>
    /// <param name="callingMethodName"></param>
    /// <returns>Returns EF notification type object. Returns null if error or is not active type.</returns>
    private NotificationType GetNotificationType(NotificationTypeEnum notificationTypeEnum, [CallerMemberName] string callingMethodName = "")
    {
      NotificationType notificationType = _notificationTypeRepository.GetNotificationType(notificationTypeEnum);

      // Check notification type exists.
      if (notificationType == null)
      {
        _logger.LogError($"{callingMethodName}: Invalid parameter value. {nameof(notificationTypeEnum)}: {notificationTypeEnum} = {(int)notificationTypeEnum}");
        return null;
      }

      // Check notification is active.
      if (!notificationType.IsActive)
      {
        _logger.LogError($"{callingMethodName}: NotificationType is not active. Id: {notificationType.NotificationTypeId}, Name: {notificationType.NotificationTypeName}");
        return null;
      }

      // Passed validation checks.
      return notificationType;
    }
    #endregion


    #region Create new


    /// <summary>
    /// Create a notification for the requester and for users with Authoriser role for a new work request submission.
    /// Pending Initial Approval.
    /// </summary>
    /// <param name="workRequestId">Id of the work request the notification is for.</param>
    /// <returns>Returns a count of the number of notifications sent.</returns>
    public async Task<int> CreateInitialSubmissionNotificationsAsync(int workRequestId, string linkUrl)
    {
      int notificationsCreatedCount = 0;

      WorkRequest workRequest = GetWorkRequest(workRequestId);
      if (workRequest is null)
        return 0;

      // Send notification to requester
      NotificationType notificationType = GetNotificationType(NotificationTypeEnum.WorkRequestReceivedToRequester);
      if (notificationType != null)
      {
        string requesterEmail = workRequest.CreatedBy;
        workRequest.LastEditedDateTime = DateTime.Now;

        Notification notification = new()
        {
          NotificationType = notificationType,
          WorkRequest = workRequest,
          Recipient = requesterEmail,
          SentDate = DateTime.Now,
          CreatedBy = workRequest.LastEditedBy
        };
        _notificationRepository.Insert(notification);
        _notificationRepository.Save();
        notificationsCreatedCount++;

        // Re-instantiate so you are not affecting the original description.
        WorkRequest imageStrippedWorkRequest = new()
        {
          WorkRequestId = workRequest.WorkRequestId,
          Trial = workRequest.Trial,
          Reference = workRequest.Reference,
          DetailDescription = HtmlHelper.RemoveImageElementsFromString(workRequest.DetailDescription, true),
          CreationDateTime = workRequest.CreationDateTime,
          LastEditedDateTime = workRequest.LastEditedDateTime
        };

        Notification emailNotification = new()
        {
          NotificationType = notificationType,
          WorkRequest = imageStrippedWorkRequest,
          Recipient = requesterEmail,
          SentDate = DateTime.Now,
          CreatedBy = workRequest.LastEditedBy,
          BccAddresses = "ytu-data+chareq@york.ac.uk"
        };

        await SendNotificationEmailAsync(emailNotification, linkUrl);
      }
      return notificationsCreatedCount;
    }

    /// <summary>
    /// Create a notification for subscribers, for initial approval complete.
    /// Approved & Pending Dev Work.
    /// </summary>
    /// <param name="workRequestId">Id of the work request the notification is for.</param>
    /// <returns>Returns a count of the number of notifications sent.</returns>
    public async Task<int> CreateInitialApprovalNotificationsAsync(int workRequestId, string linkUrl, string message)
    {
      int notificationsCreatedCount = 0;

      WorkRequest workRequest = GetWorkRequest(workRequestId);
      WorkRequestEvent approvalEvent = workRequest.WorkRequestEvents.FirstOrDefault(x => x.EventType.WorkRequestEventTypeId == WorkRequestEventTypeEnum.Approve);
      if (workRequest is null || approvalEvent is null)
        return 0;

      // Send notification to requester
      NotificationType notificationType = GetNotificationType(NotificationTypeEnum.WorkRequestApprovedToRequester);

      List<WorkRequestSubscription> subscribers = _workRequestSubscriptionRepository.GetWorkRequestActiveSubscribers(workRequestId).Result;
      List<string> mailingList = subscribers.Select(x => x.ApplicationUser.Email).Distinct().ToList();
      mailingList.Add(workRequest.CreatedBy);

      if (mailingList is null || !mailingList.Any())
      {
        _logger.LogError($"{nameof(CreateMessageNotifications)}: No active subcribers for work request {workRequestId} found!");
        return 0;
      }

      foreach (string subscriber in mailingList.Distinct())
      {
        if (subscriber.Equals(approvalEvent.CreatedBy.Email, StringComparison.CurrentCultureIgnoreCase))
        {
          //Don't send notification about the approval to the person who just made it.
          continue;
        }

        if (notificationType != null)
        {
          Notification notification = new()
          {
            NotificationType = notificationType,
            WorkRequest = workRequest,
            Recipient = subscriber,
            SentDate = DateTime.Now,
            CreatedBy = workRequest.LastEditedBy
          };
          _notificationRepository.Insert(notification);
          _notificationRepository.Save();

          Notification emailNotification = new()
          {
            NotificationType = notificationType,
            WorkRequest = workRequest,
            Recipient = subscriber,
            SentDate = DateTime.Now,
            CreatedBy = workRequest.LastEditedBy,
            Message = HtmlHelper.RemoveImageElementsFromString(message, true)
          };

          notificationsCreatedCount++;
          await SendNotificationEmailAsync(emailNotification, linkUrl);
        }
      }
      return notificationsCreatedCount;
    }

    /// <summary>
    /// Create a notification to the subscribers for completion of the request.
    /// Released.
    /// </summary>
    /// <param name="workRequestId">Id of the work request the notification is for.</param>
    /// <returns>Returns a count of the number of notifications sent.</returns>
    public async Task<int> CreateCompletedNotificationsAsync(int workRequestId, string linkUrl, string message)
    {
      int notificationsCreatedCount = 0;

      WorkRequest workRequest = GetWorkRequest(workRequestId);
      WorkRequestEvent completedEvent = workRequest.WorkRequestEvents.FirstOrDefault(x => x.EventType.WorkRequestEventTypeId == WorkRequestEventTypeEnum.Approve);

      if (workRequest is null || completedEvent is null)
        return 0;

      // Send notification to subscribers
      NotificationType notificationType = GetNotificationType(NotificationTypeEnum.WorkRequestCompletedToRequester);

      List<WorkRequestSubscription> subscribers = _workRequestSubscriptionRepository.GetWorkRequestActiveSubscribers(workRequestId).Result;
      List<string> mailingList = subscribers.Select(x => x.ApplicationUser.Email).Distinct().ToList();
      mailingList.Add(workRequest.CreatedBy);

      if (mailingList is null || !mailingList.Any())
      {
        _logger.LogError($"{nameof(CreateMessageNotifications)}: No active subcribers for work request {workRequestId} found!");
        return 0;
      }

      foreach (string subscriber in mailingList.Distinct())
      {
        if (subscriber.Equals(completedEvent.CreatedBy.Email, StringComparison.CurrentCultureIgnoreCase))
        {
          //Don't send notification about the completed to the person who just made it.
          continue;
        }

        if (notificationType != null)
        {
          Notification notification = new()
          {
            NotificationType = notificationType,
            WorkRequest = workRequest,
            Recipient = subscriber,
            SentDate = DateTime.Now,
            CreatedBy = workRequest.LastEditedBy
          };
          _notificationRepository.Insert(notification);
          _notificationRepository.Save();

          Notification emailNotification = new()
          {
            NotificationType = notificationType,
            WorkRequest = workRequest,
            Recipient = subscriber,
            SentDate = DateTime.Now,
            CreatedBy = workRequest.LastEditedBy,
            Message = HtmlHelper.RemoveImageElementsFromString(message, true)
          };

          notificationsCreatedCount++;
          await SendNotificationEmailAsync(emailNotification, linkUrl);
        }
      }
      return notificationsCreatedCount;
    }

    /// <summary>
    /// Create a notification to the subscribers for abandon.
    /// Abandoned.
    /// </summary>
    /// <param name="workRequestId">Id of the work request the notification is for.</param>
    /// <returns>Returns a count of the number of notifications sent.</returns>
    public async Task<int> CreateAbandonedNotificationsAsync(int workRequestId, string linkUrl, string message)
    {
      int notificationsCreatedCount = 0;

      WorkRequest workRequest = GetWorkRequest(workRequestId);
      WorkRequestEvent abandonedEvent = workRequest.WorkRequestEvents.FirstOrDefault(x => x.EventType.WorkRequestEventTypeId == WorkRequestEventTypeEnum.Closed);

      if (workRequest is null || abandonedEvent is null)
        return 0;

      // Send notification to subscribers
      NotificationType notificationType = GetNotificationType(NotificationTypeEnum.WorkRequestDeclinedAndAbandonedToRequester);

      List<WorkRequestSubscription> subscribers = _workRequestSubscriptionRepository.GetWorkRequestActiveSubscribers(workRequestId).Result;
      List<string> mailingList = subscribers.Select(x => x.ApplicationUser.Email).Distinct().ToList();
      mailingList.Add(workRequest.CreatedBy);

      if (mailingList == null || !mailingList.Any())
      {
        _logger.LogError($"{nameof(CreateMessageNotifications)}: No active subcribers for work request {workRequestId} found!");
        return 0;
      }

      foreach (string subscriber in mailingList.Distinct())
      {
        if (subscriber.Equals(abandonedEvent.CreatedBy.Email, StringComparison.CurrentCultureIgnoreCase))
        {
          //Don't send notification about the completed to the person who just made it.
          continue;
        }

        if (notificationType != null)
        {
          string requesterEmail = workRequest.CreatedBy;


          Notification notification = new()
          {
            NotificationType = notificationType,
            WorkRequest = workRequest,
            Recipient = subscriber,
            SentDate = DateTime.Now,
            CreatedBy = workRequest.LastEditedBy
          };
          _notificationRepository.Insert(notification);
          _notificationRepository.Save();

          Notification emailNotification = new()
          {
            NotificationType = notificationType,
            WorkRequest = workRequest,
            Recipient = subscriber,
            SentDate = DateTime.Now,
            CreatedBy = workRequest.LastEditedBy,
            Message = HtmlHelper.RemoveImageElementsFromString(message, true)
          };

          notificationsCreatedCount++;
          await SendNotificationEmailAsync(emailNotification, linkUrl);
        }
      }
      return notificationsCreatedCount;
    }

    /// <summary>
    /// Create a notification to the subscribers for rejected with amendments.
    /// Rejected with amendments.
    /// </summary>
    /// <param name="workRequestId">Id of the work request the notification is for.</param>
    /// <returns>Returns a count of the number of notifications sent.</returns>
    public async Task<int> CreateRejectedWithAmendmentsNotificationsAsync(int workRequestId, string linkUrl, string message)
    {
      int notificationsCreatedCount = 0;

      WorkRequest workRequest = GetWorkRequest(workRequestId);
      WorkRequestEvent requestedChangesEvent = workRequest.WorkRequestEvents.FirstOrDefault(x => x.EventType.WorkRequestEventTypeId == WorkRequestEventTypeEnum.Enquiry);

      if (workRequest is null || requestedChangesEvent is null)
        return 0;

      // Send notification to subscribers
      NotificationType notificationType = GetNotificationType(NotificationTypeEnum.WorkRequestDeclinedWithAmendmentsToRequester);

      List<WorkRequestSubscription> subscribers = _workRequestSubscriptionRepository.GetWorkRequestActiveSubscribers(workRequestId).Result;
      List<string> mailingList = subscribers.Select(x => x.ApplicationUser.Email).Distinct().ToList();
      mailingList.Add(workRequest.CreatedBy);

      if (mailingList == null || !mailingList.Any())
      {
        _logger.LogError($"{nameof(CreateMessageNotifications)}: No active subcribers for work request {workRequestId} found!");
        return 0;
      }

      foreach (string subscriber in mailingList.Distinct())
      {
        if (subscriber.Equals(requestedChangesEvent.CreatedBy.Email, StringComparison.CurrentCultureIgnoreCase))
        {
          //Don't send notification about the completed to the person who just made it.
          continue;
        }

        if (notificationType != null)
        {
          string requesterEmail = workRequest.CreatedBy;

          Notification notification = new()
          {
            NotificationType = notificationType,
            WorkRequest = workRequest,
            Recipient = subscriber,
            SentDate = DateTime.Now,
            CreatedBy = workRequest.LastEditedBy
          };
          _notificationRepository.Insert(notification);
          _notificationRepository.Save();

          Notification emailNotification = new()
          {
            NotificationType = notificationType,
            WorkRequest = workRequest,
            Recipient = subscriber,
            SentDate = DateTime.Now,
            CreatedBy = workRequest.LastEditedBy,
            Message = HtmlHelper.RemoveImageElementsFromString(message, true)
          };

          notificationsCreatedCount++;
          await SendNotificationEmailAsync(emailNotification, linkUrl);
        }
      }
      return notificationsCreatedCount;
    }


    /// <summary>
    /// Create a notification to the assignee for assigned and unassigned.
    /// </summary>
    /// <param name="workRequestId">Id of the work request the notification is for.</param>
    /// <param name="assigneeEmail">Email address of assignee.</param>
    /// <param name="isAssigned">Used to determine if being assigned or unassigned. This is the state at present following any change.</param>
    /// <returns>Returns a count of the number of notifications sent.</returns>
    public async Task<int> CreateAssignedToRequestNotifications(int workRequestId, string assigneeEmail, bool autoAssign, bool isAssigned, string linkUrl)
    {
      int notificationsCreatedCount = 0;

      WorkRequest workRequest = GetWorkRequest(workRequestId);

      if (workRequest == null)
        return 0;

      // Send notification to assignee
      NotificationType notificationType = isAssigned ? GetNotificationType(NotificationTypeEnum.WorkRequestAssignedToAssignee) : GetNotificationType(NotificationTypeEnum.WorkRequestUnassignedToAssignee);
      if (notificationType != null)
      {
        Notification notification = new()
        {
          NotificationType = notificationType,
          WorkRequest = workRequest,
          Recipient = assigneeEmail,
          SentDate = DateTime.Now
        };

        // Assignee actions are not recorded against the last edited value so modification is required.
        workRequest.LastEditedDateTime = DateTime.Now;

        ClaimsPrincipal httpUser = _httpContextAccessor.HttpContext?.User;
        if (httpUser == null)
        {
          notification.CreatedBy = "system";
        }
        else
        {
          ApplicationUser usr = await _userManager.GetUserAsync(httpUser);
          notification.CreatedBy = !autoAssign ? (usr?.Email ?? "system") : "system";
        }

        _notificationRepository.Insert(notification);
        _notificationRepository.Save();
        notificationsCreatedCount++;

        await SendNotificationEmailAsync(notification, linkUrl);
      }

      return notificationsCreatedCount;
    }

    /// <summary>
    /// Create a notification to all subscribers, except for the user who created the message
    /// </summary>
    /// <param name="workRequestId">Id of the work request the notification is for</param>
    /// <param name="linkUrl">URL of the work request details page that contains the 'conversation'</param>
    /// <param name="workRequestEvent">WorkRequestEvent object that contains details of the event that triggered this notification</param>
    /// <returns></returns>
    public async Task<int> CreateMessageNotifications(int workRequestId, string linkUrl, WorkRequestEvent workRequestEvent)
    {
      WorkRequest workRequest = GetWorkRequest(workRequestId);
      if (workRequest == null)
        return 0;

      NotificationType notificationType = GetNotificationType(NotificationTypeEnum.WorkRequestMessage);
      if (notificationType == null)
        return 0;

      List<WorkRequestSubscription> subscribers = _workRequestSubscriptionRepository.GetWorkRequestActiveSubscribers(workRequestId).Result;
      if (subscribers == null || !subscribers.Any())
      {
        _logger.LogError($"{nameof(CreateMessageNotifications)}: No active subcribers for work request {workRequestId} found!");
        return 0;
      }

      int notificationsCreatedCount = 0;
      foreach (WorkRequestSubscription subscriber in subscribers)
      {
        if (string.IsNullOrWhiteSpace(subscriber.ApplicationUser.Email))
        {
          _logger.LogError($"{nameof(CreateMessageNotifications)}: No recipient email. Id: {subscriber.ApplicationUser.Id}");
          continue;
        }

        if (subscriber.ApplicationUser.Email.Equals(workRequestEvent.CreatedBy.Email, StringComparison.CurrentCultureIgnoreCase))
        {
          //Don't send notification about the message to the person who just made it.
          continue;
        }

        Notification notification = new Notification
        {
          NotificationType = notificationType,
          WorkRequest = workRequest,
          Recipient = subscriber.ApplicationUser.Email,
          SentDate = DateTime.Now,
          CreatedBy = workRequest.LastEditedBy
        };
        _notificationRepository.Insert(notification);
        _notificationRepository.Save();

        Notification emailNotification = new()
        {
          NotificationType = notificationType,
          WorkRequest = workRequest,
          Recipient = subscriber.ApplicationUser.Email,
          SentDate = DateTime.Now,
          CreatedBy = workRequest.LastEditedBy,
          Message = HtmlHelper.RemoveImageElementsFromString(workRequestEvent.Content, true)
        };

        notificationsCreatedCount++;
        await SendNotificationEmailAsync(emailNotification, linkUrl);
      }

      return notificationsCreatedCount;
    }

    #endregion

    #region EMAIL
    private async Task SendNotificationEmailAsync(Notification notification, string linkUrl)
    {
      RazorToHtmlParser razorParser = new();

      NotificationEmailViewModel notificationEmailViewModel = new()
      {
        Notification = notification,
        LinkToSystem = linkUrl
      };


      try
      {
        string notificationEmail = await razorParser.RenderHtmlStringAsync("NotificationEmail", notificationEmailViewModel);

        Email email = new()
        {
          Subject = notification.WorkRequest.Trial.TrialName + " - " + notification.WorkRequest.Reference,
          Body = notificationEmail,
          CustomFooter = "",
          RemoveDevEmailBody = false,
          ToAddresses = notification.Recipient,
          BccAddresses = notification.BccAddresses
        };

        await _emailHandlerService.SendEmailAsync(email);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
    }
    #endregion

    #region Sub tasks

    /// <summary>
    /// Create a notification for the requester for a new sub task submission.
    /// </summary>
    /// <param name="subTaskId">Id of the sub task the notification is for.</param>
    /// <returns>Returns a count of the number of notifications sent.</returns>
    public async Task<int> CreateNewSubTaskNotificationAsync(int subTaskId, string linkUrl)
    {
      int notificationsCreatedCount = 0;

      SubTask subTask = _subTaskRepository.GetSubTask(subTaskId);
      if (subTask == null)
        return 0;

      WorkRequest workRequest = _workRequestRepository.GetWorkRequestBySubTask(subTaskId);
      if (workRequest == null)
        return 0;

      // Send notification to requester and users subscribed to the work request
      NotificationType notificationType = GetNotificationType(NotificationTypeEnum.SubTaskCreation);
      if (notificationType != null)
      {
        List<string> involvedUsers = new List<string>();
        List<WorkRequestSubscription> subscriptions = await _workRequestSubscriptionRepository.GetWorkRequestActiveSubscribers(workRequest.WorkRequestId);
        List<string> workRequestSubscribers = subscriptions.Select(sub => sub.ApplicationUser.Email).Distinct().ToList();

        involvedUsers.AddRange(workRequestSubscribers);
        involvedUsers.Add(subTask.CreatedBy);

        subTask.LastEditedDateTime = DateTime.Now;

        foreach (string involvedUser in involvedUsers)
        {
          Notification notification = new()
          {
            NotificationType = notificationType,
            WorkRequest = workRequest,
            SubTask = subTask,
            Recipient = involvedUser,
            SentDate = DateTime.Now,
            CreatedBy = subTask.LastEditedBy
          };
          _notificationRepository.Insert(notification);
          _notificationRepository.Save();
          notificationsCreatedCount++;

          Notification emailNotification = new()
          {
            NotificationType = notificationType,
            WorkRequest = workRequest,
            SubTask = subTask,
            Recipient = involvedUser,
            SentDate = DateTime.Now,
            CreatedBy = subTask.LastEditedBy
          };

          await SendNotificationEmailAsync(emailNotification, linkUrl);
        }
      }
      return notificationsCreatedCount;
    }

    /// <summary>
    /// Create a notification to the assignee for a subtask
    /// </summary>
    /// <param name="subTaskId">Id of the subtask the notification is for.</param>
    /// <returns>Returns a count of the number of notifications sent.</returns>
    public async Task<int> CreateAssignedToSubTaskNotificationsAsync(int subTaskId, string linkUrl)
    {
      int notificationsCreatedCount = 0;

      SubTask subTask = _subTaskRepository.GetSubTask(subTaskId);
      if (subTask == null)
        return 0;

      WorkRequest workRequest = _workRequestRepository.GetWorkRequestBySubTask(subTaskId);
      if (workRequest == null)
        return 0;

      // Send notification to assignee
      NotificationType notificationType = GetNotificationType(NotificationTypeEnum.SubTaskAssignedToAssignee);
      if (notificationType != null)
      {
        Notification notification = new()
        {
          NotificationType = notificationType,
          WorkRequest = workRequest,
          SubTask = subTask,
          Recipient = subTask.Assignee.Email,
          SentDate = DateTime.Now,
          CreatedBy = subTask.LastEditedBy
        };

        _notificationRepository.Insert(notification);
        _notificationRepository.Save();
        notificationsCreatedCount++;

        await SendNotificationEmailAsync(notification, linkUrl);
      }

      return notificationsCreatedCount;
    }

    /// <summary>
    /// Create a notification to the requester for approval of the sub task.
    /// </summary>
    /// <param name="subTaskId">Id of the work request the notification is for.</param>
    /// <returns>Returns a count of the number of notifications sent.</returns>
    public async Task<int> CreateApprovedNotificationsForSubTaskAsync(int subTaskId, string linkUrl, string message)
    {
      int notificationsCreatedCount = 0;

      SubTask subTask = _subTaskRepository.GetSubTask(subTaskId);
      if (subTask == null)
        return 0;

      WorkRequest workRequest = _workRequestRepository.GetWorkRequestBySubTask(subTaskId);
      if (workRequest == null)
        return 0;

      // Send notification to requester
      NotificationType notificationType = GetNotificationType(NotificationTypeEnum.SubTaskApproval);
      if (notificationType != null)
      {
        string requesterEmail = subTask.CreatedBy;

        Notification notification = new()
        {
          NotificationType = notificationType,
          WorkRequest = workRequest,
          SubTask = subTask,
          Recipient = requesterEmail,
          SentDate = DateTime.Now,
          CreatedBy = subTask.LastEditedBy
        };
        _notificationRepository.Insert(notification);
        _notificationRepository.Save();

        Notification emailNotification = new()
        {
          NotificationType = notificationType,
          WorkRequest = workRequest,
          SubTask = subTask,
          Recipient = requesterEmail,
          SentDate = DateTime.Now,
          CreatedBy = subTask.LastEditedBy,
          Message = HtmlHelper.RemoveImageElementsFromString(message, true)
        };

        notificationsCreatedCount++;
        await SendNotificationEmailAsync(emailNotification, linkUrl);
      }

      return notificationsCreatedCount;
    }

    /// <summary>
    /// Create a notification to the requester for rejection of the sub task.
    /// </summary>
    /// <param name="subTaskId">Id of the work request the notification is for.</param>
    /// <returns>Returns a count of the number of notifications sent.</returns>
    public async Task<int> CreateRejectionNotificationsForSubTaskAsync(int subTaskId, string linkUrl, string message)
    {
      int notificationsCreatedCount = 0;

      SubTask subTask = _subTaskRepository.GetSubTask(subTaskId);
      if (subTask == null)
        return 0;

      WorkRequest workRequest = _workRequestRepository.GetWorkRequestBySubTask(subTaskId);
      if (workRequest == null)
        return 0;

      // Send notification to requester
      NotificationType notificationType = GetNotificationType(NotificationTypeEnum.SubTaskRejection);
      if (notificationType != null)
      {
        string requesterEmail = subTask.CreatedBy;

        Notification notification = new()
        {
          NotificationType = notificationType,
          WorkRequest = workRequest,
          SubTask = subTask,
          Recipient = requesterEmail,
          SentDate = DateTime.Now,
          CreatedBy = subTask.LastEditedBy
        };
        _notificationRepository.Insert(notification);
        _notificationRepository.Save();

        Notification emailNotification = new()
        {
          NotificationType = notificationType,
          WorkRequest = workRequest,
          SubTask = subTask,
          Recipient = requesterEmail,
          SentDate = DateTime.Now,
          CreatedBy = subTask.LastEditedBy,
          Message = HtmlHelper.RemoveImageElementsFromString(message, true)
        };

        notificationsCreatedCount++;
        await SendNotificationEmailAsync(emailNotification, linkUrl);
      }

      return notificationsCreatedCount;
    }

    /// <summary>
    /// Create a notification to the requester for abandonment of a sub task.
    /// Abandoned.
    /// </summary>
    /// <param name="subTaskId">Id of the sub task the notification is for.</param>
    /// <returns>Returns a count of the number of notifications sent.</returns>
    public async Task<int> CreateAbandonedNotificationsForSubTasksAsync(int subTaskId, string linkUrl, string message)
    {
      int notificationsCreatedCount = 0;

      SubTask subTask = _subTaskRepository.GetSubTask(subTaskId);
      if (subTask == null)
        return 0;

      WorkRequest workRequest = _workRequestRepository.GetWorkRequestBySubTask(subTaskId);
      if (workRequest == null)
        return 0;

      // Send notification to requester
      NotificationType notificationType = GetNotificationType(NotificationTypeEnum.SubTaskAbandoned);
      if (notificationType != null)
      {
        string requesterEmail = subTask.CreatedBy;


        Notification notification = new()
        {
          NotificationType = notificationType,
          WorkRequest = workRequest,
          SubTask = subTask,
          Recipient = requesterEmail,
          SentDate = DateTime.Now,
          CreatedBy = subTask.LastEditedBy
        };
        _notificationRepository.Insert(notification);
        _notificationRepository.Save();

        Notification emailNotification = new()
        {
          NotificationType = notificationType,
          WorkRequest = workRequest,
          SubTask = subTask,
          Recipient = requesterEmail,
          SentDate = DateTime.Now,
          CreatedBy = subTask.LastEditedBy,
          Message = HtmlHelper.RemoveImageElementsFromString(message, true)
        };

        notificationsCreatedCount++;
        await SendNotificationEmailAsync(emailNotification, linkUrl);
      }

      return notificationsCreatedCount;
    }

    /// <summary>
    /// Create a notification to all subscribers, except for the user who created the message in a sub task
    /// </summary>
    /// <param name="subTaskId">Id of the sub task the notification is for</param>
    /// <param name="linkUrl">URL of the work request details page that contains the 'conversation'</param>
    /// <param name="subTaskEvent">SubTaskEvent object that contains details of the event that triggered this notification</param>
    /// <returns></returns>
    public async Task<int> CreateMessageNotificationsForSubTasks(int subTaskId, string linkUrl, SubTaskEvent subTaskEvent)
    {
      SubTask subTask = _subTaskRepository.GetSubTask(subTaskId);
      if (subTask == null)
        return 0;

      WorkRequest workRequest = _workRequestRepository.GetWorkRequestBySubTask(subTaskId);
      if (workRequest == null)
        return 0;

      NotificationType notificationType = GetNotificationType(NotificationTypeEnum.SubTaskMessage);
      if (notificationType == null)
        return 0;

      List<ApplicationUser> involvedUsers = _subTaskRepository.GetAllUsersInvolvedInSubTask(subTaskId).ToList();
      if (involvedUsers == null || !involvedUsers.Any())
      {
        return 0;
      }

      int notificationsCreatedCount = 0;
      foreach (ApplicationUser involvedUser in involvedUsers)
      {
        if (string.IsNullOrWhiteSpace(involvedUser.Email))
        {
          _logger.LogError($"{nameof(CreateMessageNotifications)}: No recipient email. Id: {involvedUser.Id}");
          continue;
        }

        if (involvedUser.Email.Equals(subTaskEvent.CreatedBy.Email, StringComparison.CurrentCultureIgnoreCase))
        {
          //Don't send notification about the message to the person who just made it.
          continue;
        }

        Notification notification = new Notification
        {
          NotificationType = notificationType,
          WorkRequest = workRequest,
          SubTask = subTask,
          Recipient = involvedUser.Email,
          SentDate = DateTime.Now,
          CreatedBy = subTask.LastEditedBy
        };
        _notificationRepository.Insert(notification);
        _notificationRepository.Save();

        Notification emailNotification = new()
        {
          NotificationType = notificationType,
          WorkRequest = workRequest,
          SubTask = subTask,
          Recipient = involvedUser.Email,
          SentDate = DateTime.Now,
          CreatedBy = subTask.LastEditedBy,
          Message = HtmlHelper.RemoveImageElementsFromString(subTaskEvent.Content, true)
        };

        notificationsCreatedCount++;
        await SendNotificationEmailAsync(emailNotification, linkUrl);
      }

      return notificationsCreatedCount;
    }
    #endregion
  }
}
