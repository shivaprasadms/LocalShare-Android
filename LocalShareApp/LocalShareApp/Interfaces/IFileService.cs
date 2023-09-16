using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LocalShareApp.Interfaces
{
    public interface IFileService
    {
        Task WriteFileAsync(string fileName, string content);
    }
}
