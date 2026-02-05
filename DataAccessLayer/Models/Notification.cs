using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public NotificationType NotificationType { get; set; }
        public WorkRequest WorkRequest { get; set; }
        public SubTask? SubTask { get; set; }
        public int? SubTaskId { get; set; }
        public DateTime SentDate { get; set; }
        /// <summary>
        /// Email address of user to notify.
        /// </summary>
        public string Recipient { get; set; }
        /// <summary>
        /// Comma separated list of bcc email recipients.
        /// </summary>
        public string BccAddresses { get; set; }
        /// <summary>
        /// Email address of user who triggered notification.
        /// </summary>
        public string CreatedBy { get; set; }
        public bool IsSeen { get => SeenDate.HasValue; }
        public DateTime? SeenDate { get; set; }
        public string Message { get; set; }
    }
}
