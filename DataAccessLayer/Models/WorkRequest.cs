using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models
{
    public class WorkRequest
    {
        public int WorkRequestId { get; set; }

        #region Relationships

        public List<WorkRequestEvent> WorkRequestEvents { get; set; }

        public List<InitialAuthorisation> InitialAuthorisations { get; set; }

        public List<FinalAuthorisation> FinalAuthorisations { get; set; }
        #endregion

        public string Reference { get; set; }
        public WorkRequestStatus Status { get; set; }

        public Trial Trial { get; set; }
        public string TrialOther { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime LastEditedDateTime { get; set; }
        [DataType(DataType.MultilineText)]
        public string DetailDescription { get; set; }
        public List<FileUpload> SupportingFiles { get; set; }
        public string CreatedBy { get; set; }
        public string LastEditedBy { get; set; }

        public List<ApplicationUser> Assignees { get; set; }
        public List<SubTask> SubTasks { get; set; }
        public List<Label> Labels { get; set; }
        public int? GitHubIssueNumber { get; set; }
        public long? AssignedTrialRepositoryId { get; set; }
        public ProcessDeviationReason ProcessDeviationReason { get; set; }
    }
}
