using DataAccessLayer.Models;
using Enums.Enums;
using System;

namespace BusinessLayer.ViewModels
{
    public class SubTaskEventViewModel
    {
        public SubTaskEventViewModel() { }


        public int? SubTaskEventId { get; set; }
        public int SubTaskId { get; set; }
        public SubTaskEventTypeEnum SubTaskEventType { get; set; }
        public string Content { get; set; }
        public string CreatedByEmail { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
