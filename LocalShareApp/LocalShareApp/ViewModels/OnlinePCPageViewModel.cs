using LocalShareApp.Interfaces;
using LocalShareApp.Models;
using LocalShareApp.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace LocalShareApp.ViewModels
{

    public class OnlinePCPageViewModel : ViewModelBase
    {

        public ICommand SendFileCommand { get; set; }
        public ICommand SendFolderCommand { get; set; }

        public ActiveTcpConnections Hosts { get; }

        private readonly IMessageService messageService;
        private readonly ILocalShareTransferService _localShareTransferService;
        private readonly TcpManager _tcpManager;


        public OnlinePCPageViewModel(ILocalShareTransferService localShareTransferService, ActiveTcpConnections activeTcpConnections, TcpManager tcpManager)
        {
            SendFileCommand = new Command(OpenSendFileDialogAsync);
            SendFolderCommand = new Command(OpenSendFolderDialogAsync);
            messageService = DependencyService.Get<IMessageService>();

            _localShareTransferService = localShareTransferService;
            Hosts = activeTcpConnections;
            _tcpManager = tcpManager;
            _tcpManager.AnyPCFoundEvent += OnHostDetected;

            //Hosts.AddConnection(new TcpClient());


        }

        private async void OpenSendFolderDialogAsync(object obj)
        {
            var host = (obj as TcpHostModel);
            var folderPicker = DependencyService.Get<IFilePicker>();
            var folderPath = await folderPicker.PickFolder();

            if (folderPath != null)
                await _localShareTransferService.SendToClient(host, new[] { folderPath }, true);
        }

        private async void OpenSendFileDialogAsync(object obj)
        {
            try
            {
                var host = (obj as TcpHostModel);

                var filePicker = DependencyService.Get<IFilePicker>();
                var filePath = await filePicker.PickFiles();

                if (filePath != null)
                    await _localShareTransferService.SendToClient(host, filePath, false);



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
