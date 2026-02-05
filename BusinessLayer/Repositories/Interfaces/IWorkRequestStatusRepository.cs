using DataAccessLayer.Models;
using System.Collections.Generic;

namespace BusinessLayer.Repositories.Interfaces
{
    public interface IWorkRequestStatusRepository : IRepository<WorkRequestStatus>
    {
        IEnumerable<WorkRequestStatus> GetPendingWorkRequestStatuses();
        IEnumerable<WorkRequestStatus> GetFinalisedWorkRequestStatuses();
        IEnumerable<WorkRequestStatus> GetAllActiveWorkRequestStatuses();
        IEnumerable<WorkRequestStatus> GetAllWorkRequestStatuses();
    }
}
