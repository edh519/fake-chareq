using DataAccessLayer.Models;
using System;
using System.Collections.Generic;

namespace BusinessLayer.ViewModels
{
    public class ConversationViewModel
    {
        public DateTime CreatedAt { get; set; }
        public WorkRequestEventViewModel WorkRequestEvent { get; set; }
        public SubTaskAccordionViewModel SubTasks { get; set; }
    }
}
