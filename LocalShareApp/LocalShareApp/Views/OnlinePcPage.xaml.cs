using LocalShareApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LocalShareApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnlinePcPage : ContentPage
    {

        public OnlinePcPage()
        {
            BindingContext = ((App)Application.Current).ServiceProvider.GetService(typeof(OnlinePCPageViewModel)) as OnlinePCPageViewModel;
            InitializeComponent();


        }


    }
}