using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ViewModels
{
    public class ViewAllSubTasksIndexViewModel
    {
        public IEnumerable<SubTaskStatus> Statuses { get; set; }
        public IEnumerable<Trial> Trials { get; set; }
        public string CurrentUser { get; set; }
    }
}
