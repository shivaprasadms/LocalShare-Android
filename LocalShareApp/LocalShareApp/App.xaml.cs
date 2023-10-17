using LocalShare.Services;
using LocalShareApp.Interfaces;
using LocalShareApp.Services;
using LocalShareApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;

namespace LocalShareApp
{
    public partial class App : Application
    {
        private MulticastListener _multicastListener;
        public ServiceProvider ServiceProvider { get; }

        public App()
        {


            DependencyService.Register<Interfaces.IMessageService, Utility.MessageAlertDialog>();

            IServiceCollection services = new ServiceCollection();
            ServiceProvider = RegisterServices(services);



            _multicastListener = ServiceProvider.GetRequiredService<MulticastListener>();

            InitializeComponent();


            MainPage = new AppShell();


        }

        protected override async void OnStart()
        {
            await _multicastListener.Listen();

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private ServiceProvider RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<SettingsPageViewModel>();
            services.AddSingleton<AboutPageViewModel>();
            services.AddSingleton<MulticastListener>();
            services.AddSingleton<ActiveTcpConnections>();
            services.AddSingleton<TcpManager>();
            services.AddSingleton<ILocalShareTransferService, LocalShareTransfer>();
            services.AddSingleton<LocalShareReceiver>();




            services.AddSingleton(serviceProvider =>

                new OnlinePCPageViewModel(serviceProvider.GetRequiredService<ILocalShareTransferService>(),
                    serviceProvider.GetRequiredService<ActiveTcpConnections>(), serviceProvider.GetRequiredService<TcpManager>())
            );





            return services.BuildServiceProvider();

            //services.AddLogging(loggingBuilder =>
            //{
            //    loggingBuilder.AddFile("app.log", append: true);
            //});
        }
    }
}
