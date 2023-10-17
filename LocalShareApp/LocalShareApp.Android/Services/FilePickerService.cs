using Android.App;
using Android.Content;
using LocalShareApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LocalShareApp.Droid.Services
{
    public class FilePickerService : IFilePicker
    {
        private static int FILE_PICKER_REQUEST = 1;
        private static int FOLDER_PICKER_REQUEST = 2;

        private TaskCompletionSource<string[]> tcsFile;
        private TaskCompletionSource<string> tcsFolder;

        public async Task<string[]> PickFiles()
        {
            tcsFile = new TaskCompletionSource<string[]>();

            Intent intent = new Intent(Intent.ActionOpenDocument);
            intent.AddCategory(Android.Content.Intent.CategoryOpenable);
            intent.PutExtra(Intent.ExtraAllowMultiple, true);
            intent.SetType("*/*");

            MainActivity.Instance.StartActivityForResult(intent, FILE_PICKER_REQUEST);

            return await tcsFile.Task;
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

                        tcsFile.SetResult(actualPaths.ToArray());

                    }

                }
                else
                {
                    tcsFile.SetResult(null);
                }
            }
            else if (requestCode == FOLDER_PICKER_REQUEST)
            {
                if (resultCode == Result.Ok)
                {
                    if (data != null)
                    {

                        string path = WebUtility.UrlDecode(data.DataString);
                        string folder = path.Split(":").Last();
                        tcsFolder.SetResult($"/storage/emulated/0/{folder}");
                    }
                    else
                    {
                        tcsFolder.SetResult(null);
                    }
                }
            }
        }

        public async Task<string> PickFolder()
        {
            tcsFolder = new TaskCompletionSource<string>();

            Intent intent2 = new Intent(Intent.ActionOpenDocumentTree);

            intent2.PutExtra("android.content.extra.SHOW_ADVANCED", true);
            intent2.PutExtra("android.content.extra.FANCY", true);
            intent2.PutExtra("android.content.extra.SHOW_FILESIZE", true);


            MainActivity.Instance.StartActivityForResult(intent2, FOLDER_PICKER_REQUEST);

            return await tcsFolder.Task;
        }

        public async Task OpenFolder(string folderPath)
        {
            var rootPath = "content://com.android.externalstorage.documents/document/primary:";
            var savePath = Android.Net.Uri.Parse($"{rootPath}%2flocalshare");

            Intent intent = new Intent(Intent.ActionView);



            intent.SetDataAndType(savePath, "*/*");
            intent.SetFlags(ActivityFlags.NewTask);
            intent.SetPackage("com.google.android.documentsui");


            try
            {
                Application.Context.StartActivity(intent);
            }
            catch (Exception ex)
            {
                // Handle exceptions here (e.g., folderPath does not exist)
            }



        }



    }
}
