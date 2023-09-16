using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LocalShareApp.Interfaces
{
    public interface IMessageService
    {
        Task ShowAsync(string message);
    }
}
