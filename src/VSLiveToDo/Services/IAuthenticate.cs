using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace VSLiveToDo
{
    public interface IAuthenticate
    {
        Task<bool> Authenticate(MobileServiceClient client);
    }
}
