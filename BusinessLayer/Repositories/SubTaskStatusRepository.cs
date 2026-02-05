using BusinessLayer.Repositories.Interfaces;
using DataAccessLayer;
using DataAccessLayer.Models;
using Enums.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.Repositories
{
    public class SubTaskStatusRepository : Repository<SubTaskStatus>, ISubTaskStatusRepository
    {
        public SubTaskStatusRepository(ApplicationDbContext context) : base(context)
        {

        }
        /// <summary>
        /// SubTaskStatusNames which indicate that a sub task is not pending.
        /// E.g. Completed, Abandoned.
        /// </summary>
        private readonly SubTaskStatusEnum[] FinalisedStatuses = new[] { SubTaskStatusEnum.Approved, SubTaskStatusEnum.Abandoned, SubTaskStatusEnum.Rejected };

        public IEnumerable<SubTaskStatus> GetPendingSubTaskStatuses()
        {
            // NB: Negated "Contains()" conditional to get non-finalised (pending) sub task statuses.
            return _context.SubTaskStatuses
                    .Where(q => q.IsActive && !FinalisedStatuses.Contains(q.SubTaskStatusId));
        }

        public IEnumerable<SubTaskStatus> GetFinalisedSubTaskStatuses()
        {
            return _context.SubTaskStatuses
                    .Where(q => q.IsActive && FinalisedStatuses.Contains(q.SubTaskStatusId));
        }

        public IEnumerable<SubTaskStatus> GetAllSubTaskStatuses()
        {
            return _context.SubTaskStatuses;
        }
    }
}
