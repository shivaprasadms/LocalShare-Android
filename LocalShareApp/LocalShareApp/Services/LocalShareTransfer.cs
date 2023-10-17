using LocalShareApp.Helpers;
using LocalShareApp.Interfaces;
using LocalShareApp.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace LocalShare.Services
{
    public class LocalShareTransfer : ILocalShareTransferService
    {
        private readonly ILogger<LocalShareTransfer> _logger;

        public LocalShareTransfer()
        {

        }

        public async Task SendToClient(TcpHostModel client, string[] selectedPath, bool isDirectory)
        {
            if (isDirectory)
            {
                var result = await GetFilePathsFromDirectory(selectedPath);
                client.AddFilesToQueue(result);
            }
            else
            {
                client.AddFilesToQueue(new List<Tuple<string, string[]>>() { new Tuple<string, string[]>("/", selectedPath) });
            }

            if (client.IsSendingFile) return;

            await Send(client);

        }

        private async Task Send(TcpHostModel client)
        {
            await Task.Factory.StartNew(async () =>
            {

                try
                {
                    client.IsSendingFile = true;

                    NetworkStream stream = client.Host.GetStream();

                    while (!client.IsQueueEmpty())
                    {

                        var (fileLevel, filePaths) = client.PopFileFromQueue();

                        foreach (var path in filePaths)
                        {

                            FileInfo fileInfo = new FileInfo(path);

                            string fileName = fileInfo.Name;

                            long fileSizeInBytes = fileInfo.Length;

                            string fileInfoString = $"{fileName}:{fileSizeInBytes}:{fileLevel}:"; // <300 length refactor

                            int fileInfoStringLength = 0;

                            if (fileInfoString.Length >= 0 && fileInfoString.Length <= 9)
                            {
                                fileInfoStringLength = 1;
                            }
                            else if (fileInfoString.Length >= 10 && fileInfoString.Length <= 99)
                            {
                                fileInfoStringLength = 2;
                            }
                            else
                            {
                                fileInfoStringLength = 3;
                            }

                            await stream.WriteAsync(Encoding.UTF8.GetBytes(fileInfoStringLength.ToString()), 0, fileInfoStringLength.ToString().Length);

                            byte[] fileSizeHeader = Encoding.UTF8.GetBytes(fileInfoString.Length.ToString());

                            await stream.WriteAsync(fileSizeHeader, 0, fileSizeHeader.Length);


                            byte[] fileInfobuffer = new byte[Encoding.UTF8.GetByteCount(fileInfoString)];

                            Encoding.UTF8.GetBytes(fileInfoString, 0, fileInfoString.Length, fileInfobuffer, 0);

                            await stream.WriteAsync(fileInfobuffer, 0, fileInfobuffer.Length);


                            var updateFileInfoToClient = DisplayInfo.UpdateCurrentSendingFileInfo(client, fileName, fileSizeInBytes);



                            int bytesRead = 0;
                            long bytesSent = 0;

                            Stopwatch stopwatch = new Stopwatch();
                            Timer timer = new Timer(300);

                            timer.Elapsed += async (sender, e) =>
                            await DisplayInfo.UpdateCurrentSendingFileProgress(client, bytesSent, fileSizeInBytes, stopwatch.Elapsed.TotalSeconds);

                            long pg = fileSizeInBytes;

                            await updateFileInfoToClient;

                            using (FileStream fileStream = File.OpenRead(path))
                            {
                                byte[] buffer = new byte[8192];

                                timer.Start();
                                stopwatch.Start();

                                while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                                {
                                    await stream.WriteAsync(buffer, 0, bytesRead);

                                    pg -= bytesRead;
                                    if (pg < 0) break;

                                    bytesSent += bytesRead;

                                }
                            }

                            timer.Dispose();
                        }

                    }

                    client.IsSendingFile = false;
                    client.ResetProperties();

                }
                catch (Exception ex)
                {
                    //  _logger.LogError(ex, ex.Message);
                    throw;
                    //MessageBox.Show(ex.Message);
                }
            }, TaskCreationOptions.LongRunning);
        }

        private async Task<List<Tuple<string, string[]>>> GetFilePathsFromDirectory(string[] rootDirectoryPath)
        {
            try
            {
                var result = await Task.Run(() =>
                {
                    List<Tuple<string, string[]>> transferStructure = new();

                    foreach (var folder in rootDirectoryPath)
                    {
                        string[] rootDirFilesPath = Directory.GetFiles(folder);
                        string selectedFolderName = Path.GetFileName(folder);

                        if (rootDirFilesPath.Length != 0)
                            transferStructure.Add(new Tuple<string, string[]>(selectedFolderName, rootDirFilesPath));

                        var nestedDirectories = Directory.GetDirectories(folder, "*", System.IO.SearchOption.AllDirectories);

                        foreach (var dir in nestedDirectories)
                        {
                            string[] nestedDirFiles = Directory.GetFiles(dir);
                            if (nestedDirFiles.Length == 0)
                                continue;
                            transferStructure.Add(new Tuple<string, string[]>(dir.Substring(dir.IndexOf(selectedFolderName)), nestedDirFiles));
                        }
                    }

                    return transferStructure;
                });

                return result;
            }
            catch (Exception ex)
            {
                //  _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }


}
