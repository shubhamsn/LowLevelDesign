using NotificationService.Interfaces;
using System;

namespace NotificationService.Implementation
{
    public class TextNotification : NotificationType
    {
        public TextNotification(INotificationChannel notificationChannel) : base(notificationChannel)
        {
                
        }
        public override void SendNotification()
        {
            Console.WriteLine("Sending text");
            NotificationChannel.SendNotification();
        }
    }
}
