using DataAccessLayer.Models;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interfaces
{
  public interface INotificationService
  {
    #region Create new

    Task<int> CreateInitialSubmissionNotificationsAsync(int workRequestId, string linkUrl);
    Task<int> CreateInitialApprovalNotificationsAsync(int workRequestId, string linkUrl, string message);
    Task<int> CreateCompletedNotificationsAsync(int workRequestId, string linkUrl, string message);
    Task<int> CreateAbandonedNotificationsAsync(int workRequestId, string linkUrl, string message);
    Task<int> CreateRejectedWithAmendmentsNotificationsAsync(int workRequestId, string linkUrl, string message);
    Task<int> CreateMessageNotifications(int workRequestId, string linkUrl, WorkRequestEvent workRequestEvent);
    #endregion

    #region Sub tasks
    Task<int> CreateNewSubTaskNotificationAsync(int subTaskId, string linkUrl);
    Task<int> CreateAssignedToSubTaskNotificationsAsync(int subTaskId, string linkUrl);
    Task<int> CreateApprovedNotificationsForSubTaskAsync(int subTaskId, string linkUrl, string message);
    Task<int> CreateRejectionNotificationsForSubTaskAsync(int subTaskId, string linkUrl, string message);
    Task<int> CreateAbandonedNotificationsForSubTasksAsync(int subTaskId, string linkUrl, string message);
    Task<int> CreateMessageNotificationsForSubTasks(int subTaskId, string linkUrl, SubTaskEvent subTaskEvent);
    #endregion


    #region Update existing

    #endregion

    #region Get existing
    Task<int> CreateAssignedToRequestNotifications(int workRequestId, string assigneeEmail, bool autoAssign, bool isAssigned, string linkUrl);

    #endregion
  }
}
