using Enums.Enums;

namespace DataAccessLayer.Models
{
    public class NotificationType
    {
        public NotificationTypeEnum NotificationTypeId { get; set; }
        public string NotificationTypeName { get; set; }
        public bool IsActive { get; set; }
    }
}
