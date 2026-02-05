using BusinessLayer.Repositories;
using BusinessLayer.Repositories.Interfaces;
using BusinessLayer.Services;
using DataAccessLayer.External.Repos;
using DataAccessLayer.Models;
using Enums.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]

    public class PullRequestController : Controller
    {
        private readonly GitHubApiRepository _gitHubRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWorkRequestRepository _workRequestRepository;
        private readonly IWorkRequestStatusRepository _workRequestStatusRepository;
        private readonly PullRequestRepository _pullRequestRepository;
        private readonly PullRequestService _pullRequestService;
        private readonly ApiTokenConfig _apiTokenConfig;
        private readonly ILogger<PullRequestController> _logger;
        private readonly ILabelRepository _labelRepository;
        public PullRequestController(GitHubApiRepository githubRepository, UserManager<ApplicationUser> userManager, IWorkRequestRepository workRequestRepository, IWorkRequestStatusRepository workRequestStatusRepository, PullRequestRepository pullRequestRepository, PullRequestService pullRequestService, IOptions<ApiTokenConfig> apiTokenConfig, ILogger<PullRequestController> logger, ILabelRepository labelRepository)
        {
            _gitHubRepository = githubRepository;
            _userManager = userManager;
            _workRequestRepository = workRequestRepository;
            _workRequestStatusRepository = workRequestStatusRepository;
            _pullRequestRepository = pullRequestRepository;
            _pullRequestService = pullRequestService;
            _apiTokenConfig = apiTokenConfig.Value;
            _logger = logger;
            _labelRepository = labelRepository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reviews"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("PullRequestDetails")]
        public async Task<IActionResult> GetPullRequestDetailsAsync([FromHeader] string apiToken, [FromBody] Root json)
        {
            if (string.IsNullOrEmpty(apiToken) || json == null || json.Reviews == null || string.IsNullOrEmpty(json.PR_Body))
            {
                return BadRequest("Invalid request data.");
            }
            if (apiToken != _apiTokenConfig.ChaReqApiToken)
            {
                return BadRequest("ApiToken incorrect.");
            }

            List<Review> listOfReviews = json.Reviews;
            if (listOfReviews == null || !listOfReviews.Any())
            {
                return Ok("No reviews found.");
            }
            // Find the approved review
            Review approvedReview = listOfReviews
                .Where(r => r.state == "APPROVED")
                .OrderByDescending(r => r.submitted_at)
                .FirstOrDefault();
            if (approvedReview == null)
            {
                return Ok("No approved reviews found.");
            }

            // Get the email associated with the approver
            string email = await _pullRequestRepository.GetEmailFromGitHubLogin(approvedReview.user.login);

            List<WorkRequest> workRequests = new List<WorkRequest>();
            List<int> linkedIssuesNumber = _pullRequestService.ExtractLinkedIssues(json.PR_Body);
            foreach (int linkedIssue in linkedIssuesNumber)
            {
                try
                {
                    Issue? issue = await _gitHubRepository.GetIssueByIssueNumber(json.RepositoryId, linkedIssue);

                    if (issue != null)
                    {
                        // Fetch associated work requests
                        List<WorkRequest>? issueWorkRequests = _pullRequestRepository.GetWRAssociatedWithIssue(json.RepositoryId, issue);

                        if (issueWorkRequests != null)
                        {
                            workRequests.AddRange(issueWorkRequests);
                        }
                    }
                }
                catch (NotFoundException ex)
                {
                    // Log the issue that couldn't be found
                    _logger.LogError($"Issue not found for ID {linkedIssue}.");
                }
            }
            if (workRequests.Count() == 0)
            {
                return Ok("No work requests found.");
            }

            // Extract the ChaReq approval message
            string chaReqMessage = _pullRequestService.ExtractChaReqApprovalMessage(approvedReview);
            if (chaReqMessage == string.Empty)
            {
                return Ok("Approval message required.");
            }

            // Mark associated work requests as completed
            await MarkWorkRequestsAsCompleted(workRequests, chaReqMessage, email);

            return Ok("Action completed");
        }

        [HttpPut]
        [Route("WorkRequest/MarkAsComplete")]
        public async Task<IActionResult> MarkWorkRequestsAsCompleted(List<WorkRequest> workRequests, string completionMessage, string email)
        {
            foreach (WorkRequest workRequest in workRequests)
            {
                if (workRequest == null)
                {
                    return Ok($"WorkRequest with ID {workRequest.WorkRequestId} not found.");
                }
                if (workRequest.Status.WorkRequestStatusId != WorkRequestStatusEnum.PendingCompletion)
                {
                    return Ok($"WorkRequest with ID {workRequest.WorkRequestId} is not pending completion.");
                }

                // Remove 'Awaiting Review' label to request if not already
                int labelId = _labelRepository.GetLabels().FirstOrDefault(l => string.Equals(l.LabelShort, "Awaiting Review", StringComparison.OrdinalIgnoreCase)).LabelId;
                IActionResult githubLabelResult = this.RemoveLabelFromWorkRequest(workRequest.WorkRequestId, labelId);

                // Update the status to 'Complete'
                workRequest.Status = _workRequestStatusRepository.Get(q => q.WorkRequestStatusId == WorkRequestStatusEnum.Completed).Single();
                workRequest.LastEditedDateTime = DateTime.Now;
                workRequest.LastEditedBy = email;

                ApplicationUser user = await _userManager.FindByEmailAsync(email);

                // Add a completion event
                WorkRequestEvent workRequestEvent = new WorkRequestEvent
                {
                    WorkRequest = workRequest,
                    Content = completionMessage,
                    CreatedAt = DateTime.Now,
                    CreatedBy = user,
                    EventType = _workRequestRepository.GetWorkRequestEventType(WorkRequestEventTypeEnum.Complete)
                };

                _workRequestRepository.InsertWorkRequestEvent(workRequestEvent);
                _workRequestRepository.Update(workRequest);
                await _workRequestRepository.SaveAsync();
            }
            return Ok(new { Message = "Action complete." });
        }
        private IActionResult RemoveLabelFromWorkRequest(int workRequestId, int labelId)
        {
            // Perform initial dumb checks.
            if (workRequestId <= 0)
                return BadRequest("Invalid workRequestId");
            if (labelId <= 0)
                return BadRequest("Invalid labelId");

            // Get work request and labels and check they are allowed.
            WorkRequest workRequest = _workRequestRepository.GetWorkRequest(workRequestId);
            if (workRequest == null)
                return BadRequest("Invalid workRequestId");

            IEnumerable<DataAccessLayer.Models.Label> allLabels = _labelRepository.GetUnarchivedLabels();
            if (allLabels != null && !allLabels.Any(x => x.LabelId == labelId))
                return BadRequest("Invalid labelId");

            // Label does not exist on the work request.
            if (workRequest.Labels == null || !workRequest.Labels.Any(x => x.LabelId == labelId))
                return Ok("Label not found on work request");

            // Select label and remove from work request.
            DataAccessLayer.Models.Label labelToRemove = workRequest.Labels.Single(x => x.LabelId == labelId);
            workRequest.Labels.Remove(labelToRemove);

            _workRequestRepository.Save();

            return Ok("Label removed successfully");
        }
    }
}

