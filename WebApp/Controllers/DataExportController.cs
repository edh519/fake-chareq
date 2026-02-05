using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Helpers;
using BusinessLayer.Repositories.Interfaces;
using BusinessLayer.ViewModels;
using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers;

public class DataExportController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITrialRepository _trialRepository;
    private readonly IDataExportRepository _dataExportRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public DataExportController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ITrialRepository trialRepository, IDataExportRepository dataExportRepository, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _userManager = userManager;
        _trialRepository = trialRepository;
        _dataExportRepository = dataExportRepository;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        string userId = _userManager.GetUserId(User);
        List<DataExportJob> exportHistory = await _dataExportRepository.GetExportHistoryByUserIdAsync(userId);

        DataExportViewModel viewModel = new()
        {
            Trials = _trialRepository.GetTrialsWithClosedOrAbandonedWorkRequests()
                .Select(t => new SelectListItem { Value = t.TrialId.ToString(), Text = t.TrialName })
                .ToList(),

            ExportHistory = exportHistory
                .Select(de => new DataExportJobViewModel
                {
                    Id = de.Id,
                    TrialName = de.Trial.TrialName,
                    RequestedAt = de.RequestedAt,
                    StatusEnum = de.StatusId,
                    CompletedAt = de.CompletedAt,
                    CanDownload = de.StatusId == DataExportStatusEnum.Completed && ExportPathHelper.DataExportFileExists(_webHostEnvironment.EnvironmentName,de.FilePath)
                })
                .ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(DataExportViewModel model)
    {
        if (model.SelectedTrialId == 0)
        {
            ModelState.AddModelError(nameof(model.SelectedTrialId), "Please select a trial.");
        }

        if (!ModelState.IsValid)
        {
            model.Trials = _trialRepository.GetTrialsWithClosedOrAbandonedWorkRequests()
                .Select(t => new SelectListItem { Value = t.TrialId.ToString(), Text = t.TrialName })
                .ToList();

            List<DataExportJob> exportHistory = await _dataExportRepository.GetExportHistoryByUserIdAsync(_userManager.GetUserId(User));

            model.ExportHistory = exportHistory
                .Select(de => new DataExportJobViewModel
                {
                    Id = de.Id,
                    TrialName = de.Trial.TrialName,
                    RequestedAt = de.RequestedAt,
                    StatusEnum = de.StatusId,
                    CompletedAt = de.CompletedAt,
                    CanDownload = de.StatusId == DataExportStatusEnum.Completed && ExportPathHelper.DataExportFileExists(_webHostEnvironment.EnvironmentName, de.FilePath)
                })
                .ToList();
            return View(model);
        }

        DataExportJob dataExport = new()
        {
            TrialId = model.SelectedTrialId,
            RequestedById = _userManager.GetUserId(User),
            RequestedAt = DateTime.Now,
            StatusId = DataExportStatusEnum.Queued
        };

        await _dataExportRepository.AddDataExportJobAsync(dataExport);

        TempData["Message"] = "Bulk export has been successfully requested.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult TriggerDownload(int? id)
    {
        if (id is null)
        {
            return BadRequest();
        }

        TempData["DownloadExportId"] = id;
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Download(int? id)
    {
        if (id is null)
        {
            return BadRequest();
        }
        string userId = _userManager.GetUserId(User);

        DataExportJob export = await _context.DataExportJobs
            .AsNoTracking()
            .Include(e => e.Trial)
            .FirstOrDefaultAsync(de => de.Id == id && de.RequestedById == userId);

        if (export == null || export.StatusId != DataExportStatusEnum.Completed || string.IsNullOrEmpty(export.FilePath))
        {
            return NotFound();
        }

        string baseDir = ExportPathHelper.GetExportDirectory(_webHostEnvironment.EnvironmentName);

        string filePath = Path.Combine(baseDir, export.FilePath);
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        MemoryStream memory = new();
        await using (FileStream stream = new(filePath, FileMode.Open))
        {
            await stream.CopyToAsync(memory);
        }
        memory.Position = 0;

        string downloadFileName = $"{export.Trial.TrialName.Replace(" ", "_")}_Export_{export.CompletedAt:yyyyMMdd}.zip";
        return File(memory, "application/zip", downloadFileName);
    }
}