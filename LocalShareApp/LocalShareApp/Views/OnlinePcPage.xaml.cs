using LocalShareApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LocalShareApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnlinePcPage : ContentPage
    {
        public OnlinePcPage()
        {
            InitializeComponent();
           // BindingContext = new OnlinePCPageViewModel();
        }
    }
}