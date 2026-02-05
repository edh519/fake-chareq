using Enums.Enums;

namespace DataAccessLayer.Models
{
    public class WorkRequestStatus
    {
        public WorkRequestStatusEnum WorkRequestStatusId { get; set; }
        public string WorkRequestStatusName { get; set; }
        public bool IsActive { get; set; }
    }
}
