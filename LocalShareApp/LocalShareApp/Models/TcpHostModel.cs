using LocalShareApp.ViewModels;
using System.Net.Sockets;

namespace LocalShareApp.Models
{
    public class TcpHostModel : ViewModelBase
    {

        public TcpHostModel(string hostName, string hostIp, TcpClient host)
        {
            HostPcName = hostName;
            HostPcIP = hostIp;
            Host = host;
        }


        public string HostPcName { get; set; }

        public string HostPcIP { get; set; }

        public TcpClient Host { get; private set; }



        private bool isSendingFile = false;

        public bool IsSendingFile
        {
            get { return isSendingFile; }
            set
            {
                if (isSendingFile != value)
                {
                    SetProperty(ref isSendingFile, value, nameof(IsSendingFile));
                }
            }
        }

        private bool isReceivingFile = false;

        public bool IsReceivingFile
        {
            get { return isReceivingFile; }
            set
            {
                if (isReceivingFile != value)
                {
                    SetProperty(ref isReceivingFile, value, nameof(IsReceivingFile));
                }
            }
        }


        private string currentSendingFileName = "";


        public string CurrentSendingFileName
        {
            get { return currentSendingFileName; }
            set
            {
                if (currentSendingFileName != value)
                {
                    SetProperty(ref currentSendingFileName, value, nameof(CurrentSendingFileName));
                }
            }
        }

        private string currentReceivingFileName = "nyll";

        public string CurrentReceivingFileName
        {
            get { return currentReceivingFileName; }
            set
            {
                if (currentReceivingFileName != value)
                {
                    SetProperty(ref currentReceivingFileName, value, nameof(CurrentReceivingFileName));
                }
            }
        }

        private string currentSendingFileSize = "";

        public string CurrentSendingFileSize
        {
            get { return currentSendingFileSize; }
            set
            {
                if (currentSendingFileSize != value)
                {
                    SetProperty(ref currentSendingFileSize, value, nameof(CurrentSendingFileSize));
                }
            }
        }


        private string currentReceivingFileSize = "";

        public string CurrentReceivingFileSize
        {
            get { return currentReceivingFileSize; }
            set
            {
                if (currentReceivingFileSize != value)
                {
                    SetProperty(ref currentReceivingFileSize, value, nameof(CurrentReceivingFileSize));
                }
            }
        }

        private double currentSendingFilePercentage;

        public double CurrentSendingFilePercentage
        {
            get { return currentSendingFilePercentage; }
            set { SetProperty(ref currentSendingFilePercentage, value, nameof(CurrentSendingFilePercentage)); }
        }









    }
}
