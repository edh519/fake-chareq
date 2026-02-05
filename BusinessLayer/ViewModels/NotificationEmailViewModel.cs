using DataAccessLayer.Models;

namespace BusinessLayer.ViewModels
{
    public class NotificationEmailViewModel
    {
        public Notification Notification { get; set; }
        public string LinkToSystem { get; set; }
        public string RecipientFriendlyFormat { get => Helpers.CommonHelpers.RemoveDomainFromEmail(Notification.Recipient); }
        public string CreatedByFriendlyFormat { get => Helpers.CommonHelpers.RemoveDomainFromEmail(Notification.CreatedBy); }
        public string SalutationFriendlyFormat { get => Helpers.CommonHelpers.SalutationFromEmail(Notification.Recipient); }
    }
}
