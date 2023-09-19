using System;
using System.Threading.Tasks;

namespace LocalShareApp.Interfaces
{
    public interface IFilePicker
    {
        Task<Tuple<string, string[]>> PickFiles();

    }
}
