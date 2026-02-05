using BusinessLayer.Repositories.Interfaces;
using BusinessLayer.Services.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.Services
{
    public class ViewAllWorkRequestsService : IViewAllWorkRequestsService
    {
        private readonly IWorkRequestRepository _workRequestRepository;
        private readonly IWorkRequestStatusRepository _workRequestStatusRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ViewAllWorkRequestsService(IWorkRequestRepository workRequestRepository, IWorkRequestStatusRepository workRequestStatusRepository, UserManager<ApplicationUser> userManager)
        {
            _workRequestRepository = workRequestRepository;
            _workRequestStatusRepository = workRequestStatusRepository;
            _userManager = userManager;
        }

        /// <summary>
        /// Gets all work requests with a pending status type. Returns null if none are found.
        /// </summary>
        /// <returns>Returns all work requests with a pending status type. Returns null if none are found.</returns>
        public IEnumerable<WorkRequest> GetPendingWorkRequests()
        {
            IEnumerable<WorkRequestStatus> pendingStatuses = _workRequestStatusRepository.GetPendingWorkRequestStatuses();
            if (pendingStatuses == null || !pendingStatuses.Any())
                return null;

            IEnumerable<WorkRequest> pendingWorkRequests = _workRequestRepository.GetWorkRequestsByStatuses(pendingStatuses);
            if (pendingWorkRequests == null || !pendingWorkRequests.Any())
                return null;

            return pendingWorkRequests;
        }
        /// <summary>
        /// Gets all work requests with a pending status type and created by <paramref name="email"/>. Returns null if none are found.
        /// </summary>
        /// <returns>Returns all work requests with a pending status type and created by <paramref name="email"/>. Returns null if none are found.</returns>
        public IEnumerable<WorkRequest> GetPendingWorkRequestsByEmail(string email)
        {
            IEnumerable<WorkRequestStatus> pendingStatuses = _workRequestStatusRepository.GetPendingWorkRequestStatuses();
            if (pendingStatuses == null || !pendingStatuses.Any())
                return null;

            IEnumerable<WorkRequest> pendingWorkRequests = _workRequestRepository.GetWorkRequestsByStatusesAndEmail(pendingStatuses, email);
            if (pendingWorkRequests == null || !pendingWorkRequests.Any())
                return null;

            return pendingWorkRequests;
        }
        /// <summary>
        /// Gets all work requests with a finalised status type. Returns null if none are found.
        /// </summary>
        /// <returns>Returns all work requests with a finalised status type. Returns null if none are found.</returns>
        public IEnumerable<WorkRequest> GetFinalisedWorkRequests()
        {
            IEnumerable<WorkRequestStatus> finalisedStatuses = _workRequestStatusRepository.GetFinalisedWorkRequestStatuses();
            if (finalisedStatuses == null || !finalisedStatuses.Any())
                return null;

            IEnumerable<WorkRequest> finalisedWorkRequests = _workRequestRepository.GetWorkRequestsByStatuses(finalisedStatuses);
            if (finalisedWorkRequests == null || !finalisedWorkRequests.Any())
                return null;

            return finalisedWorkRequests;
        }
        /// <summary>
        /// Gets all work requests with a finalised status type and created by <paramref name="email"/>. Returns null if none are found.
        /// </summary>
        /// <returns>Returns all work requests with a finalised status type and created by <paramref name="email"/>. Returns null if none are found.</returns>
        public IEnumerable<WorkRequest> GetFinalisedWorkRequestsByEmail(string email)
        {
            IEnumerable<WorkRequestStatus> finalisedStatuses = _workRequestStatusRepository.GetFinalisedWorkRequestStatuses();
            if (finalisedStatuses == null || !finalisedStatuses.Any())
                return null;

            IEnumerable<WorkRequest> finalisedWorkRequests = _workRequestRepository.GetWorkRequestsByStatusesAndEmail(finalisedStatuses, email);
            if (finalisedWorkRequests == null || !finalisedWorkRequests.Any())
                return null;

            return finalisedWorkRequests;
        }
        /// <summary>
        /// Gets all work requests. Returns null if none are found.
        /// </summary>
        /// <returns>Returns all work requests. Returns null if none are found.</returns>
        public IEnumerable<WorkRequest> GetAllWorkRequests()
        {
            DateTime allTime = DateTime.MinValue;
            
            IEnumerable<WorkRequest> workRequests = _workRequestRepository.GetAllWorkRequestsCreatedAfterDate(allTime);
            if (workRequests == null || !workRequests.Any())
                return null;

            return workRequests;
        }

        /// <summary>
        /// Overload of GetAllWorkRequests()
        /// </summary>
        /// <param name="date">Date from which all work requests should be returned</param>
        /// <returns>Returns all work requests since 'date'. Returns null if none are found.</returns>
        public IEnumerable<WorkRequest> GetAllWorkRequests(DateTime date)
        {
            IEnumerable<WorkRequest> workRequests = _workRequestRepository.GetAllWorkRequestsCreatedAfterDate(date);
            if (workRequests == null || !workRequests.Any())
                return null;

            return workRequests;
        }

        /// <summary>
        /// Gets all work requests created by <paramref name="email"/>. Returns null if none are found.
        /// </summary>
        /// <returns>Returns all work requests created by <paramref name="email"/>. Returns null if none are found.</returns>
        public IEnumerable<WorkRequest> GetAllWorkRequestsByEmail(string email)
        {
            IEnumerable<WorkRequest> workRequests = _workRequestRepository.Get(filter: q => q.CreatedBy == email.ToUpper(), includeProperties: "Trial,Status");
            if (workRequests == null || !workRequests.Any())
                return null;

            return workRequests;
        }




        public IEnumerable<WorkRequest> GetAllParticipatingWorkRequests(ApplicationUser user, bool? isFinalised)
        {
            IEnumerable<WorkRequestStatus> workRequestStatuses;
            if (isFinalised.HasValue && !isFinalised.Value)
                workRequestStatuses = _workRequestStatusRepository.GetPendingWorkRequestStatuses();
            else if (isFinalised.HasValue && isFinalised.Value)
                workRequestStatuses = _workRequestStatusRepository.GetFinalisedWorkRequestStatuses();
            else
                workRequestStatuses = _workRequestStatusRepository.GetAllActiveWorkRequestStatuses();

            IEnumerable<WorkRequest> workRequests = _workRequestRepository.GetParticipatingWorkRequests(workRequestStatuses, user);
            if (workRequests == null || !workRequests.Any())
                return null;

            return workRequests;
        }

        public IEnumerable<WorkRequest> GetAllAssignedWorkRequests(ApplicationUser user, bool? isFinalised, string[] users = null)
        {
            IEnumerable<WorkRequestStatus> workRequestStatuses;
            if (isFinalised.HasValue && !isFinalised.Value)
                workRequestStatuses = _workRequestStatusRepository.GetPendingWorkRequestStatuses();
            else if (isFinalised.HasValue && isFinalised.Value)
                workRequestStatuses = _workRequestStatusRepository.GetFinalisedWorkRequestStatuses();
            else
                workRequestStatuses = _workRequestStatusRepository.GetAllActiveWorkRequestStatuses();

            IEnumerable<WorkRequest> workRequests;
            if (users != null && !users.All(x => string.IsNullOrWhiteSpace(x)))
            {
                List<ApplicationUser> usersFilter = new();
                foreach(string usersItem in users)
                {
                    ApplicationUser applicationUser = _userManager.FindByEmailAsync(usersItem + "@york.ac.uk").Result;
                    if (applicationUser != null && !usersFilter.Any(q => q.Id == applicationUser.Id))
                        usersFilter.Add(applicationUser);
                }
                workRequests = _workRequestRepository.GetAssignedWorkRequests(workRequestStatuses, usersFilter);
            }
            else
            {
                workRequests = _workRequestRepository.GetAssignedWorkRequests(workRequestStatuses, user);
            }

            
            if (workRequests == null || !workRequests.Any())
                return null;

            return workRequests;
        }

        public IEnumerable<WorkRequest> GetAllUnassignedWorkRequests(bool? isFinalised)
        {
            IEnumerable<WorkRequestStatus> workRequestStatuses;
            if (isFinalised.HasValue && !isFinalised.Value)
                workRequestStatuses = _workRequestStatusRepository.GetPendingWorkRequestStatuses();
            else if (isFinalised.HasValue && isFinalised.Value)
                workRequestStatuses = _workRequestStatusRepository.GetFinalisedWorkRequestStatuses();
            else
                workRequestStatuses = _workRequestStatusRepository.GetAllActiveWorkRequestStatuses();

            IEnumerable<WorkRequest> workRequests = _workRequestRepository.GetUnassignedWorkRequests(workRequestStatuses);
            if (workRequests == null || !workRequests.Any())
                return null;

            return workRequests;
        }
    }
}
