using BusinessLayer.Repositories.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Hosting;
using Quartz;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace WebApp.Jobs;

[DisallowConcurrentExecution]
public class CleanupBulkExportsJob : IJob
{
    private readonly IDataExportRepository _dataExportRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CleanupBulkExportsJob(IDataExportRepository dataExportRepository, IWebHostEnvironment webHostEnvironment)
    {
        _dataExportRepository = dataExportRepository;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        List<DataExportJob> oldExports = await _dataExportRepository.GetOldCompletedExportJobs(5);

        foreach (DataExportJob export in oldExports)
        {
            if (!string.IsNullOrEmpty(export.FilePath))
            {
                string exportDir = Path.Combine(_webHostEnvironment.ContentRootPath, "InternalStorage", "exports");
                string filePath = Path.Combine(exportDir, export.FilePath);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }
    }
}