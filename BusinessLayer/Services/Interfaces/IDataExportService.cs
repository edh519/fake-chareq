using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Services.Models.DTOs;
using BusinessLayer.ViewModels;

namespace BusinessLayer.Services.Interfaces;

public interface IDataExportService
{
    Task<byte[]> GenerateWorkRequestPdfAsync(int workRequestId, ApplicationUser exportedBy);
    Task<WorkRequestExportDto> GetExportDataAsync(int workRequestId);
    List<DataExportDiscussionViewModel> SetUpDiscussionVM(int workRequestId, List<SubTask> st);
    Task ProcessBulkExportQueue(string websiteUrl);
    Task<byte[]> GenerateExport(int workRequestId, string userName);
    string GetExportName(int workRequestId, string trialName);
}