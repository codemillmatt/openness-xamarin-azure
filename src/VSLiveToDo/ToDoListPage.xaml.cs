using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace VSLiveToDo
{
    public partial class ToDoListPage : ContentPage
    {
        ViewModels.ToDoListPageViewModel vm = null;

        public ToDoListPage()
        {
            InitializeComponent();

            vm = new ViewModels.ToDoListPageViewModel(Navigation);

            BindingContext = vm;
            vm.RefreshCommand.Execute(null);
        }

        public void Delete_Clicked(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;
            var vm = (ViewModels.ToDoListPageViewModel)BindingContext;

            vm.DeleteCommand.Execute(mi.CommandParameter);
        }

    }
}
