using BusinessLayer.Repositories.Interfaces;
using DataAccessLayer;
using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.Repositories
{
    public class FileRepository : Repository<FileUpload>, IFileRepository
    {
        public FileRepository(ApplicationDbContext context) : base(context)
        { }

        public IEnumerable<FileUpload> GetFileUploadsByWorkRequestId(int workRequestId)
        {
            return _context.FileUploads
                .Where(q => q.WorkRequestId == workRequestId)
                .ToList();
        }
    }
}
