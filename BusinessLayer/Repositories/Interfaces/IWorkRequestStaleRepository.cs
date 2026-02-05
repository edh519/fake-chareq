using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Repositories.Interfaces
{
    public interface IWorkRequestStaleRepository
    {
        Task<List<WorkRequest>> GetAllApprovedWorkRequests();
        Task<List<WorkRequest>> GetAllRequestingChangesWorkRequests();
    }
}