using Enums.Enums;
using System;

namespace BusinessLayer.ViewModels
{
    public class WorkRequestEventViewModel
    {
        public WorkRequestEventViewModel() { }


        public int? WorkRequestEventId { get; set; }
        public int WorkRequestId { get; set; }
        public WorkRequestEventTypeEnum WorkRequestEventType { get; set; }
        public string Content { get; set; }
        public double? DurationDays { get; set; }
        public string CreatedByEmail { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
