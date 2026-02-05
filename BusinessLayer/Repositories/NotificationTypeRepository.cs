using BusinessLayer.Repositories.Interfaces;
using DataAccessLayer;
using DataAccessLayer.Models;
using Enums.Enums;
using System.Linq;

namespace BusinessLayer.Repositories
{
    public class NotificationTypeRepository : Repository<NotificationType>, INotificationTypeRepository
    {
        public NotificationTypeRepository(ApplicationDbContext context) : base(context)
        { }

        /// <summary>
        /// Gets a specific notification type based on the NotificationTypeEnum passed in.
        /// Throws exception if there is more than one notification type.
        /// </summary>
        /// <param name="notificationType">Enum for notification type to get.</param>
        /// <returns>SingleOrDefault Notification type matching the NotificationType passed in.</returns>
        public NotificationType GetNotificationType(NotificationTypeEnum notificationType)
        {
            return _context.NotificationTypes
                    .Where(q => q.NotificationTypeId == notificationType)
                    .SingleOrDefault();
        }


    }
}
