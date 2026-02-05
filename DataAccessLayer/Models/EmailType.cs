using Enums.Enums;

namespace DataAccessLayer.Models
{
    public class EmailType
    {
        public EmailTypeEnum EmailTypeId { get; set; }
        public string EmailTypeName { get; set; }
        public bool IsActive { get; set; }
    }
}
