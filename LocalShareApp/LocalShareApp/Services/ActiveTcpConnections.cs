using LocalShareApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;

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

            Connections.Add(obj);

        }
    }
}
