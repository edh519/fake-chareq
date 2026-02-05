using BusinessLayer.ViewModels;
using DataAccessLayer.Models;
using Octokit;
using System.Collections.Generic;

namespace BusinessLayer.Services.Models.DTOs
{
    public class WorkRequestExportDto
    {
        public WorkRequest WorkRequest { get; set; }
        public List<SubTask> SubTasks { get; set; }
        public Issue? GHIssue { get; set; }
        public List<DataExportDiscussionViewModel> DiscussionVM { get; set; }

    }

}
