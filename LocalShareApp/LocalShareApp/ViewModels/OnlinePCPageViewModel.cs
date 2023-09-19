using LocalShareApp.Interfaces;
using LocalShareApp.Models;
using LocalShareApp.Services;
using System;
using System.Net.Sockets;
using System.Windows.Input;
using Xamarin.Forms;

namespace LocalShareApp.ViewModels
{

    internal class OnlinePCPageViewModel : ViewModelBase
    {

        //public ObservableCollection<TcpHostModel> Hosts { get; } = ActiveTcpConnections.Instance.Connections;



        public ActiveTcpConnections Hosts { get; set; } = ActiveTcpConnections.Instance;

        private readonly Interfaces.IMessageService messageService;



        public OnlinePCPageViewModel()
        {
            messageService = DependencyService.Get<Interfaces.IMessageService>();
            TcpManager.HostFoundEvent += OnHostDetected;
            // Hosts.Add(new TcpHostModel("APPu", "192.168.0.108", new TcpClient()));
            SendFileCommand = new Command(OpenSendFileDialogAsync);

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




        public ICommand SendFileCommand { get; set; }






        private void OnHostDetected(object sender, TcpClient client)
        {
            IsStackLayoutVisible = false;

            try
            {

                TcpHostModel model = new TcpHostModel("APPUPC", "192.168.0.105", client);

                Hosts.AddConnection(client);

                //Console.WriteLine($"Hosts collection count: {Hosts.Count}");
            }
            catch (Exception ex)
            {
                // Handle the exception or log it
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

        }








    }
}
