using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DataAccessLayer.Models
{
    public class ApplicationUser : IdentityUser<string>
    {
        public ICollection<WorkRequest> WorkRequests { get; set; }
        public ICollection<WorkRequestSubscription> WorkRequestSubscriptions { get; set; }
        public bool isSystemAccount { get; set; }
    }
}
