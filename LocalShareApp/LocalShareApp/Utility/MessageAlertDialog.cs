using LocalShareApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LocalShareApp.Utility
{
    public class MessageAlertDialog : IMessageService
    {

        public async Task ShowAsync(string message)
        {
            await App.Current.MainPage.DisplayAlert("YourApp", message, "Ok");
        }
    }
}
