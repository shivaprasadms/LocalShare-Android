using LocalShareApp.Models;
using System.Collections.ObjectModel;

namespace LocalShareApp.Services
{
    public class ActiveTcpConnections
    {
        public ObservableCollection<TcpHostModel> Connections { get; private set; }

        public ActiveTcpConnections()
        {
            Connections = new ObservableCollection<TcpHostModel>();
        }


        public void AddConnection(TcpHostModel host)
        {

            Connections.Add(host);

        }
    }
}
