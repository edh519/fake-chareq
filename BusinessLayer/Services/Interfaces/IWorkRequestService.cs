using BusinessLayer.ViewModels;
using DataAccessLayer.Models;
using Enums.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interfaces
{
  public interface IWorkRequestService
  {
    Task<WorkRequestDetailsViewModel> GetWorkRequestById(int workRequestId, string userEmail);
    EditWorkRequestViewModel GetEditWorkRequestViewModelFromWorkRequest(WorkRequest workRequest);
    bool UpdateWorkRequest(WorkRequest updateWorkRequest);
    List<UserSimpleViewModel> GetAssignedUsers(WorkRequest workRequest);
    List<UserSimpleViewModel> GetAuthorisers(bool isActiveOnly = true);
    IEnumerable<WorkRequestEventTypeEnum> GetAllowedWorkrequestEventTypes(WorkRequestStatusEnum workRequestStatusEnum);
    List<UserSimpleViewModel> GetAssignableUsers();
    Task<(bool Success, string ErrorMessage)> AssignUserToWorkRequestAsync(int workRequestId, string assigneeEmail, string executorEmail, string linkUrl, bool isAutoAssign);
    Task<(bool Success, string ErrorMessage)> UnassignUserToWorkRequestAsync(int workRequestId, string assigneeEmail, string executorEmail, bool isAutoUnassign);
    Task<(bool Success, string ErrorMessage)> AddLabelToWorkRequestAsync(int workRequestId, int labelId, string executorEmail);
    Task<(bool Success, string ErrorMessage)> RemoveLabelToWorkRequestAsync(int workRequestId, int labelId, string executorEmail);
  }

}
