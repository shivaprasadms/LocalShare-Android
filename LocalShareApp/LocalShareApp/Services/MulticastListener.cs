using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LocalShareApp.Services
{
    public class MulticastListener
    {

        public static async Task Listen()
        {




            string multicastIPAddress = "226.1.1.1";

            int multicastPort = 52345;

            UdpClient udpClient = new UdpClient();

            IPAddress multicastAddress = IPAddress.Parse(multicastIPAddress);

            // udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, multicastPort);
            udpClient.Client.Bind(endPoint);

            udpClient.JoinMulticastGroup(multicastAddress);

            udpClient.EnableBroadcast = true;

            try
            {
                while (true)
                {


                    UdpReceiveResult result = await udpClient.ReceiveAsync();

                    string message = Encoding.UTF8.GetString(result.Buffer).Split(':').Last();

                    int port = Convert.ToInt32(message);

                    await TcpManager.ConnectToHost(result.RemoteEndPoint.Address.ToString(), port);


                    // await Task.Delay(2000);

                    //make sure to accept only one host once skip rest of scans

                    break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                udpClient.DropMulticastGroup(multicastAddress);
                udpClient.Close();
            }
        }



    }
}
