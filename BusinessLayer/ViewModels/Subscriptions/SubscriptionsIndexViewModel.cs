using System.Collections.Generic;
using DataAccessLayer.Models;

namespace BusinessLayer.ViewModels.Subscriptions;

public class SubscriptionsIndexViewModel
{
    public string UserId { get; set; }
    public List<WorkRequestSubscription> UserSubscriptions { get; set; }
}