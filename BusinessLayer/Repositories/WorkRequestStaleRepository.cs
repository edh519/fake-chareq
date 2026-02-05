using BusinessLayer.Repositories.Interfaces;
using DataAccessLayer;
using DataAccessLayer.Models;
using Enums.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Repositories;

public class WorkRequestStaleRepository : Repository<WorkRequestStaleRepository>, IWorkRequestStaleRepository
{
    public WorkRequestStaleRepository(ApplicationDbContext context) : base(context)
    {

    }

    public async Task<List<WorkRequest>> GetAllApprovedWorkRequests()
    {
        return await _context.WorkRequests
            .Include(s => s.Status)
            .Include(s => s.Trial)
            .Include(s => s.Assignees)
            .Include(s => s.WorkRequestEvents)
            .ThenInclude(wre => wre.EventType)
            .Where(s => s.Status.WorkRequestStatusId == WorkRequestStatusEnum.PendingCompletion)
            .ToListAsync();
    }

    public Task<List<WorkRequest>> GetAllRequestingChangesWorkRequests()
    {
        return _context.WorkRequests
            .Include(s => s.Status)
            .Include(s => s.Trial)
            .Include(s => s.Assignees)
            .Include(s => s.WorkRequestEvents)
            .Where(s => s.Status.WorkRequestStatusId == WorkRequestStatusEnum.PendingRequester)
            .ToListAsync();
    }
}
