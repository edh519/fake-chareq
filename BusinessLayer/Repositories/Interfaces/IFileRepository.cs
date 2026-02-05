using DataAccessLayer.Models;
using System.Collections.Generic;

namespace BusinessLayer.Repositories.Interfaces
{
    public interface IFileRepository : IRepository<FileUpload>
    {
        IEnumerable<FileUpload> GetFileUploadsByWorkRequestId(int workRequestId);
    }
}
