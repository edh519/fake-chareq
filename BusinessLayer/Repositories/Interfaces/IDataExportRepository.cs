using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Repositories.Interfaces;

public interface IDataExportRepository
{
    Task<List<DataExportJob>> GetExportHistoryByUserIdAsync(string userId);
    Task AddDataExportJobAsync(DataExportJob dataExport);
    Task<DataExportJob> GetExportJobByIdAndUserIdAsync(int id, string userId);
    Task<List<DataExportJob>> GetOldCompletedExportJobs(int days);
}