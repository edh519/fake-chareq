using BusinessLayer.Repositories.Interfaces;
using BusinessLayer.Services.EmailProcessing.EmailHelpers;
using BusinessLayer.Services.Interfaces;
using BusinessLayer.ViewModels;
using DataAccessLayer.Models;
using Enums.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTU.EmailService;

namespace BusinessLayer.Services.EmailProcessing;

public class StaleRequestsService
{
    private readonly IWorkRequestStaleRepository _workRequestStaleRepository;
    private readonly EmailHandlerService _emailHandlerService;
    public StaleRequestsService(IWorkRequestStaleRepository workRequestStaleRepository, EmailHandlerService emailHandlerService)
    {
        _workRequestStaleRepository = workRequestStaleRepository;
        _emailHandlerService = emailHandlerService;
    }

    public async Task RunStaleRequestsService(string linkToCurrentSystem, string viewWorkRequestUrl)
    {
        IEnumerable<StaleWorkRequest> staleWorkRequests = await GetAllStaleRequests();

        List<string> usersToBeNotified = GetUsersToBeNotified(staleWorkRequests);

        foreach(string userEmail in usersToBeNotified)
        {
            List<StaleWorkRequest> workRequestsWithUser = staleWorkRequests.Where(swr => swr.InvolvedUsers.Contains(userEmail)).ToList();

            //Dummy Check - Since usersToBeNotified is pulled from staleWorkRequests it shouldn't be empty but just in case...
            if (!workRequestsWithUser.Any()) continue;

            RazorToHtmlParser razorParser = new();

            StaleWorkRequestEmailViewModel model = new()
            {
                StaleWorkRequests = workRequestsWithUser,
                SystemUrl = linkToCurrentSystem,
                ViewWorkRequestUrl = viewWorkRequestUrl
            };

            string notificationEmail = await razorParser.RenderHtmlStringAsync("StaleReminderSummaryEmail", model);

            Email email = new()
            {
                Subject = "Attention Required",
                Body = notificationEmail,
                CustomFooter = "",
                RemoveDevEmailBody = false,
                ToAddresses = userEmail
            };

            await _emailHandlerService.SendEmailAsync(email);
        }
    }

    private List<string> GetUsersToBeNotified(IEnumerable<StaleWorkRequest> staleWorkRequests)
    {
        List<string> usersToBeNotified = new();
        foreach(StaleWorkRequest request in staleWorkRequests)
        {
            usersToBeNotified.AddRange(request.InvolvedUsers);
        }

        return usersToBeNotified.Distinct().ToList();
    }

    private async Task<IEnumerable<StaleWorkRequest>> GetAllStaleRequests()
    {
        //Approved Requests are considered stale when there has been no event for two weeks
        List<WorkRequest> allApproved = await _workRequestStaleRepository.GetAllApprovedWorkRequests();
        DateTime olderThanTwoWeeks = DateTime.Now.AddDays(-14);

        List<WorkRequest> staleApproved = allApproved
            .Where(all => !all.WorkRequestEvents
                .Any(e => e.CreatedAt > olderThanTwoWeeks))
            .ToList();

        List<StaleWorkRequest> approvedStaleRequests = new();
        foreach (WorkRequest request in staleApproved)
        {
            //Approver and assignees should be notified of stale Approved Requests
            List<string> involvedUsers = new();
            foreach(WorkRequestEvent wrEvent in request.WorkRequestEvents)
            {
                if(wrEvent.EventType.WorkRequestEventTypeId == WorkRequestEventTypeEnum.Approve)
                {
                    //Dev enviroment has some fake users that do not have proper Appuser details
                    if(wrEvent.CreatedBy != null)
                        involvedUsers.Add(wrEvent.CreatedBy.Email);
                }
            }
            foreach(ApplicationUser assignee in request.Assignees)
            {
                involvedUsers.Add(assignee.Email);
            }

            StaleWorkRequest staleWorkRequest = new()
            {
                InvolvedUsers = involvedUsers,
                WorkRequest = request,
            };
            
            approvedStaleRequests.Add(staleWorkRequest);
        }

        //Requesting Changes requests are considered stale when there has been no event for three days
        List<WorkRequest> allRequesting = await _workRequestStaleRepository.GetAllRequestingChangesWorkRequests();
        DateTime olderThanThreeDays = DateTime.Now.AddDays(-3);

        List<WorkRequest> staleRequesting = allRequesting
            .Where(all => !all.WorkRequestEvents
                .Any(e => e.CreatedAt > olderThanThreeDays))
            .ToList();

        List<StaleWorkRequest> requestingStaleRequests = new();
        foreach(WorkRequest request in staleRequesting)
        {
            //Initial Requester and assignees should be notified of stale Requesting Changes Requests
            List<string> involvedUsers = new();
            involvedUsers.Add(request.CreatedBy);
            foreach (ApplicationUser assignee in request.Assignees)
            {
                involvedUsers.Add(assignee.Email);
            }

            StaleWorkRequest staleWorkRequest = new()
            {
                InvolvedUsers = involvedUsers,
                WorkRequest = request,
            };

            requestingStaleRequests.Add(staleWorkRequest);
        }

        return approvedStaleRequests.Concat(requestingStaleRequests);
    }
}
