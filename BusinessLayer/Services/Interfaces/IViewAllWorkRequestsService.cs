using DataAccessLayer.Models;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Services.Interfaces
{
    public interface IViewAllWorkRequestsService
    {
        IEnumerable<WorkRequest> GetPendingWorkRequests();
        IEnumerable<WorkRequest> GetPendingWorkRequestsByEmail(string email);
        IEnumerable<WorkRequest> GetFinalisedWorkRequests();
        IEnumerable<WorkRequest> GetFinalisedWorkRequestsByEmail(string email);
        IEnumerable<WorkRequest> GetAllWorkRequests();
        IEnumerable<WorkRequest> GetAllWorkRequests(DateTime date);
        IEnumerable<WorkRequest> GetAllWorkRequestsByEmail(string email);
        IEnumerable<WorkRequest> GetAllParticipatingWorkRequests(ApplicationUser user, bool? isFinalised);
        IEnumerable<WorkRequest> GetAllAssignedWorkRequests(ApplicationUser user, bool? isFinalised, string[] group = null);
        IEnumerable<WorkRequest> GetAllUnassignedWorkRequests(bool? isFinalised);
    }
}
