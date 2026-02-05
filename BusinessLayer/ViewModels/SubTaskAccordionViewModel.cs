using DataAccessLayer.Models;
using System.Collections.Generic;

namespace BusinessLayer.ViewModels
{
    public class SubTaskAccordionViewModel
    {
        public SubTask SubTask { get; set; }
        public List<SubTaskEventViewModel> SubTaskEvents { get; set; }
        public SubTaskEvent NewSubTaskEvent { get; set; } = new();
        public List<UserSimpleViewModel> AssignableUsers { get; set; }
        public bool IsLoggedInUserOfRoleTypeUser { get; set; }

    }
}
