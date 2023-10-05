using LocalShareApp.Interfaces;
using LocalShareApp.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace LocalShareApp.ViewModels
{

    internal class OnlinePCPageViewModel : ViewModelBase
    {

        //public ObservableCollection<TcpHostModel> Hosts { get; } = ActiveTcpConnections.Instance.Connections;

        public ICommand SendFileCommand { get; set; }
        public ICommand SendFolderCommand { get; set; }

        public ActiveTcpConnections Hosts { get; set; } = ActiveTcpConnections.Instance;

        private readonly IMessageService messageService;



        public OnlinePCPageViewModel()
        {
            SendFileCommand = new Command(OpenSendFileDialogAsync);
            SendFolderCommand = new Command(OpenSendFolderDialogAsync);
            messageService = DependencyService.Get<IMessageService>();
            TcpManager.AnyPCFoundEvent += OnHostDetected;

            //Hosts.AddConnection(new TcpClient());


        }

        private async void OpenSendFolderDialogAsync(object obj)
        {
            var folderPicker = DependencyService.Get<IFilePicker>();
            var folderPath = await folderPicker.PickFolder();
        }

        private async void OpenSendFileDialogAsync(object obj)
        {
            try
            {
                var filePicker = DependencyService.Get<IFilePicker>();
                var filePath = await filePicker.PickFiles();


                await FileTransferService.SendToHost("192.168.0.108", filePath);

                //var host = obj as TcpHostModel;


                //if (!string.IsNullOrEmpty(filePath))
                //{

                //    await messageService.ShowAsync(filePath);
                //    await FileTransferService.SendToHost("192.168.0.108", filePath);
                //}

            }
            catch (Exception ex)
            {

                await messageService.ShowAsync($"Error picking a file: {ex.Message}");
            }

        }



        #region This section handles the code for loading spinner at the startup of the app while finding for the host

        private bool LoadingStackLayoutVisible = true;


        public bool IsStackLayoutVisible
        {
            get { return LoadingStackLayoutVisible; }
            set
            {
                if (LoadingStackLayoutVisible != value)
                {
                    SetProperty(ref LoadingStackLayoutVisible, value, nameof(IsStackLayoutVisible));
                }
            }
        }


        #endregion

        private void OnHostDetected(object sender, bool status)
        {
            IsStackLayoutVisible = !status;


        }

    }
}
