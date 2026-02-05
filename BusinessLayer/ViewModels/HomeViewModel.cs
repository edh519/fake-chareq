using System.Collections.Generic;

namespace BusinessLayer.ViewModels
{
    public class HomeViewModel
    {
        public List<dynamic> TotalWRsPerTrial { get; set; }
        public List<dynamic> AllWRsByStatus { get; set; }
        public List<dynamic> AverageDecisionsAndCompletedTime { get; set; }
        public List<dynamic> DecisionsAndCompletedTime { get; set; }
        public List<dynamic> WREventsGroupAssignmentSplit { get; set;}
        public Dictionary<string, int> AgeOfIncompleteTasks { get; set; }
    }
}
