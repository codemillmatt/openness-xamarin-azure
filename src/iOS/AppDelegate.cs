using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Xamarin.Forms;
using VSLiveToDo.Models;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using VSLiveToDo.Services;

namespace VSLiveToDo.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
