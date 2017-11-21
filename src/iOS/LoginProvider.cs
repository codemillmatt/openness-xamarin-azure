using System;
using System.Threading.Tasks;
using Foundation;
using Microsoft.WindowsAzure.MobileServices;

using UIKit;

using VSLiveToDo.iOS;
using Xamarin.Forms;


[assembly: Dependency(typeof(LoginProvider))]
namespace VSLiveToDo.iOS
{
    public class LoginProvider : IAuthenticate
    {
        public LoginProvider()
        {
        }

        public async Task<bool> Authenticate(MobileServiceClient client)
        {
            try
            {
                var user = await client.LoginAsync(UIApplication.SharedApplication.KeyWindow.RootViewController, MobileServiceAuthenticationProvider.Twitter);

                return user != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
