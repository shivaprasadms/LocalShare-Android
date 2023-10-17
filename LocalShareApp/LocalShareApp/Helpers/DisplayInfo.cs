
using LocalShareApp.Models;
using LocalShareApp.Utility;
using System;
using System.Threading.Tasks;

namespace LocalShareApp.Helpers
{
    internal class DisplayInfo
    {
        public static Task UpdateCurrentSendingFileInfo(TcpHostModel client, string fileName, long fileSizeInBytes)
        {
            var task = Task.Run(() =>
            {
                client.CurrentSendingFileName = fileName;
                client.CurrentSendingFileSize = FormatFileSize.GetSize(fileSizeInBytes);
            });

            return task;
        }

        public static Task UpdateCurrentReceivingFileInfo(TcpHostModel client, string fileName, long fileSizeInBytes)
        {
            var task = Task.Run(() =>
            {
                client.CurrentReceivingFileName = fileName;
                client.CurrentReceivingFileSize = FormatFileSize.GetSize(fileSizeInBytes);
            });

            return task;
        }

        public static async Task UpdateCurrentSendingFileProgress(TcpHostModel client, long bytesRead, long fileSizeInBytes, double timeInSeconds)
        {
            await Task.Run(() =>
            {
                double speed = (double)bytesRead / 1024.0 / 1024.0 / timeInSeconds;

                client.CurrentSendingFileSpeed = $"{Math.Round(speed, 2)} MB/s";

                double progressPercentage = ((double)bytesRead / fileSizeInBytes);

                client.CurrentSendingFilePercentage = progressPercentage;



            });
        }

        public static async Task UpdateCurrentReceivingFileProgress(TcpHostModel client, long bytesRead, long fileSizeInBytes, double timeInSeconds)
        {
            await Task.Run(() =>
            {
                double speed = (double)bytesRead / 1024.0 / 1024.0 / timeInSeconds;

                client.CurrentReceivingFileSpeed = $"{Math.Round(speed, 2)} MB/s";

                double progressPercentage = ((double)bytesRead / fileSizeInBytes);

                client.CurrentReceivingFilePercentage = progressPercentage;

            });
        }


    }
}
