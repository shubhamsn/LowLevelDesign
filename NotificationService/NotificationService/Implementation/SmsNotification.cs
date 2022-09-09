using NotificationService.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationService.Implementation
{
    public class SmsNotification : INotificationChannel
    {
        public void SendNotification()
        {
            Console.WriteLine("Sending SMS");
        }
    }
}
