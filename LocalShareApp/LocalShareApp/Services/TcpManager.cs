using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LocalShareApp.Services
{
    public class TcpManager
    {

        public static EventHandler<TcpClient> HostFoundEvent;

        private readonly Interfaces.IMessageService messageService;

        public TcpManager()
        {
            messageService = DependencyService.Get<Interfaces.IMessageService>();
        }

        public static async Task ConnectToHost(string hostIp, int port)
        {

            TcpClient host = new TcpClient();

            try
            {

                await host.ConnectAsync(hostIp, port);

                // ActiveTcpConnections.Instance.AddConnection(host);

                TcpManager.HostFoundEvent?.Invoke(EventArgs.Empty, host);

                await FileReceivingService.ReceiveFromHost(); // fix for multiple hosts based on send file button tag.

            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }

            //TcpManager.Instance.ConnectedClients.Add(client);

        }

    }
}