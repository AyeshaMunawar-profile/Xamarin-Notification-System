using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using xamnot.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(Notandroid))]
namespace xamnot.Droid
{
    public class Notandroid : notificationI
    {
        Context appcontext;

        

        public bool sendNotification()
        {
        appcontext = Android.App.Application.Context;
        PushHandlerService service1 = new PushHandlerService();
       
            service1.RegisterWithGCM(appcontext);
            Console.WriteLine("specific android function called");
            Log.Info("Register specific ", "registerd");
            return true;
        }

        public bool createNotification(string a, string b, string c)
        {
            PushHandlerService service1 = new PushHandlerService();
            service1.send_to_userAsync(a,b,c, appcontext);
            Console.WriteLine("specific Send generic notification called");
            Log.Info("Send notification specific ", "Done");
            return true;
        }

       
    }
}