using DataAccessLayer.Models;
using Enums.Enums;

namespace BusinessLayer.Repositories.Interfaces
{
    public interface INotificationTypeRepository : IRepository<NotificationType>
    {
        NotificationType GetNotificationType(NotificationTypeEnum notificationType);
    }
}
