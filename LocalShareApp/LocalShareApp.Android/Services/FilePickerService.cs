using Android.App;
using Android.Content;
using Android.Database;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LocalShareApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;


namespace LocalShareApp.Droid.Services
{
    public class FilePickerService : IFilePicker
    {
        private static int FILE_PICKER_REQUEST = 1;

        private TaskCompletionSource<string> tcs;

        public async Task<string> PickAFile()
        {
            tcs = new TaskCompletionSource<string>();

            Intent intent = new Intent(Intent.ActionOpenDocument);
            intent.AddCategory(Android.Content.Intent.CategoryOpenable);
            intent.SetType("*/*");

            MainActivity.Instance.StartActivityForResult(intent, FILE_PICKER_REQUEST);

            return await tcs.Task;
        }

        public void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == FILE_PICKER_REQUEST)
            {
                if (resultCode == Result.Ok)
                {
                    Android.Net.Uri uri = data.Data;

                    string filePath = MainActivity.Instance.GetActualPathFromFile(uri);
                    tcs.SetResult(filePath);
                }
                else
                {
                    tcs.SetResult(null);
                }
            }
        }

       
    }
}