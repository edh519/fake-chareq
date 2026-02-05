using Enums.Enums.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Enums.Enums
{
    public enum WorkRequestStatusEnum
    {
        /// <summary>
        /// Pending Requester
        /// </summary>
        [Display(Name = "Requested Changes"), IsActive(true)]
        PendingRequester = 10,
        /// <summary>
        /// Pending Initial Approval
        /// </summary>
        [Display(Name = "New"), IsActive(true)]
        PendingInitialApproval = 20,
        /// <summary>
        /// Pending Work/Completion
        /// </summary>
        [Display(Name = "In Progress"), IsActive(true)]
        PendingCompletion = 30,
        /// <summary>
        /// Completed
        /// </summary>
        [Display(Name = "Completed"), IsActive(true)]
        Completed = 100,
        /// <summary>
        /// Abandoned
        /// </summary>
        [Display(Name = "Closed"), IsActive(true)]
        Abandoned = 110
    }
}
