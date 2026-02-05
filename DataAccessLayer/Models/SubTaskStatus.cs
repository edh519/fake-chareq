using Enums.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class SubTaskStatus
    {
        public SubTaskStatusEnum SubTaskStatusId { get; set; }
        public string SubTaskStatusName { get; set; }
        public bool IsActive { get; set; }
    }
}
