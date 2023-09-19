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

                         byte[] filelen = new byte[1];
                         await stream.ReadAsync(filelen, 0, 1);

                         byte[] fileInfoBufferSize = new byte[int.Parse(Encoding.UTF8.GetString(filelen))];

                         await stream.ReadAsync(fileInfoBufferSize, 0, fileInfoBufferSize.Length);

                         string size = Encoding.UTF8.GetString(fileInfoBufferSize);

                         byte[] fileInfoBuffer = new byte[int.Parse(size)];

                         await stream.ReadAsync(fileInfoBuffer, 0, fileInfoBuffer.Length);

                         string fileInfo = Encoding.UTF8.GetString(fileInfoBuffer);

                         var fileInfoArray = fileInfo.Split(':');

                         string fileName = fileInfoArray[0];

                         string fileSize = fileInfoArray[1];

                         string type = fileInfoArray[2];

                         long fileSizeBytes = long.Parse(fileSize);

                         long pg = long.Parse(fileSize);

                         DisplayStats(fileName, fileSize, host);

                         var savePath = $"/storage/emulated/0/localshare{type}"; // fix extra / for single file

                         if (!(type.Equals("/")))
                         {
                             Directory.CreateDirectory(savePath);
                         }


                         using (FileStream fileStream = new FileStream($"{savePath}/{fileName}", FileMode.Create))
                         {
                             byte[] buffer = new byte[8192];

                             int bytesRead;
                             long completed = 0;

                             while ((bytesRead = await stream.ReadAsync(buffer, 0, (int)(fileSizeBytes > 8192 ? 8192 : fileSizeBytes))) > 0)
                             {


                                 await fileStream.WriteAsync(buffer, 0, bytesRead);

                                 fileSizeBytes -= bytesRead;

                                 if (fileSizeBytes <= 0) // optimize loop
                                     break;

                                 completed += bytesRead;

                                 host.CurrentReceivingFilePercentage = ((double)completed / pg);

                             }


                         }

                         //   await stream.WriteAsync(Encoding.UTF8.GetBytes("Done"), 0, 4);



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