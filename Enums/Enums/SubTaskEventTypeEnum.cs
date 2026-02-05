using Enums.Enums.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Enums.Enums
{
    public enum SubTaskEventTypeEnum
    {
        /// <summary>
        /// Default message.
        /// </summary>
        [Display(Name = "Message"), IsActive(true)]
        None = 1,
        /// <summary>
        /// Approving a sub task
        /// </summary>
        [Display(Name = "Approve"), IsActive(true)]
        Approve = 2,
        /// <summary>
        /// Rejecting a sub task
        /// </summary>
        [Display(Name = "Reject"), IsActive(true)]
        Reject = 3,
        /// <summary>
        /// Closing a request - As not complete
        /// </summary>
        [Display(Name = "Closed as Not Planned"), IsActive(true)]
        Abandon = 4,
        /// <summary>
        /// Changing the assigned user
        /// </summary>
        [Display(Name = "Assignment"), IsActive(true)]
        Assignment = 5,
    }
}

