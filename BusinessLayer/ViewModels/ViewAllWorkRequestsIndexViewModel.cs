using DataAccessLayer.Models;
using System.Collections.Generic;

namespace BusinessLayer.ViewModels
{
    public class ViewAllWorkRequestsIndexViewModel
    {
        public IEnumerable<WorkRequestStatus> Statuses { get; set; }
        public IEnumerable<Trial> Trials { get; set; }
        public IEnumerable<Label> Labels { get; set; }
        public string CurrentUser { get; set; }
    }
}
