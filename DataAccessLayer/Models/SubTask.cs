using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class SubTask
    {
        public int SubTaskId { get; set; }
        public string SubTaskTitle { get; set; }
        public int WorkRequestId { get; set; }
        public SubTaskStatus Status { get; set; }
        public ApplicationUser Assignee { get; set; }
        public string CreatedBy { get; set; }
        public string LastEditedBy { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime LastEditedDateTime { get; set; }
        public List<SubTaskEvent> SubTaskEvents { get; set; }

    }
}
