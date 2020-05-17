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
using Android.Util;
using Gcm.Client;
using WindowsAzure.Messaging;


//File where code related to recieving notification is written

//Uses precompiled dll files for operations 
//Assembly is a file that is created upon successful run of a .NET project in form of DLL or EXE file
[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]

//GET_ACCOUNTS is needed only for Android versions 4.0.3 and below
[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]


namespace xamnot.Droid
{
  
    [BroadcastReceiver(Permission = Gcm.Client.Constants.PERMISSION_GCM_INTENTS)]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_MESSAGE },
        Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK },
        Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_LIBRARY_RETRY },
        Categories = new string[] { "@PACKAGE_NAME@" })]
    //MyBroadcastReiciever is a class that implements the interface GCMBroadcastReceiverBase 
    //thus it should implement all the fucntions andlocal variables whose signatures are declared in 
    //the inteface GCMBroadcastReceiverBase
   
    public class MyBroadcastReceiver : GcmBroadcastReceiverBase<PushHandlerService>
    {
        public static string[] SENDER_IDS = new string[] { Constants.SenderID };

        public const string TAG = "MyBroadcastReceiver-GCM";
    }


   
    [Service] // Must use the service tag
    public class PushHandlerService : GcmServiceBase
    {
        public static string RegistrationID { get; private set; }
        private NotificationHub Hub { get; set; }
      
        public PushHandlerService() : base(Constants.SenderID)
        {
            Log.Info(MyBroadcastReceiver.TAG, "PushHandlerService() constructor");
        }

        protected override void OnRegistered(Context context, string registrationId)
        {
            Log.Verbose(MyBroadcastReceiver.TAG, "GCM Registered: " + registrationId);
            RegistrationID = registrationId;

            createNotification("SAINT Notification",
                "You are sucessfully registed as authorized user");

            Hub = new NotificationHub(Constants.NotificationHubName, Constants.ListenConnectionString,
                context);
            try
            {
                Hub.UnregisterAll(registrationId);
            }
            catch (Exception ex)
            {
                Log.Error(MyBroadcastReceiver.TAG, ex.Message);
            }

            var tags = new List<string>() { "SAINT" }; // create tags if you want

            Console.WriteLine(tags);
            try
            {
                var hubRegistration = Hub.Register(registrationId, tags.ToArray());
            }
            catch (Exception ex)
            {
                Log.Error(MyBroadcastReceiver.TAG, ex.Message);
            }
        }

        protected override void OnMessage(Context context, Intent intent)
        {
            Log.Info(MyBroadcastReceiver.TAG, "Device registered");

            var msg = new StringBuilder();

            if (intent != null && intent.Extras != null)
            {
                foreach (var key in intent.Extras.KeySet())
                    msg.AppendLine(key + "=" + intent.Extras.Get(key).ToString());
            }

            string messageText = intent.Extras.GetString("message");
            if (!string.IsNullOrEmpty(messageText))
            {
                createNotification("SAINT Notification", messageText);
                //Toast.MakeText(this, "Registration notification created" + tags, ToastLength.Short).Show();
            }
            else
            {
                createNotification("Unknown message details", msg.ToString());
            }
        }


        void createNotification(string title, string desc)
        {
            //Create notification
            var notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;

            //Create an intent to show UI
            var uiIntent = new Intent(this, typeof(MainActivity));

            //Create the notification
            var notification = new Notification(Android.Resource.Drawable.SymActionEmail, title);

            //Auto-cancel will remove the notification once the user touches it
            notification.Flags = NotificationFlags.AutoCancel;

            //Set the notification info
            //we use the pending intent, passing our ui intent over, which will get called
            //when the notification is tapped.
            notification.SetLatestEventInfo(this, title, desc, PendingIntent.GetActivity(this, 0, uiIntent, 0));

            //Show the notification
            notificationManager.Notify(1, notification);
            //dialogNotify(title, desc);
        }

        //protected void dialogNotify(String title, String message)
        //{

        //    MainActivity.instance.RunOnUiThread(() => {
        //        AlertDialog.Builder dlg = new AlertDialog.Builder(MainActivity.instance);
        //        AlertDialog alert = dlg.Create();
        //        alert.SetTitle(title);
        //        alert.SetButton("Ok", delegate {
        //            alert.Dismiss();
        //        });
        //        alert.SetMessage(message);
        //        alert.Show();
        //    });
        //}

        public async void send_to_userAsync(string a, string b ,string userTag , Context c)
        {

            Microsoft.Azure.NotificationHubs.NotificationOutcome outcome = null;
            var notif = "{ \"data\" : {\"message\":\"" + "From " + a + ": " + b + "\"}}";
      
        outcome = await Notificationsgeneral.Instance.Hub.SendGcmNativeNotificationAsync(notif, userTag);
            Console.WriteLine("notification generic sent");
            //Toast.MakeText(this, "Custom notification sent to " + tags, ToastLength.Short).Show();

        }
            

        protected override void OnUnRegistered(Context context, string registrationId)
        {
            Log.Verbose(MyBroadcastReceiver.TAG, "GCM Unregistered: " + registrationId);

            createNotification("GCM Unregistered...", "The device has been unregistered!");
        }

        protected override bool OnRecoverableError(Context context, string errorId)
        {
            Log.Warn(MyBroadcastReceiver.TAG, "Recoverable Error: " + errorId);

            return base.OnRecoverableError(context, errorId);
        }

        protected override void OnError(Context context, string errorId)
        {
            Log.Error(MyBroadcastReceiver.TAG, "GCM Error: " + errorId);

           
        }

       
        public void RegisterWithGCM(Context c )
        {
            // Check to ensure everything's set up right
            
            GcmClient.CheckDevice(c);
            GcmClient.CheckManifest(c);
          
            // Register for push notifications
            Log.Info("MainActivity", "Registering...");
            GcmClient.Register(c, Constants.SenderID);
          
        }

    }
}

