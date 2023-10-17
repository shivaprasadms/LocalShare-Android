using LocalShareApp.Interfaces;
using System.Threading.Tasks;

namespace LocalShareApp.Utility
{
    public class MessageAlertDialog : IMessageService
    {

        public async Task ShowAsync(string message)
        {
            await App.Current.MainPage.DisplayAlert("LocalShare", message, "OK");
        }
    }
}
