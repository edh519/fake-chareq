using BusinessLayer.Repositories.Interfaces;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.Services
{
    public class ViewAllSubTasksService : IViewAllSubTasksService
    {
        private readonly ISubTaskRepository _subTaskRepository;

        public ViewAllSubTasksService(ISubTaskRepository subTaskRepository)
        {
            _subTaskRepository = subTaskRepository;
        }

        public IEnumerable<SubTask> GetAllSubTasks()
        {
            DateTime allTime = DateTime.MinValue;

            IEnumerable<SubTask> subTasks = _subTaskRepository.GetAllSubTasksCreatedAfterDate(allTime);
            if (subTasks == null || !subTasks.Any())
                return null;

            return subTasks;
        }

        public IEnumerable<SubTask> GetAllSubTasks(DateTime date)
        {
            IEnumerable<SubTask> subTasks = _subTaskRepository.GetAllSubTasksCreatedAfterDate(date);

            return subTasks;
        }
    }
}
