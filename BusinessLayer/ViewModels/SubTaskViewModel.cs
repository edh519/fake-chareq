using DataAccessLayer.Models;
using System.Collections.Generic;

namespace BusinessLayer.ViewModels
{
    public class SubTaskViewModel
    {
        public int WorkRequestId { get; set; }
        public int WorkRequestStatus { get; set; }
        public List<SubTaskAccordionViewModel> SubTasks { get; set; } 
        public bool IsLoggedInUserOfRoleTypeUser { get; set; }
    }
}
