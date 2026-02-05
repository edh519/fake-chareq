using Enums.Enums;

namespace DataAccessLayer.Models
{
    public class WorkRequestEventType
    {
        public WorkRequestEventTypeEnum WorkRequestEventTypeId { get; set; }
        public string WorkRequestEventTypeName { get; set; }
        public bool IsActive { get; set; }
    }
}
