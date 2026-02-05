using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ViewModels
{
    public class DataExportDiscussionViewModel
    {
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string Content { get; set; }
    }
}
