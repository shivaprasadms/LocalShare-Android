using LocalShareApp.Services;
using Xamarin.Forms;

namespace LocalShareApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<Interfaces.IMessageService, Utility.MessageAlertDialog>();
            MainPage = new AppShell();

        }

        protected override async void OnStart()
        {

            await MulticastListener.Listen();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
