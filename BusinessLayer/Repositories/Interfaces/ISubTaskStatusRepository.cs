using DataAccessLayer.Models;
using Enums.Enums;
using System.Collections.Generic;

namespace BusinessLayer.Repositories.Interfaces
{
    public interface ISubTaskStatusRepository : IRepository<SubTaskStatus>
    {
        IEnumerable<SubTaskStatus> GetPendingSubTaskStatuses();
        IEnumerable<SubTaskStatus> GetFinalisedSubTaskStatuses();
        IEnumerable<SubTaskStatus> GetAllSubTaskStatuses();
    }
}
