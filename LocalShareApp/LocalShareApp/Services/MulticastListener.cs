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



            string multicastIPAddress = "239.0.0.1"; // Multicast IP address (use a valid multicast IP)
            int multicastPort = 42345; // Multicast port number (use any available port)

            UdpClient udpClient = new UdpClient();

            // Join the multicast group
            IPAddress multicastAddress = IPAddress.Parse(multicastIPAddress);
            udpClient.JoinMulticastGroup(multicastAddress);
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, multicastPort);
            udpClient.Client.Bind(endPoint);

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
