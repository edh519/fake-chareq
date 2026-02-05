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
    public class SubTaskRepository : Repository<SubTask>, ISubTaskRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IWorkRequestRepository _workRequestRepository;

        public SubTaskRepository(ApplicationDbContext context, IWorkRequestRepository workRequestRepository) : base(context)
        {
            _context = context;
            _workRequestRepository = workRequestRepository;
        }


        public SubTask? GetSubTask(int? subTaskId)
        {
            if (subTaskId == null || subTaskId == 0)
            {
                return null;
            }
            return _context.SubTasks
                .Include(i => i.Status)
                .Include(i => i.Assignee)
                .FirstOrDefault(s => s.SubTaskId == subTaskId);
        }

        public IEnumerable<SubTask> GetAllSubTasksForWorkRequest(int workRequestId)
        {
            return _context.SubTasks
                .Include(i => i.Status)
                .Include(i => i.Assignee)
                .Include(i => i.SubTaskEvents).ThenInclude(i => i.EventType)
                .Where(q => q.WorkRequestId == workRequestId)
                .ToList();
        }

        public IEnumerable<SubTask> GetAllSubTasksCreatedAfterDate(DateTime date)
        {
            return _context.SubTasks
                .Include(i => i.Status)
                .Include(i => i.Assignee)
                .Include(i => i.SubTaskEvents)
                .ThenInclude(i => i.EventType)
                .AsSingleQuery()
                .Where(q => q.CreationDateTime > date)
                .AsEnumerable();
        }

        public List<ApplicationUser> GetAllUsersInvolvedInSubTask(int subTaskId)
        {
            SubTask subTask = GetSubTask(subTaskId);

            List<ApplicationUser> involvedUsers = new List<ApplicationUser>();

            IEnumerable<SubTaskEvent> subTaskEvents = GetSubTaskEvents(subTaskId);

            foreach (SubTaskEvent subTaskEvent in subTaskEvents)
            {
                involvedUsers.Add(subTaskEvent.CreatedBy);
            }

            involvedUsers.Add(subTask.Assignee);

            return involvedUsers.Distinct().ToList();
        }

        #region Events

        public IEnumerable<SubTaskEvent> GetSubTaskEvents(int subTaskId)
        {
            return _context.SubTaskEvents
                .Include(i => i.EventType)
                .Include(i => i.CreatedBy)
                .Include(i => i.SubTask)
                .Where(q => q.SubTask.SubTaskId == subTaskId)
                .AsSingleQuery()?.ToList();
        }

        public IEnumerable<SubTaskEvent> GetAllSubTaskEventsForWorkRequest(int workRequestId)
        {
            return _context.SubTaskEvents
                .Include(i => i.EventType)
                .Include(i => i.CreatedBy)
                .Include(i => i.SubTask)
                .Where(q => q.SubTask.WorkRequestId == workRequestId)
                .AsSingleQuery()?.ToList();
        }

        public SubTaskEventType GetSubTaskEventsType(SubTaskEventTypeEnum subTaskEventTypeEnum)
        {
            return _context.SubTaskEventTypes
                .Where(q => q.SubTaskEventTypeId == subTaskEventTypeEnum)
                .AsSingleQuery().SingleOrDefault();
        }

        public int InsertSubTaskEvent(SubTaskEvent subTaskEvent)
        {
            _context.SubTaskEvents.Add(subTaskEvent);
            _context.SaveChanges();
            return subTaskEvent.SubTaskEventId;
        }

        #endregion



        public string? GetTrialForSubTask(int? subTaskId)
        {
            SubTask? subTask = GetSubTask(subTaskId);
            if (subTask == null)
            {
                return null;
            }

            WorkRequest? workRequest = _workRequestRepository.GetWorkRequestBySubTask(subTaskId);

            Trial? trial = workRequest?.Trial;

            // If the trial is the "Other" one (ID 1000), include the TrialOther text.
            if (trial?.TrialId == 1000)
            {
                return workRequest.TrialOther;
            }

            return trial.TrialName;
        }

    }
}
