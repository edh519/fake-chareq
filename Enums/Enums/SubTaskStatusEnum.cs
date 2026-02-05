using Enums.Enums.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Enums.Enums
{
    public enum SubTaskStatusEnum
    {
        /// <summary>
        /// Open
        /// </summary>
        [Display(Name = "Open"), IsActive(true)]
        Open = 510,
        /// <summary>
        /// Approved
        /// </summary>
        [Display(Name = "Approved"), IsActive(true)]
        Approved = 520,
        /// <summary>
        /// Rejected
        /// </summary>
        [Display(Name = "Rejected"), IsActive(true)]
        Rejected = 600,
        /// <summary>
        /// Closed as not planned
        /// </summary>
        [Display(Name = "Closed as not planned"), IsActive(true)]
        Abandoned = 610
    }
}
