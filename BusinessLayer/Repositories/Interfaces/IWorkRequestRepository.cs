using DataAccessLayer.Models;
using Enums.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Repositories.Interfaces;

public interface IWorkRequestRepository : IRepository<WorkRequest>
{
    IEnumerable<FileUpload> GetFileUploadsByWorkRequestId(int? workRequestId);
    WorkRequest GetWorkRequest(int workRequestId);
    IEnumerable<WorkRequest> GetAllWorkRequestsCreatedAfterDate(DateTime date);
    IEnumerable<WorkRequest> GetWorkRequestsByStatuses(IEnumerable<WorkRequestStatus> workRequestStatuses);
    IEnumerable<WorkRequest> GetWorkRequestsByStatusesAndEmail(IEnumerable<WorkRequestStatus> workRequestStatuses, string email);
    IEnumerable<WorkRequest> GetAssignedWorkRequests(IEnumerable<WorkRequestStatus> workRequestStatuses, ApplicationUser user);
    IEnumerable<WorkRequest> GetAssignedWorkRequests(IEnumerable<WorkRequestStatus> workRequestStatuses, List<ApplicationUser> users);
    IEnumerable<WorkRequest> GetUnassignedWorkRequests(IEnumerable<WorkRequestStatus> workRequestStatuses);
    IEnumerable<WorkRequest> GetParticipatingWorkRequests(IEnumerable<WorkRequestStatus> workRequestStatuses, ApplicationUser user);
    int InsertProcessDeviationReason(ProcessDeviationReason processDeviationReason, WorkRequest workRequest);
    IEnumerable<Label> GetLabelsByWorkRequestId(int workRequestId);
    IEnumerable<ApplicationUser> GetAssigneesByWorkRequestId(int workRequestId);
    IEnumerable<WorkRequestEvent> GetWorkRequestEvents(int workRequestId);
    WorkRequestEventType GetWorkRequestEventType(WorkRequestEventTypeEnum workRequestEventTypeEnum);
    int InsertWorkRequestEvent(WorkRequestEvent workRequestEvent);
    Task<bool> WorkRequestHasGitHubIssue(int workRequestId);
    WorkRequest? GetWorkRequestBySubTask(int? subTaskId);
    List<WorkRequest>? GetAllOpenWRsAssignedToDevelopers();
    Task<List<int>> GetWorkRequestIdsForTrialAbandonedOrClosed(int trialId);
}