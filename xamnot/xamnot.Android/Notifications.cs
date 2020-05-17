using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Azure.NotificationHubs;

namespace xamnot.Droid
{
    public class Notificationsgeneral
    {
        public static Notificationsgeneral Instance = new Notificationsgeneral();

        public NotificationHubClient Hub { get; set; }

        private Notificationsgeneral()
        {
            Hub = NotificationHubClient.CreateClientFromConnectionString("Endpoint=sb://saint-c.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=ykrWXJjN78rctSlx4hn+A0PlJtvO7fMFfQV6jIoWNGA=",
                                                                         "SAINT-C");
        }
    }
}