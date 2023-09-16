using LocalShareApp.Utility;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LocalShareApp.Services
{
    public class FileTransferService
    {

        public static async Task SendToHost(string hostIp, string path)
        {

            var host = ActiveTcpConnections.Instance.Connections.FirstOrDefault(conn => conn.HostPcIP == hostIp);

            try
            {

                NetworkStream stream = host.Host.GetStream();


                string fileName = Path.GetFileName(path);

                FileInfo fileInfo = new FileInfo(path);

                string fileSize = FormatFileSize.GetSize(fileInfo.Length);

                long fileSizeInBytes = fileInfo.Length;

                string fileInfoString = $"{fileName}:{fileSizeInBytes}:";

                byte[] fileInfobuffer = new byte[Encoding.UTF8.GetByteCount(fileInfoString)];

                Encoding.UTF8.GetBytes(fileInfoString, 0, fileInfoString.Length, fileInfobuffer, 0);

                await stream.WriteAsync(fileInfobuffer, 0, fileInfobuffer.Length);

                host.CurrentSendingFileName = fileName;
                host.CurrentSendingFileSize = fileSize;


                using (FileStream fileStream = File.OpenRead(path))
                {
                    byte[] buffer = new byte[8192]; // 8 KB buffer

                    host.IsSendingFile = true; // refactoring required


                    int bytesRead = 0;
                    long completed = 0;

                    var taskController = new CancellationTokenSource();
                    var token = taskController.Token;




                    while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await stream.WriteAsync(buffer, 0, bytesRead);

                        completed += bytesRead;

                    }


                }



                host.CurrentSendingFileName = " ";

            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }



    }


}