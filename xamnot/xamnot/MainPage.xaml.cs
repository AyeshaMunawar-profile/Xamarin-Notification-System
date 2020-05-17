using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace xamnot
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
            string tag = "Saint";

            ////var register = DependencyService.Get<notificationI>().sendNotification();
            ////DisplayAlert("Alert", "registered" , "OK");
            var send = DependencyService.Get<notificationI>().createNotification("hello", "Ayesha", tag);
            DisplayAlert("Alert", "Custom notification sent", "OK");

            InitializeComponent();
		}
	}



}
