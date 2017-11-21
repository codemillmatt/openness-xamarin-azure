using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using VSLiveToDo.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(LoginProvider))]
namespace VSLiveToDo.Droid
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
                var user = await client.LoginAsync(Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity, MobileServiceAuthenticationProvider.Twitter);

                return user != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
