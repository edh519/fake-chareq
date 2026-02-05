using System.Collections.Generic;
using DataAccessLayer.Models;

namespace BusinessLayer.ViewModels.Subscriptions;

public class UserSubscriptionUpdateModel
{
    public List<WorkRequest> UpdatedWorkRequests { get; set; }
    public string SystemUrl { get; set; }
    public string ViewWorkRequestUrl { get; set; }
}