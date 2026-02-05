using DataAccessLayer.Models;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Repositories.Interfaces
{
    public interface IViewAllSubTasksService
    {
        IEnumerable<SubTask> GetAllSubTasks();
        IEnumerable<SubTask> GetAllSubTasks(DateTime date);
    }
}
