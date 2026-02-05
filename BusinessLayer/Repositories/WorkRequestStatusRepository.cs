using BusinessLayer.Repositories.Interfaces;
using DataAccessLayer;
using DataAccessLayer.Models;
using Enums.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.Repositories
{
    public class WorkRequestStatusRepository : Repository<WorkRequestStatus>, IWorkRequestStatusRepository
    {
        public WorkRequestStatusRepository(ApplicationDbContext context) : base(context)
        {

        }


        /// <summary>
        /// WorkRequesStatusNames which indicate that a work request is not pending.
        /// E.g. Completed, Abandoned.
        /// </summary>
        private readonly WorkRequestStatusEnum[] FinalisedStatuses = new[] { WorkRequestStatusEnum.Completed, WorkRequestStatusEnum.Abandoned };

        /// <summary>
        /// Gets all active work request statuses which indicate that a work request is not finalised.
        /// </summary>
        /// <returns>Returns all active work request statuses which indicate that a work request is not finalised.</returns>
        public IEnumerable<WorkRequestStatus> GetPendingWorkRequestStatuses()
        {
            // NB: Negated "Contains()" conditional to get non-finalised (pending) work request statuses.
            return _context.WorkRequestStatuses
                    .Where(q => q.IsActive && !FinalisedStatuses.Contains(q.WorkRequestStatusId));
        }
        /// <summary>
        /// Gets all active work request statuses which indicate that a work request is finalised.
        /// </summary>
        /// <returns>Returns all active work request statuses which indicate that a work request is finalised.</returns>
        public IEnumerable<WorkRequestStatus> GetFinalisedWorkRequestStatuses()
        {
            return _context.WorkRequestStatuses
                    .Where(q => q.IsActive && FinalisedStatuses.Contains(q.WorkRequestStatusId));
        }
        /// <summary>
        /// Gets all active work request statuses. I.e. those that are in active use.
        /// </summary>
        /// <returns>Returns all active work request statuses.</returns>
        public IEnumerable<WorkRequestStatus> GetAllActiveWorkRequestStatuses()
        {
            return _context.WorkRequestStatuses.Where(q => q.IsActive);
        }

        public IEnumerable<WorkRequestStatus> GetAllWorkRequestStatuses()
        {
            return _context.WorkRequestStatuses;
        }

    }
}
