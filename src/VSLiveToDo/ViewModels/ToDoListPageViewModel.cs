using System;
using System.Collections.ObjectModel;
using MvvmHelpers;
using VSLiveToDo.Models;
using System.Threading.Tasks;
using System.Diagnostics;
using VSLiveToDo.Services;
using Xamarin.Forms;
using System.Linq;

namespace VSLiveToDo.ViewModels
{
    public class ToDoListPageViewModel : BaseViewModel
    {
        INavigation navigation;
        public ToDoListPageViewModel(INavigation nav)
        {
            Title = "To Do Items";

            navigation = nav;
            MessagingCenter.Subscribe<ItemUpdatedMessage, ToDoItem>(this, "refresh_list",
                (msg, item) =>
                {
                    if (msg.IsNewItem)
                        Items.Add(item);
                    else
                    {
                        var itemIndex = this.Items.IndexOf(this.Items.Where(i => i.Id == item.Id).First());
                        this.Items.RemoveAt(itemIndex);
                        this.Items.Insert(itemIndex, item);
                    }
                });
        }

        bool isRefreshing;
        public bool Refreshing
        {
            get
            {
                return isRefreshing;
            }
            set
            {
                SetProperty(ref isRefreshing, value);
            }
        }

        ObservableRangeCollection<ToDoItem> items = new ObservableRangeCollection<ToDoItem>();
        public ObservableRangeCollection<ToDoItem> Items
        {
            get { return items; }
            set { SetProperty(ref items, value, nameof(Items)); }
        }

        ToDoItem selectedItem;
        public ToDoItem SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                SetProperty(ref selectedItem, value, nameof(SelectedItem));

                if (selectedItem != null)
                {
                    navigation.PushAsync(new ToDoDetailPage(selectedItem));
                    SelectedItem = null;
                }
            }
        }

        Command refreshCommand;
        public Command RefreshCommand => refreshCommand ?? (refreshCommand =
                                                            new Command(async () => await ExecuteRefreshingCommand()));

        Command addNewCommand;
        public Command AddNewCommand => addNewCommand ?? (addNewCommand =
              new Command(async () =>
              {
                  if (IsBusy)
                      return;
                  IsBusy = true;

                  try
                  {
                      await navigation.PushAsync(new ToDoDetailPage());
                  }
                  finally
                  {
                      IsBusy = false;
                  }
              }));

        Command<ToDoItem> deleteCommand;
        public Command<ToDoItem> DeleteCommand => deleteCommand ?? (deleteCommand =
              new Command<ToDoItem>(async (todo) =>
              {
                  if (IsBusy)
                      return;

                  IsBusy = true;

                  await ZumoService.DefaultInstance.DeleteToDo(todo);

                  Items.Remove(todo);

                  IsBusy = false;
              }));

        async Task ExecuteRefreshingCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Refreshing = true;

                var success = await ZumoService.DefaultInstance.SyncData();
                if (!success)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Error while getting data", "OK");
                }

                Refreshing = false;

                var results = await ZumoService.DefaultInstance.GetAllToDoItems();

                Items.ReplaceRange(results);
            }
            finally
            {
                IsBusy = false;
                Refreshing = false;
            }
        }

        bool authenticated = false;
        public bool Authenticated
        {
            get
            {
                return authenticated;
            }
            set
            {
                SetProperty(ref authenticated, value);
            }
        }

        Command loginCmd;
        public Command LoginCommand => loginCmd ?? (loginCmd = new Command(async () =>
        {
            await ExecuteLoginCommand();

            LoginCommand.ChangeCanExecute();

        }, () => !Authenticated));

        async Task ExecuteLoginCommand()
        {
            Authenticated = await ZumoService.DefaultInstance.Login();
        }
    }
}
