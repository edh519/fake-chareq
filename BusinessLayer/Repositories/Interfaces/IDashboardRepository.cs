using System;
using System.Collections.Generic;

namespace BusinessLayer.Repositories.Interfaces
{
    public interface IDashboardRepository
    {
        List<dynamic> GetWRsByTrial(string dateInput = "12m");
        List<dynamic> GetAllWRsByStatus(string dateInput = "12m");
        List<dynamic> GetDecisionAndCompletionTime(string dateInput = "12m");
        List<dynamic> GetAverageDecisionAndCompletionTime(string dateInput = "12m");
        List<dynamic> GetAssignmentGroupCounts(string dateInput = "12m");
        Dictionary<string, int> GetCurrentAgeOfIncompleteTasks(string dateInput = "12m");
    }
}
