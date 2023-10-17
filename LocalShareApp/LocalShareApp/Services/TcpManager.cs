using LocalShare.Services;
using LocalShareApp.Models;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LocalShareApp.Services
{
    public class TcpManager
    {

        public EventHandler<bool> AnyPCFoundEvent;
        private readonly LocalShareReceiver _localShareReceiver;
        private readonly ActiveTcpConnections _activeTcpConnections;
        private readonly Interfaces.IMessageService messageService;

        public TcpManager(LocalShareReceiver localShareReceiver, ActiveTcpConnections activeTcpConnections)
        {
            messageService = DependencyService.Get<Interfaces.IMessageService>();
            _localShareReceiver = localShareReceiver;
            _activeTcpConnections = activeTcpConnections;
        }

        public async Task ConnectToHost(string hostIp, int port)
        {

            TcpClient host = new TcpClient();

            try
            {
                await host.ConnectAsync(hostIp, port);
                AnyPCFoundEvent?.Invoke(EventArgs.Empty, true);

                HandleClient(host);
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }

        }

        private async Task HandleClient(TcpClient client)
        {
            var clientModel = new TcpHostModel("PIXEL", ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString(), client);
            _activeTcpConnections.AddConnection(clientModel);

            await _localShareReceiver.ReceiveFromClient(clientModel);

        }

    }
}