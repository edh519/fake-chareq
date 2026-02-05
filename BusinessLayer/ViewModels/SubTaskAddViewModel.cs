using BusinessLayer.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.ViewModels
{
    public class SubTaskAddViewModel
    {
        public int SubTaskId { get; set; }
        public int WorkRequestId { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [StringLength(100, ErrorMessage = "Description must be 100 characters or fewer")]
        public string SubTaskTitle { get; set; }
        [Required(ErrorMessage = "Assigned user is required")]
        public string AssignedUserEmail { get; set; }
        public List<UserSimpleViewModel> AssignableUsers { get; set; }
    }
}
