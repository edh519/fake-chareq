using System.Collections.Generic;

namespace BusinessLayer.ViewModels
{
    public class AssigneesViewModel
    {
        public int WorkRequestId { get; set; }
        /// <summary>
        /// Users assigned to the request
        /// </summary>
        public List<UserSimpleViewModel> AssignedUsers { get; set; }
        /// <summary>
        /// All users that are assignable to a request, may already be assigned to this request.
        /// </summary>
        public List<UserSimpleViewModel> AssignableUsers { get; set; }
        public bool IsLoggedInUserOfRoleTypeUser { get; set; }
    }
}
