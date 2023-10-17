using LocalShareApp.Models;
using System.Threading.Tasks;

namespace LocalShareApp.Interfaces
{
    public interface ILocalShareTransferService
    {
        Task SendToClient(TcpHostModel client, string[] selectedPath, bool isDirectory);
    }
}
