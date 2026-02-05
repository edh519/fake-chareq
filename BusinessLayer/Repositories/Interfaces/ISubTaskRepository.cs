using BusinessLayer.Repositories.Interfaces;
using DataAccessLayer.Models;
using Enums.Enums;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Repositories.Interfaces
{
    public interface ISubTaskRepository : IRepository<SubTask>
    {
        SubTask? GetSubTask(int? subTaskId);
        IEnumerable<SubTask> GetAllSubTasksForWorkRequest(int workRequestId);
        IEnumerable<SubTask> GetAllSubTasksCreatedAfterDate(DateTime date);
        List<ApplicationUser> GetAllUsersInvolvedInSubTask(int subTaskId);
        IEnumerable<SubTaskEvent> GetSubTaskEvents(int subTaskId);
        IEnumerable<SubTaskEvent> GetAllSubTaskEventsForWorkRequest(int workRequestId);
        SubTaskEventType GetSubTaskEventsType(SubTaskEventTypeEnum subTaskEventTypeEnum);
        int InsertSubTaskEvent(SubTaskEvent subTaskEvent);
        string? GetTrialForSubTask(int? subTaskId);
    }
}
