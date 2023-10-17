using LocalShareApp.Interfaces;
using System.Windows.Input;
using Xamarin.Forms;

namespace LocalShareApp.ViewModels
{
    public class SettingsPageViewModel
    {
        public ICommand OpenSaveDirectory { get; private set; }
        public ICommand CheckAppUpdates { get; private set; }
        private readonly IMessageService messageService;

        public string AppUpdateLastChecked { get; set; }

        public SettingsPageViewModel()
        {
            OpenSaveDirectory = new Command(OpenSaveDir);
            CheckAppUpdates = new Command(CheckIfNewUpdateAvailable);
            messageService = DependencyService.Get<IMessageService>();
            AppUpdateLastChecked = "Never";
        }

        private void CheckIfNewUpdateAvailable(object obj)
        {
            messageService.ShowAsync("Server currently unavailable"); // yet to be implemented.
        }

        private async void OpenSaveDir()
        {
            var folderService = DependencyService.Get<IFilePicker>();
            await folderService.OpenFolder("/storage/emulated/0/localshare");
        }

    }
}
