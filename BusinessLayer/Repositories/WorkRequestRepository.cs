using BusinessLayer.Repositories.Interfaces;
using DataAccessLayer;
using DataAccessLayer.Models;
using Enums.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Label = DataAccessLayer.Models.Label;

namespace BusinessLayer.Repositories
{
    public class WorkRequestRepository : Repository<WorkRequest>, IWorkRequestRepository
    {
        public WorkRequestRepository(ApplicationDbContext context) : base(context)
        { }

        #region Get Authorisations

        public IEnumerable<WorkRequestEvent> GetWorkRequestEvents(int workRequestId)
        {
            return _context.WorkRequestEvents
                .Include(i => i.EventType)
                .Include(i => i.CreatedBy)
                .Include(i => i.WorkRequest)
                .Where(q => q.WorkRequest.WorkRequestId == workRequestId)
                .AsSingleQuery()?.ToList();
        }

        public WorkRequestEventType GetWorkRequestEventType(WorkRequestEventTypeEnum workRequestEventTypeEnum)
        {
            return _context.WorkRequestEventTypes
                .Where(q => q.WorkRequestEventTypeId == workRequestEventTypeEnum)
                .AsSingleQuery().SingleOrDefault();
        }

        public int InsertWorkRequestEvent(WorkRequestEvent workRequestEvent)
        {
            _context.WorkRequestEvents.Add(workRequestEvent);
            _context.SaveChanges();
            return workRequestEvent.WorkRequestEventId;
        }

        #endregion


        public int InsertProcessDeviationReason(ProcessDeviationReason processDeviationReason, WorkRequest workRequest)
        {
            _context.ProcessDeviationReasons.Add(processDeviationReason);
            _context.SaveChanges();
            workRequest.ProcessDeviationReason = processDeviationReason;
            _context.Update(workRequest);
            _context.SaveChanges();

            return processDeviationReason.ProcessDeviationReasonId;
        }

        public WorkRequest GetWorkRequest(int workRequestId)
        {
            return _context.WorkRequests
                .Include(i => i.Status)
                .Include(i => i.Trial)
                .Include(i => i.SupportingFiles)
                .Include(i => i.Labels)
                .Include(i => i.Assignees)
                .Include(i => i.WorkRequestEvents).ThenInclude(i => i.EventType)
                .Include(i => i.ProcessDeviationReason)
                .Include(i => i.SubTasks).ThenInclude(i => i.Status)
                .AsSingleQuery()
                .Where(q => q.WorkRequestId == workRequestId)
                .SingleOrDefault();
        }

        public IEnumerable<WorkRequest> GetTrialForWorkRequest(int trialId)
        {
            return _context.WorkRequests
                .Include(i => i.Trial)
                .AsSingleQuery()
                .Where(q => q.Trial.TrialId == trialId);

        }

        public IEnumerable<FileUpload> GetFileUploadsByWorkRequestId(int? workRequestId)
        {
            return _context.FileUploads
                .Where(x => x.WorkRequestId == workRequestId);
        }

        /// <summary>
        /// Gets all the work requests posted after the supplied date. If not supplied, default to 1 year ago.
        /// </summary>
        /// <param name="date">Date from which to include work requests. Defaults to 1 year ago.</param>
        /// <returns>Returns all the work requests without authorisations.</returns>
        public IEnumerable<WorkRequest> GetAllWorkRequestsCreatedAfterDate(DateTime date)
        {
            return _context.WorkRequests
                .Include(i => i.Status)
                .Include(i => i.Trial)
                .Include(i => i.Labels)
                .Include(i => i.Assignees)
                .Include(i => i.WorkRequestEvents).ThenInclude(i => i.EventType)
                .AsSingleQuery()
                .Where(q => q.CreationDateTime > date)
                .AsEnumerable();
        }

        /// <summary>
        /// Gets all the work requests with a status matching one in <paramref name="workRequestStatuses"/>.
        /// </summary>
        /// <param name="workRequestStatuses">IEnumerable of work request statuses to match against.</param>
        /// <returns>Returns all the work requests with a status matching one in <paramref name="workRequestStatuses"/>.</returns>
        public IEnumerable<WorkRequest> GetWorkRequestsByStatuses(IEnumerable<WorkRequestStatus> workRequestStatuses)
        {
            return _context.WorkRequests
                .Include(i => i.Status)
                .Include(i => i.Trial)
                .Include(i => i.Labels)
                .Include(i => i.Assignees)
                .Include(i => i.WorkRequestEvents).ThenInclude(i => i.EventType)
                .AsSingleQuery()
                .Where(q => workRequestStatuses.Any(x => x.WorkRequestStatusId == q.Status.WorkRequestStatusId)).AsEnumerable();
        }
        /// <summary>
        /// Gets all the work requests with a status matching one in <paramref name="workRequestStatuses"/> and also created by <paramref name="email"/>.
        /// </summary>
        /// <param name="workRequestStatuses">IEnumerable of work request statuses to match against.</param>
        /// <param name="email">Email address of user to match against CreatedBy field.</param>
        /// <returns>Returns all the work requests with a status matching one in <paramref name="workRequestStatuses"/> and that was created by the user <paramref name="email"/>.</returns>
        public IEnumerable<WorkRequest> GetWorkRequestsByStatusesAndEmail(IEnumerable<WorkRequestStatus> workRequestStatuses, string email)
        {
            return _context.WorkRequests
                .Include(i => i.Status)
                .Include(i => i.Trial)
                .Include(i => i.Labels)
                .Include(i => i.Assignees)
                .Include(i => i.WorkRequestEvents).ThenInclude(i => i.EventType)
                .AsSingleQuery()
                .Where(q => workRequestStatuses.Any(x => x.WorkRequestStatusId == q.Status.WorkRequestStatusId) && q.CreatedBy.ToUpper() == email.ToUpper()).AsEnumerable();
        }

        /// <summary>
        /// Gets all the labels assigned to a work request by its Id.
        /// </summary>
        /// <param name="workRequestId">Id of the work request to query against.</param>
        /// <returns></returns>
        public IEnumerable<Label> GetLabelsByWorkRequestId(int workRequestId)
        {
            return _context.WorkRequests
                .Include(i => i.Labels)
                .AsSingleQuery()
                .Single(q => q.WorkRequestId == workRequestId)
                .Labels.AsEnumerable();
        }

        /// <summary>
        /// Gets all the assignees assigned to a work request by its Id.
        /// </summary>
        /// <param name="workRequestId">Id of the work request to query against.</param>
        /// <returns></returns>
        public IEnumerable<ApplicationUser> GetAssigneesByWorkRequestId(int workRequestId)
        {
            return _context.WorkRequests
                .Include(i => i.Assignees)
                .AsSingleQuery()
                .Single(q => q.WorkRequestId == workRequestId)
                .Assignees.AsEnumerable();
        }

        public IEnumerable<WorkRequest> GetAssignedWorkRequests(IEnumerable<WorkRequestStatus> workRequestStatuses, ApplicationUser user)
        {

            return _context.ApplicationUsers
                .Include(i => i.WorkRequests).ThenInclude(i => i.Assignees)
                .Include(i => i.WorkRequests).ThenInclude(i => i.Status)
                .Include(i => i.WorkRequests).ThenInclude(i => i.Trial)
                .Include(i => i.WorkRequests).ThenInclude(i => i.Labels)
                .Include(i => i.WorkRequests).ThenInclude(i => i.WorkRequestEvents).ThenInclude(i => i.EventType)
                .AsSingleQuery()
                .Single(q => q.Id == user.Id)
                .WorkRequests
                .Where(q => workRequestStatuses.Any(x => x.WorkRequestStatusId == q.Status.WorkRequestStatusId))
                .AsEnumerable();
        }
        public IEnumerable<WorkRequest> GetAssignedWorkRequests(IEnumerable<WorkRequestStatus> workRequestStatuses, List<ApplicationUser> users)
        {
            IEnumerable<WorkRequest> workRequests = null;
            foreach (ApplicationUser user in users)
            {
                if (workRequests == null)
                {
                    workRequests = GetAssignedWorkRequests(workRequestStatuses, user);
                }
                else
                {
                    workRequests = workRequests.UnionBy(GetAssignedWorkRequests(workRequestStatuses, user), x => x.WorkRequestId); // Union rather than Concat to prevent duplicates (i.e. unique work requests only)
                }
            }
            return workRequests.AsEnumerable();
        }
        public IEnumerable<WorkRequest> GetUnassignedWorkRequests(IEnumerable<WorkRequestStatus> workRequestStatuses)
        {

            return _context.WorkRequests
                 .Include(i => i.Status)
                 .Include(i => i.Trial)
                 .Include(i => i.Labels)
                 .Include(i => i.Assignees)
                 .Include(i => i.WorkRequestEvents).ThenInclude(i => i.EventType)
                 .AsSingleQuery()
                 .Where(q => workRequestStatuses.Any(x => x.WorkRequestStatusId == q.Status.WorkRequestStatusId))
                 .Where(q => q.Assignees.Count == 0).AsEnumerable();
        }

        public IEnumerable<WorkRequest> GetParticipatingWorkRequests(IEnumerable<WorkRequestStatus> workRequestStatuses, ApplicationUser user)
        {
            return _context.WorkRequests
                    .Include(i => i.WorkRequestEvents)
                    .Include(i => i.Status)
                    .Include(i => i.Trial)
                    .Include(i => i.Labels)
                    .Include(i => i.Assignees)
                    .Include(i => i.WorkRequestEvents).ThenInclude(i => i.EventType)
                    .AsSingleQuery()
                    .Where(q => workRequestStatuses.Any(x => x.WorkRequestStatusId == q.Status.WorkRequestStatusId))
                    .Where(q => q.Assignees.Any(q => q.NormalizedEmail == user.NormalizedEmail)
                            || q.CreatedBy.ToUpper() == user.NormalizedEmail
                            || q.WorkRequestEvents.Any(x => x.CreatedBy.NormalizedEmail == user.NormalizedEmail)
                        )
                    .AsEnumerable();
        }
        /// <summary>
        /// Returns true if the work request has a GitHub issue associated with it, false otherwise. 
        /// </summary>
        /// <param name="workRequestId"></param>
        /// <returns></returns>
        public Task<bool> WorkRequestHasGitHubIssue(int workRequestId)
        {
            return _context.WorkRequests.Where(e =>
                    e.WorkRequestId == workRequestId && e.AssignedTrialRepositoryId.HasValue &&
                    e.GitHubIssueNumber.HasValue)
                .AsNoTracking()
                .AnyAsync();
        }

        public WorkRequest? GetWorkRequestBySubTask(int? subTaskId)
        {
            if (subTaskId == null || subTaskId == 0)
            {
                return null;
            }

            int? workRequestId = _context.SubTasks
                .Where(st => st.SubTaskId == subTaskId)
                .Select(st => st.WorkRequestId)
                .FirstOrDefault();

            if (workRequestId == null || workRequestId == 0)
            {
                return null;
            }

            return _context.WorkRequests
                    .Include(i => i.WorkRequestEvents)
                    .Include(i => i.Status)
                    .Include(i => i.Trial)
                    .Include(i => i.Labels)
                    .Include(i => i.Assignees)
                    .Include(i => i.WorkRequestEvents).ThenInclude(i => i.EventType)
                    .AsSingleQuery()
                .FirstOrDefault(wr => wr.WorkRequestId == workRequestId);
        }

        public List<WorkRequest>? GetAllOpenWRsAssignedToDevelopers()
        {
            List<WorkRequestStatusEnum> openStatuses =
            [
              WorkRequestStatusEnum.PendingRequester,
        WorkRequestStatusEnum.PendingCompletion,
        WorkRequestStatusEnum.PendingInitialApproval
          ];

            return _context.WorkRequests
                .Include(i => i.Assignees)
                .Include(i => i.Status)
                .Where(x =>
                    x.Assignees.Any(a => a.NormalizedEmail == "ytu-developers-group@york.ac.uk")
                    && openStatuses.Contains(x.Status.WorkRequestStatusId)
                )
                .ToList();
        }

        public async Task<List<int>> GetWorkRequestIdsForTrialAbandonedOrClosed(int trialId)
        {
            return await _context.WorkRequests
                .Where(q => q.Trial.TrialId == trialId &&
                            (q.Status.WorkRequestStatusId == WorkRequestStatusEnum.Completed ||
                             q.Status.WorkRequestStatusId == WorkRequestStatusEnum.Abandoned))
                .Select(e => e.WorkRequestId)
                .ToListAsync();
        }
    }
}
