using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

using VSLiveToDo.Services;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;

namespace VSLiveToDo.Droid
{
    [Activity(Label = "VSLiveToDo.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            Microsoft.AppCenter.Push.Push.PushNotificationReceived += async (sender, e) =>
            {
                await App.Current.MainPage.DisplayAlert("Push", e.Message, "OK");
            };

            AppCenter.Start("",
                  typeof(Analytics), typeof(Crashes), typeof(Microsoft.AppCenter.Push.Push));


            LoadApplication(new App());
        }

    }
}
