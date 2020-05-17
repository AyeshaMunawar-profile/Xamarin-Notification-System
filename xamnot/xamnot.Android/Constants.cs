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

namespace xamnot.Droid
{
    class Constants
    {
        //credentials for conncting with cloud and using notification HUB 
        public const string SenderID = "869438665846"; // Google API Project Number
        public const string ListenConnectionString = "Endpoint=sb://saint-c.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=ykrWXJjN78rctSlx4hn+A0PlJtvO7fMFfQV6jIoWNGA=";
        public const string NotificationHubName = "SAINT-C";
    }
}