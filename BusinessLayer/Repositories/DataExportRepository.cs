using BusinessLayer.Repositories.Interfaces;
using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Repositories;

public class DataExportRepository : IDataExportRepository
{
    private readonly ApplicationDbContext _context;

    public DataExportRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddDataExportJobAsync(DataExportJob dataExport)
    {
        _context.DataExportJobs.Add(dataExport);
        await _context.SaveChangesAsync();
    }

    public async Task<DataExportJob> GetExportJobByIdAndUserIdAsync(int id, string userId)
    {
        return await _context.DataExportJobs
            .AsNoTracking()
            .Include(e => e.Trial)
            .FirstOrDefaultAsync(de => de.Id == id && de.RequestedById == userId);
    }

    public async Task<List<DataExportJob>> GetExportHistoryByUserIdAsync(string userId)
    {
        return await _context.DataExportJobs
            .AsNoTracking()
            .Include(de => de.Trial)
            .Where(de => de.RequestedById == userId)
            .OrderByDescending(de => de.RequestedAt)
            .ToListAsync();
    }

    public async Task<List<DataExportJob>> GetOldCompletedExportJobs(int days)
    {
        DateTime cutoff = DateTime.Now.AddDays(-days);
        return await _context.DataExportJobs
            .Where(j => j.StatusId == DataExportStatusEnum.Completed && j.CompletedAt < cutoff)
            .ToListAsync();
    }
}