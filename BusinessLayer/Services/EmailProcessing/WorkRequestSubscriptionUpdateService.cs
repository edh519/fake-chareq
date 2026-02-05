using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Repositories.Interfaces;
using BusinessLayer.Services.EmailProcessing.EmailHelpers;
using BusinessLayer.ViewModels.Subscriptions;
using DataAccessLayer.Models;
using Microsoft.Extensions.Logging;
using YTU.EmailService;

namespace BusinessLayer.Services.EmailProcessing
{
    public class WorkRequestSubscriptionUpdateService
    {
        private readonly IWorkRequestSubscriptionRepository _workRequestSubscriptionRepository;
        private readonly EmailHandlerService _emailHandlerService;
        private readonly ILogger<WorkRequestSubscriptionUpdateService> _logger;
        public WorkRequestSubscriptionUpdateService(IWorkRequestSubscriptionRepository workRequestSubscriptionRepository, EmailHandlerService emailHandlerService, ILogger<WorkRequestSubscriptionUpdateService> logger)
        {
            _workRequestSubscriptionRepository = workRequestSubscriptionRepository;
            _emailHandlerService = emailHandlerService;
            _logger = logger;
        }

        public async Task RunWorkRequestSubscriptionUpdate(string linkToCurrentSystem, string viewWorkRequestUrl)
        {
            List<WorkRequestSubscription> allWorkRequestSubscriptions = await _workRequestSubscriptionRepository.GetAllWorkRequestSubscriptions();

            var userWorkRequestSubscriptionGrouping = allWorkRequestSubscriptions.GroupBy(
                    wrs => wrs.ApplicationUser.Email,
                    wrs => wrs.WorkRequest,
                    (key, wrk) => new
                    {
                        UserEmail = key,
                        SubscribedRequests = wrk.ToList()
                    })
                .ToList();

            foreach (var userSubscriptionGrouping in userWorkRequestSubscriptionGrouping)
            {

                List<WorkRequest> updatedWorkRequests = userSubscriptionGrouping.SubscribedRequests.Where(sr =>
                    // Only send updates for new events created within the last 24hrs
                   sr.WorkRequestEvents.Any(e => e.CreatedAt >= DateTime.Now.AddDays(-1) &&
                                                 e.CreatedAt <= DateTime.Now)).ToList();
                
                if(!updatedWorkRequests.Any()) continue;
                    
                // Send update
                RazorToHtmlParser razorParser = new();

                UserSubscriptionUpdateModel model = new()
                {
                    UpdatedWorkRequests = updatedWorkRequests,
                    SystemUrl = linkToCurrentSystem,
                    ViewWorkRequestUrl = viewWorkRequestUrl
                };

                string notificationEmail = await razorParser.RenderHtmlStringAsync("UserSubscriptionUpdate", model);

                Email email = new()
                {
                    Subject = "Subscriptions Update",
                    Body = notificationEmail,
                    CustomFooter = "",
                    RemoveDevEmailBody = false,
                    ToAddresses = userSubscriptionGrouping.UserEmail

                };

                await _emailHandlerService.SendEmailAsync(email);
            }
        }
    }
}