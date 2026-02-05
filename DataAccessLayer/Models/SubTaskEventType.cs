using Enums.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class SubTaskEventType
    {
        public SubTaskEventTypeEnum SubTaskEventTypeId { get; set; }
        public string SubTaskEventTypeName { get; set; }
        public bool IsActive { get; set; }
    }
}
