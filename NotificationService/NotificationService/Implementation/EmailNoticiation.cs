using NotificationService.Interfaces;
using System;

namespace NotificationService.Implementation
{
    public class EmailNoticiation : INotificationChannel
    {
        void INotificationChannel.SendNotification()
        {
            Console.WriteLine("Sending Email");
        }
    }
}
