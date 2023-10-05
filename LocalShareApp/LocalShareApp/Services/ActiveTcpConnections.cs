using LocalShareApp.Models;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;

namespace LocalShareApp.Services
{
    public class ActiveTcpConnections
    {
        public ObservableCollection<TcpHostModel> Connections { get; private set; }

        private static readonly Lazy<ActiveTcpConnections> lazyInstance =
        new Lazy<ActiveTcpConnections>(() => new ActiveTcpConnections());

        public static ActiveTcpConnections Instance => lazyInstance.Value;

        private ActiveTcpConnections()
        {
            Connections = new ObservableCollection<TcpHostModel>();
        }


        public void AddConnection(TcpClient client)
        {

            var obj = new TcpHostModel("APPUPC", ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString(), client);
            //var obj = new TcpHostModel("APPUPC", "192.168.1.1", client);

            Connections.Add(obj);

        }
    }
}
