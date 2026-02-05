using Enums.Enums.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Enums.Enums
{
    public enum WorkRequestEventTypeEnum
    {
        /// <summary>
        /// Default message.
        /// </summary>
        [Display(Name = "Message"), IsActive(true)]
        None = 1,
        /// <summary>
        /// Requesting changes
        /// </summary>
        [Display(Name = "Request Changes"), IsActive(true)]
        Enquiry = 2,
        /// <summary>
        /// Approving a request
        /// </summary>
        [Display(Name = "Approve"), IsActive(true)]
        Approve = 3,
        /// <summary>
        /// Completing a request
        /// </summary>
        [Display(Name = "Close as Completed"), IsActive(true)]
        Complete = 4,
        /// <summary>
        /// Closing a request - As not complete
        /// </summary>
        [Display(Name = "Close as Not Planned"), IsActive(true)]
        Closed = 5,
        /// <summary>
        /// Change of assignment - assigned or unassigned
        /// </summary>
        [Display(Name = "Assignment"), IsActive(false)]
        Assignment = 10,
        /// <summary>
        /// Change of labels - adding or removing
        /// </summary>
        [Display(Name = "Label"), IsActive(false)]
        Label = 11,
        /// <summary>
        /// Change of GitHub issue attachment - adding or removing
        /// </summary>
        [Display(Name = "GitHub Issue Attachment"), IsActive(false)]
        GHIssueAttachment = 12,
        /// <summary>
        /// Exporting work request
        /// </summary>
        [Display(Name = "Export work request"), IsActive(false)]
        Export = 13,
        /// <summary>
        /// Subscribing to a work request
        /// </summary>
        [Display(Name = "Subscribe"), IsActive(false)]
        Subscription = 24,
        /// <summary>
        /// Activity of attached files
        /// </summary>
        [Display(Name = "File Management"), IsActive(false)]
        FileManagement = 20
    }
}
