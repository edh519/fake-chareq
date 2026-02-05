using BusinessLayer.Helpers;
using BusinessLayer.Repositories.Interfaces;
using BusinessLayer.Services.DataExport;
using BusinessLayer.Services.EmailProcessing;
using BusinessLayer.Services.EmailProcessing.EmailHelpers;
using BusinessLayer.Services.Interfaces;
using BusinessLayer.Services.Models.DTOs;
using BusinessLayer.ViewModels;
using DataAccessLayer;
using DataAccessLayer.External.Repos;
using DataAccessLayer.Models;
using Enums.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using QuestPDF.Fluent;
using YTU.EmailService;

namespace BusinessLayer.Services
{
    public class DataExportService : IDataExportService
    {
        private readonly IWorkRequestRepository _workRequestRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISubTaskRepository _subTaskRepository;
        private readonly GitHubApiRepository _gitHubApiRepository;
        private readonly ApplicationDbContext _context;
        private readonly EmailHandlerService _emailHandlerService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DataExportService(
            IWorkRequestRepository workRequestRepository,
            UserManager<ApplicationUser> userManager,
            ISubTaskRepository subTaskRepository,
            GitHubApiRepository gitHubApiRepository,
            ApplicationDbContext context,
            EmailHandlerService emailHandlerService, IWebHostEnvironment webHostEnvironment)
        {
            _workRequestRepository = workRequestRepository;
            _userManager = userManager;
            _subTaskRepository = subTaskRepository;
            _gitHubApiRepository = gitHubApiRepository;
            _context = context;
            _emailHandlerService = emailHandlerService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<byte[]> GenerateWorkRequestPdfAsync(int workRequestId, ApplicationUser exportedBy)
        {
            WorkRequestExportDto data = await GetExportDataAsync(workRequestId);

            WorkRequestPdfDocument model = new(data, exportedBy.Email);

            return model.GeneratePdf();
        }

        public async Task<WorkRequestExportDto> GetExportDataAsync(int workRequestId)
        {
            WorkRequest wr = _workRequestRepository.GetWorkRequest(workRequestId);
            List<SubTask> st = _subTaskRepository.GetAllSubTasksForWorkRequest(workRequestId).ToList();

            Issue ghIssue = null;

            if (wr.AssignedTrialRepositoryId != null)
            {
                ghIssue = await _gitHubApiRepository.GetIssueByIssueNumber(
                            wr.AssignedTrialRepositoryId.Value,
                            wr.GitHubIssueNumber.GetValueOrDefault());
            }


            return new WorkRequestExportDto
            {
                WorkRequest = wr,
                SubTasks = st,
                DiscussionVM = SetUpDiscussionVM(workRequestId, st),
                GHIssue = ghIssue
            };
        }

        public List<DataExportDiscussionViewModel> SetUpDiscussionVM(int workRequestId, List<SubTask> st)
        {
            WorkRequest workRequest = _workRequestRepository.GetWorkRequest(workRequestId);
            List<WorkRequestEvent> workRequestEvents = _workRequestRepository.GetWorkRequestEvents(workRequestId).ToList();
            List<DataExportDiscussionViewModel> discussionEvents = new();

            if (workRequestEvents is not null)
            {
                discussionEvents.AddRange(
                    workRequestEvents.Select(wre =>
                    {
                        string? content = null;
                        string wreContent = HtmlHelper.RemoveHtmlElementsFromString(wre.Content ?? "");

                        if (wreContent.Contains(" at "))
                        {
                            content = wreContent.Substring(0, wreContent.IndexOf(" at ")).Trim();
                        }
                        else
                        {
                            content = $"{CommonHelpers.RemoveDomainFromEmail(wre.CreatedBy.Email)} - {wre.EventType.WorkRequestEventTypeName.ToLower()} - '{wreContent}'";
                        }

                        return new DataExportDiscussionViewModel
                        {
                            CreatedAt = wre.CreatedAt,
                            CreatedBy = CommonHelpers.RemoveDomainFromEmail(wre.CreatedBy.Email),
                            Content = content
                        };
                    })
                );
            }

            if (st.Any())
            {
                discussionEvents.AddRange(
                    st.Select(st =>
                    {
                        string stContent = HtmlHelper.RemoveHtmlElementsFromString(st.SubTaskTitle);

                        string? subtaskContent = $"{CommonHelpers.RemoveDomainFromEmail(st.CreatedBy)} - created subtask - '{stContent}'";

                        return new DataExportDiscussionViewModel
                        {
                            CreatedAt = st.CreationDateTime,
                            CreatedBy = CommonHelpers.RemoveDomainFromEmail(st.CreatedBy),
                            Content = subtaskContent
                        };
                    })
                );
            }

            if (workRequest.ProcessDeviationReason is not null)
            {
                List<WorkRequestEventTypeEnum> closedWorkRequestEvents = new()
                {
                        WorkRequestEventTypeEnum.Complete,
                            WorkRequestEventTypeEnum.Closed,
                    };

                WorkRequestEvent closedEvent = workRequestEvents.Where(i => closedWorkRequestEvents.Contains(i.EventType.WorkRequestEventTypeId)).FirstOrDefault();

                ProcessDeviationReason pdr = workRequest.ProcessDeviationReason;
                string? pdrContent = $"{CommonHelpers.RemoveDomainFromEmail(closedEvent.CreatedBy.Email)} - process deviation reason - '{pdr.Reason}'";

                discussionEvents.Add(
                    new DataExportDiscussionViewModel
                    {
                        CreatedAt = closedEvent.CreatedAt,
                        CreatedBy = CommonHelpers.RemoveDomainFromEmail(closedEvent.CreatedBy.Email),
                        Content = pdrContent
                    });
            }

            return discussionEvents
                .OrderBy(e => e.CreatedAt)
                .ToList();
        }

        public async Task ProcessBulkExportQueue(string websiteUrl)
        {
            DataExportJob pendingExport = await _context.DataExportJobs
                .Include(de => de.Trial)
                .Include(e => e.Status)
                .OrderBy(x => x.RequestedAt)
                .FirstOrDefaultAsync(de => de.StatusId == DataExportStatusEnum.Queued);

            if (pendingExport is null)
            {
                return;
            }

            pendingExport.StatusId = DataExportStatusEnum.Processing;
            await _context.SaveChangesAsync();

            try
            {
                ApplicationUser user = await _userManager.FindByIdAsync(pendingExport.RequestedById);
                List<int> workRequestIds = await _workRequestRepository.GetWorkRequestIdsForTrialAbandonedOrClosed(pendingExport.TrialId);

                if (!workRequestIds.Any())
                {
                    pendingExport.StatusId = DataExportStatusEnum.Completed;
                    pendingExport.CompletedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    return;
                }

                using (MemoryStream memoryStream = new())
                {
                    using (ZipArchive archive = new(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (int workRequestId in workRequestIds)
                        {
                            byte[] pdfFile = await GenerateExport(workRequestId, user.UserName);
                            string fileName = GetExportName(workRequestId, pendingExport.Trial.TrialName);

                            ZipArchiveEntry zipEntry = archive.CreateEntry(fileName, CompressionLevel.Fastest);
                            using (Stream zipStream = zipEntry.Open())
                            {
                                await zipStream.WriteAsync(pdfFile, 0, pdfFile.Length);
                            }
                        }
                    }

                    string exportDir = ExportPathHelper.GetExportDirectory(_webHostEnvironment.EnvironmentName);

                    Directory.CreateDirectory(exportDir);

                    string uniqueFileName = $"{Guid.NewGuid()}.zip";
                    string filePath = Path.Combine(exportDir, uniqueFileName);

                    await File.WriteAllBytesAsync(filePath, memoryStream.ToArray());

                    pendingExport.FilePath = uniqueFileName;
                }

                pendingExport.StatusId = DataExportStatusEnum.Completed;
                pendingExport.CompletedAt = DateTime.UtcNow;

                await SendBulkExportCompleteEmailAsync(user.Email, pendingExport.Trial.TrialName, pendingExport.Id, websiteUrl);
                pendingExport.NotificationSent = true;
            }
            catch (Exception)
            {
                pendingExport.StatusId = DataExportStatusEnum.Failed;
                throw;
            }
            finally
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task<byte[]> GenerateExport(int workRequestId, string userName)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(userName);

            LogExport(workRequestId, user);

            byte[] pdfFile = await GenerateWorkRequestPdfAsync((int)workRequestId, user);

            return pdfFile;
        }

        public string GetExportName(int workRequestId, string trialName)
        {
            return $"ChaReq-Export_{trialName}_WR{workRequestId}_{FileHelpers.GetTimeStamp()}.pdf";
        }

        private void LogExport(int workRequestId, ApplicationUser user)
        {
            WorkRequestEvent workRequestEvent = new()
            {
                WorkRequestId = workRequestId,
                Content = $"{CommonHelpers.RemoveDomainFromEmail(user.Email)} - exported - work request #{workRequestId} at {DateTime.Now:HH:mm}",
                CreatedAt = DateTime.Now,
                CreatedBy = user,
                EventType = _workRequestRepository.GetWorkRequestEventType(WorkRequestEventTypeEnum.Export)
            };
            _workRequestRepository.InsertWorkRequestEvent(workRequestEvent);
            _workRequestRepository.Save();
        }

        private async Task SendBulkExportCompleteEmailAsync(string userEmail, string trialName, int exportId, string websiteUrl)
        {
            string downloadUrl = $"{websiteUrl}/DataExport/TriggerDownload/{exportId}";

            string subject = $"Bulk Export Complete for {trialName}";
            BulkExportCompleteEmailViewModel viewModel = new()
            {
                TrialName = trialName,
                DownloadUrl = downloadUrl
            };

            RazorToHtmlParser parser = new();

            string emailBody = await parser.RenderHtmlStringAsync("BulkExportCompleteEmail", viewModel);


            Email email = new()
            {
                Subject = subject,
                Body = emailBody,
                ToAddresses = userEmail
            };

            await _emailHandlerService.SendEmailAsync(email);
        }
    }
}
