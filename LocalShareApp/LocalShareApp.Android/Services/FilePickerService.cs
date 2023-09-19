using Android.App;
using Android.Content;
using LocalShareApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocalShareApp.Droid.Services
{
    public class FilePickerService : IFilePicker
    {
        private static int FILE_PICKER_REQUEST = 1;

        private TaskCompletionSource<Tuple<string, string[]>> tcs;

        public async Task<Tuple<string, string[]>> PickFiles()
        {
            tcs = new TaskCompletionSource<Tuple<string, string[]>>();

            Intent intent = new Intent(Intent.ActionOpenDocument);
            intent.AddCategory(Android.Content.Intent.CategoryOpenable);
            intent.PutExtra(Intent.ExtraAllowMultiple, true);
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

                    if (null != data)
                    { // checking empty selection
                        List<string> actualPaths = new List<string>();

                        if (null != data.ClipData)
                        { // checking multiple selection or not                           
                            for (int i = 0; i < data.ClipData.ItemCount; i++)
                            {
                                actualPaths.Add(MainActivity.Instance.GetActualPathFromFile(data.ClipData.GetItemAt(i).Uri));
                            }
                        }
                        else
                        {
                            actualPaths.Add(MainActivity.Instance.GetActualPathFromFile(data.Data));
                        }

                        tcs.SetResult(new Tuple<string, string[]>("/", actualPaths.ToArray()));

                    }

                    //Android.Net.Uri uri = data.Data;

                    //string filePath = MainActivity.Instance.GetActualPathFromFile(uri);
                    //tcs.SetResult(filePath);
                }
                else
                {
                    tcs.SetResult(null);
                }
            }
        }


    }
}
