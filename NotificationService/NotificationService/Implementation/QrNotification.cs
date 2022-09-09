
using NotificationService.Interfaces;
using System;

namespace NotificationService.Implementation
{
    public class QrNotification: NotificationType
    {
        public QrNotification(INotificationChannel notificationChannel) : base(notificationChannel)
        {

        }

        public override void SendNotification()
        {
            Console.WriteLine("Sending QR");
            NotificationChannel.SendNotification();
        }
    }
}
