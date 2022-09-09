using NotificationService.Interfaces;

namespace NotificationService.Implementation
{
    public abstract class NotificationType
    {
        public INotificationChannel NotificationChannel;

        protected NotificationType(INotificationChannel notificationChannel)
        {
            NotificationChannel = notificationChannel;
        }

        public abstract void SendNotification();
    }
}
