using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class SubTaskEvent
    {
        /// <summary>
        /// PK
        /// </summary>
        public int SubTaskEventId { get; set; }
        /// <summary>
        /// Work request that this event belongs to.
        /// </summary>
        public SubTask SubTask { get; set; }
        /// <summary>
        /// Type of the event, used for display differences.
        /// </summary>
        public SubTaskEventType EventType { get; set; }
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
