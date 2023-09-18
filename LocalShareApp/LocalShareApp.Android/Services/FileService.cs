using LocalShareApp.Interfaces;
using MyXamarinApp.Droid.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileService))]
namespace MyXamarinApp.Droid.Services
{
    public class FileService : IFileService
    {
        public async Task WriteFileAsync(string fileName, string content)
        {
            try
            {
                //string downloadsFolder = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
                //string filePath = Path.Combine(downloadsFolder, fileName);

                //using (StreamWriter writer = File.CreateText(filePath))
                //{
                //    await writer.WriteAsync(content);
                //}

                string root = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "localshare");

                Directory.CreateDirectory(root);




            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
