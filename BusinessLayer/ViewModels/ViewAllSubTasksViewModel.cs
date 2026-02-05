using BusinessLayer.Helpers;
using BusinessLayer.Repositories.Interfaces;
using DataAccessLayer.Models;
using Enums.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

public class ViewAllSubTasksViewModel
{
    private readonly ISubTaskRepository _subTaskRepository;

    public ViewAllSubTasksViewModel(IEnumerable<SubTask> subTasks, ISubTaskRepository subTaskRepository)
    {
        _subTaskRepository = subTaskRepository;

        List<SubTaskViewModel> subTaskViewModels = new();

        if (subTasks != null)
        {
            foreach (SubTask subTask in subTasks.ToList())
            {
                subTaskViewModels.Add(new SubTaskViewModel(subTask, _subTaskRepository));
            }
        }

        SubTasks = subTaskViewModels;
    }

    public IEnumerable<SubTaskViewModel> SubTasks { get; set; }

    public class SubTaskViewModel
    {
        public SubTaskViewModel(SubTask subTask, ISubTaskRepository subTaskRepository)
        {
            SubTaskId = subTask.SubTaskId;
            SubTaskTitle = subTask.SubTaskTitle;
            Status = subTask.Status?.SubTaskStatusName;
            Progress = CommonHelpers.ConvertSubTaskStatusToProgress(subTask.Status);

            Trial = subTaskRepository.GetTrialForSubTask(subTask.SubTaskId);

            WorkRequestId = subTask.WorkRequestId;
            Assignee = CommonHelpers.RemoveDomainFromEmail(subTask.Assignee.Email);
            Requester = CommonHelpers.RemoveDomainFromEmail(subTask.CreatedBy);
            CreatedDateTime = subTask.CreationDateTime;

            if (subTask.Status.SubTaskStatusId != SubTaskStatusEnum.Open)
            {
                CompletedDateTime = subTask?.SubTaskEvents
                    .FirstOrDefault(x => x.EventType.SubTaskEventTypeId == SubTaskEventTypeEnum.Reject ||
                                         x.EventType.SubTaskEventTypeId == SubTaskEventTypeEnum.Approve ||
                                         x.EventType.SubTaskEventTypeId == SubTaskEventTypeEnum.Abandon)?.CreatedAt;
            }
        }

        public int SubTaskId { get; set; }
        public string SubTaskTitle { get; set; }
        public string Status { get; set; }
        public double Progress { get; set; }
        public string Trial { get; set; }
        public int WorkRequestId { get; set; }
        public string Assignee { get; set; }
        public string Requester { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? CompletedDateTime { get; set; }
    }
}
