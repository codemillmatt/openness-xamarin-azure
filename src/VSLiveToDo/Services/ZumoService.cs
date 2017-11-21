using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using VSLiveToDo.Models;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using System.Diagnostics;

namespace VSLiveToDo.Services
{
    public class ZumoService
    {
        static ZumoService defaultInstance;

        MobileServiceClient client;
        IMobileServiceSyncTable<ToDoItem> table;
        //IMobileServiceTable<ToDoItem> table;

        private ZumoService()
        {
            client = new MobileServiceClient("__YOUR URL HERE__");
        }

        public static ZumoService DefaultInstance
        {
            get
            {
                if (defaultInstance == null)
                    defaultInstance = new ZumoService();

                return defaultInstance;
            }
        }

        async Task Initializer()
        {
            if (client.SyncContext.IsInitialized)
                return;

            var store = new MobileServiceSQLiteStore("todos.db");

            store.DefineTable<ToDoItem>();

            await client.SyncContext.InitializeAsync(store);

            table = client.GetSyncTable<ToDoItem>();
            //table = client.GetTable<ToDoItem>();
        }

        public async Task<bool> SyncData()
        {
            try
            {
                Settings.HasSyncStarted = true;

                await Initializer();

                await client.SyncContext.PushAsync();

                await table.PullAsync("todo-incremental", table.CreateQuery());
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                Settings.HasSyncStarted = false;
            }

            return true;
        }

        public async Task<List<ToDoItem>> GetAllToDoItems()
        {
            await this.Initializer();

            return await table.ToListAsync();
        }

        public async Task CreateToDo(ToDoItem item)
        {
            await this.Initializer();

            await table.InsertAsync(item);
        }

        public async Task UpdateToDo(ToDoItem item)
        {
            await this.Initializer();

            await table.UpdateAsync(item);
        }

        public async Task DeleteToDo(ToDoItem item)
        {
            await this.Initializer();

            await table.DeleteAsync(item);
        }

        public async Task<bool> Login()
        {
            var authenticator = DependencyService.Get<IAuthenticate>(DependencyFetchTarget.GlobalInstance);

            return await authenticator.Authenticate(client);
        }
    }
}
