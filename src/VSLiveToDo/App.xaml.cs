using Xamarin.Forms;
using VSLiveToDo.Services;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace VSLiveToDo
{
    public partial class App : Application
    {
        public static IAuthenticate Authenticator { get; private set; }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new ToDoListPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            OnResume();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

    }
}
