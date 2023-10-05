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

        public static async Task SendToHost(string hostIp, Tuple<string, string[]> files)
        {

            var host = ActiveTcpConnections.Instance.Connections.FirstOrDefault(conn => conn.HostPcName == "APPUPC");

            host.AddFilesToQueue(files);

            if (host.IsSendingFile) return;

            var tsk = Task.Factory.StartNew(async () =>
              {

                  try
                  {

                      NetworkStream stream = host.Host.GetStream();

                      while (!host.IsQueueEmpty())
                      {
                          host.IsSendingFile = true;

                          var file = host.PopFileFromQueue();

                          foreach (var path in file.Item2)
                          {



                              string fileName = Path.GetFileName(path);

                              FileInfo fileInfo = new FileInfo(path);

                              string fileSize = FormatFileSize.GetSize(fileInfo.Length);

                              long fileSizeInBytes = fileInfo.Length;

                              long pg = fileSizeInBytes;

                              string fileInfoString = $"{fileName}:{fileSizeInBytes}:{file.Item1}";

                              byte[] fileInfobuffer = new byte[Encoding.UTF8.GetByteCount(fileInfoString)];

                              Encoding.UTF8.GetBytes(fileInfoString, 0, fileInfoString.Length, fileInfobuffer, 0);

                              int length = 0;

                              if (fileInfoString.Length > 0 && fileInfoString.Length < 9)
                              {
                                  length = 1;
                              }
                              else if (fileInfoString.Length > 10 && fileInfoString.Length < 99)
                              {
                                  length = 2;
                              }
                              else
                              {
                                  length = 3;
                              }

                              string len = length.ToString();


                              int fileInfoStringByteCount = Encoding.UTF8.GetByteCount(fileInfoString);

                              byte[] fileSizeHeader = new byte[fileInfoStringByteCount];
                              fileSizeHeader = Encoding.UTF8.GetBytes(fileInfoString.Length.ToString());

                              await stream.WriteAsync(Encoding.UTF8.GetBytes(len), 0, len.Length);

                              await stream.WriteAsync(fileSizeHeader, 0, fileSizeHeader.Length);


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

                                      pg -= bytesRead;
                                      if (pg < 0) break;

                                      completed += bytesRead;



                                      host.CurrentSendingFilePercentage = ((double)completed / fileSizeInBytes);

                                  }


                              }

                              //  byte[] signal = new byte[3];




                              // so no thread should access this when this reads the stream, the receiver can wait until 3 bytes of the stream are received here



                              //   await stream.ReadAsync(signal, 0, 3);

                              host.CurrentSendingFileName = " ";
                              host.CurrentSendingFileSize = "";
                          }
                      }

                      host.IsSendingFile = false;

                  }
                  catch (Exception ex)
                  {

                      Debug.WriteLine(ex.Message);
                  }
              }, TaskCreationOptions.LongRunning);
        }



    }


}