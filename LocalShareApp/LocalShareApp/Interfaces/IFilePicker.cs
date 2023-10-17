using System.Threading.Tasks;

namespace LocalShareApp.Interfaces
{
    public interface IFilePicker
    {
        Task<string[]> PickFiles();

        Task<string> PickFolder();

        Task OpenFolder(string folderPath);

    }
}
