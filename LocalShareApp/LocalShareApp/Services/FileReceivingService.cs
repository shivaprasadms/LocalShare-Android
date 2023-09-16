using LocalShareApp.Models;
using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LocalShareApp.Services
{
    public class FileReceivingService
    {

        public static async Task ReceiveFromHost()
        {
            await Task.Factory.StartNew(async () =>
             {
                 TcpHostModel host = ActiveTcpConnections.Instance.Connections.First();
                 NetworkStream stream = host.Host.GetStream();

                 try
                 {

                     while (true)
                     {

                         byte[] fileInfoBufferSize = new byte[3];

                         //await stream.ReadAsync(fileInfoBufferSize, 0, 3);

                         //string size = Encoding.UTF8.GetString(fileInfoBufferSize);

                         byte[] fileInfoBuffer = new byte[275];

                         await stream.ReadAsync(fileInfoBuffer, 0, fileInfoBuffer.Length);

                         string fileInfo = Encoding.UTF8.GetString(fileInfoBuffer);

                         var fileInfoArray = fileInfo.Split(':');

                         string fileName = fileInfoArray[0];

                         string fileSize = fileInfoArray[1];

                         long fileSizeBytes = long.Parse(fileSize);

                         long pg = long.Parse(fileSize);

                         DisplayStats(fileName, fileSize, host);

                         //await stream.WriteAsync(Encoding.UTF8.GetBytes("Start"), 0, 5);

                         using (FileStream fileStream = new FileStream($"/storage/emulated/0/localshare/{fileName}", FileMode.Create))
                         {
                             byte[] buffer = new byte[8192];

                             int bytesRead;
                             long completed = 0;

                             while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                             {

                                 await fileStream.WriteAsync(buffer, 0, bytesRead);
                                 fileSizeBytes -= bytesRead;

                                 if (fileSizeBytes <= 0) // optimize loop
                                     break;

                                 completed += bytesRead;

                                 host.CurrentSendingFilePercentage = ((double)completed / pg);

                             }


                         }

                         await stream.WriteAsync(Encoding.UTF8.GetBytes("Start"), 0, 4);



                         ResetStats(host);


                     }
                 }
                 catch (Exception ex)
                 {
                     System.Diagnostics.Debug.WriteLine(ex.Message);

                 }
             }, TaskCreationOptions.LongRunning);



        }

        private static void ResetStats(TcpHostModel host)
        {
            host.IsReceivingFile = false;
            host.CurrentSendingFileSize = "";
            host.CurrentSendingFileName = "";

        }

        private static void DisplayStats(string fileName, string fileSize, TcpHostModel host)
        {
            host.IsReceivingFile = true;
            host.CurrentReceivingFileName = fileName;
            host.CurrentReceivingFileSize = fileSize;
        }
    }
}