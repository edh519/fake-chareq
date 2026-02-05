using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    public class WorkRequestEvent
    {
        /// <summary>
        /// PK
        /// </summary>
        public int WorkRequestEventId { get; set; }
        [ForeignKey("WorkRequest")]
        public int WorkRequestId { get; set; }
        /// <summary>
        /// Work request that this event belongs to.
        /// </summary>
        public WorkRequest WorkRequest { get; set; }
        /// <summary>
        /// Type of the event, used for display differences.
        /// </summary>
        public WorkRequestEventType EventType { get; set; }
        /// <summary>
        /// Number of days to signify task took, optional.
        /// </summary>
        public double? DurationDays { get; set; }
        /// <summary>
        /// The event itself in text form. 
        /// Often would be the reasoning text or message.
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// User that created this event.
        /// </summary>
        public ApplicationUser CreatedBy { get; set; }
        /// <summary>
        /// DateTime the event was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
