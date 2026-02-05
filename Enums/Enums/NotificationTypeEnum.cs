using Enums.Enums.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Enums.Enums
{
    public enum NotificationTypeEnum
    {
        #region To Requester (1-99)

        /// <summary>
        /// Notification to Requester when request is approved.
        /// </summary>
        [Display(Name = "Work Request Approved"), IsActive(true)]
        WorkRequestApprovedToRequester = 1,
        /// <summary>
        /// Notification to Requester when request is declined with amendments.
        /// </summary>
        [Display(Name = "Request Requires Ammendments"), IsActive(true)]
        WorkRequestDeclinedWithAmendmentsToRequester = 2,
        /// <summary>
        /// Notification to Requester when request is declined and abandoned.
        /// </summary>
        [Display(Name = "Request Closed"), IsActive(true)]
        WorkRequestDeclinedAndAbandonedToRequester = 3,
        /// <summary>
        /// Notification to Requester when request is completed.
        /// </summary>
        [Display(Name = "Request Completed"), IsActive(true)]
        WorkRequestCompletedToRequester = 4,
        /// <summary>
        /// Notification to the requester when request is submitted initially.
        /// </summary>
        [Display(Name = "Work Request Received"), IsActive(true)]
        WorkRequestReceivedToRequester = 5,

        #endregion


        #region To Authoriser (100-199)

        /// <summary>
        /// Notification to authoriser when request is submitted initially.
        /// </summary>
        [Display(Name = "Request Pending Initial Approval"), IsActive(true)]
        WorkRequestPendingInitialApprovalToAuthoriser = 100,
        /// <summary>
        /// Notification to authoriser when request is re-submitted with changes.
        /// </summary>
        [Display(Name = "Request Re-submitted Pending Initial Approval"), IsActive(true)]
        WorkRequestPendingSubsequentApprovalToAuthoriser = 101,

        #endregion


        #region To Assignee (200-299)

        /// <summary>
        /// Notification to Assignee when request is approved and requires dev work.
        /// </summary>
        [Display(Name = "Request Pending Work"), IsActive(true)]
        WorkRequestPendingWorkToAssignee = 200,
        /// <summary>
        /// Notification to Assignee when assigned to a request
        /// </summary>
        [Display(Name = "Assigned to Request"), IsActive(true)]
        WorkRequestAssignedToAssignee = 201,
        /// <summary>
        /// Notification to Assignee when unassigned from a request
        /// </summary>
        [Display(Name = "Unassigned from Request"), IsActive(true)]
        WorkRequestUnassignedToAssignee = 202,

        #endregion

        #region Generic (300-399)
        
        /// <summary>
        /// Notification to Subscribers when a new message is posted
        /// </summary>
        [Display(Name = "New Message"), IsActive(true)]
        WorkRequestMessage = 300,

        #endregion

        #region SubTasks (500-599)

        /// <summary>
        /// Notification to creator when sub task is created
        /// </summary>
        [Display(Name = "Sub Task Created"), IsActive(true)]
        SubTaskCreation = 501,

        /// <summary>
        /// Notification to assignee when sub task is created
        /// </summary>
        [Display(Name = "Sub Task Assigned"), IsActive(true)]
        SubTaskAssignedToAssignee = 502,

        /// <summary>
        /// Notification to subscribers when a new message is posted on a sub task
        /// </summary>
        [Display(Name = "Sub Task - New Message"), IsActive(true)]
        SubTaskMessage = 503,

        /// <summary>
        /// Notification to subscribers when a sub task is approved
        /// </summary>
        [Display(Name = "Sub Task Approved"), IsActive(true)]
        SubTaskApproval = 504,

        /// <summary>
        /// Notification to subscribers when a sub task is rejected
        /// </summary>
        [Display(Name = "Sub Task Rejected"), IsActive(true)]
        SubTaskRejection = 505,

        /// <summary>
        /// Notification to subscribers when a sub task is closed as not planned
        /// </summary>
        [Display(Name = "Sub Task Closed As Not Planned"), IsActive(true)]
        SubTaskAbandoned = 506,
        #endregion
    }
}
