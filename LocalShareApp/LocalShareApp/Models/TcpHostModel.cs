using LocalShareApp.ViewModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
            FilePathQueue = new ConcurrentQueue<Tuple<string, string[]>>();
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

        private string currentSendingFileSpeed;

        public string CurrentSendingFileSpeed
        {
            get { return currentSendingFileSpeed; }
            set { SetProperty(ref currentSendingFileSpeed, value, nameof(CurrentSendingFileSpeed)); }
        }

        private string currentReceivingFileSpeed;

        public string CurrentReceivingFileSpeed
        {
            get { return currentReceivingFileSpeed; }
            set { SetProperty(ref currentReceivingFileSpeed, value, nameof(CurrentReceivingFileSpeed)); }
        }



        private string currentReceivingFileName = "";

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

        private double currentReceivingFilePercentage;

        public double CurrentReceivingFilePercentage
        {
            get { return currentReceivingFilePercentage; }
            set { SetProperty(ref currentReceivingFilePercentage, value, nameof(CurrentReceivingFilePercentage)); }
        }




        private ConcurrentQueue<Tuple<string, string[]>> FilePathQueue;



        public void AddFilesToQueue(List<Tuple<string, string[]>> fileList)
        {

            foreach (var path in fileList)
                FilePathQueue.Enqueue(path);
        }

        public Tuple<string, string[]> PopFileFromQueue()
        {
            Tuple<string, string[]> returnValue;
            FilePathQueue.TryDequeue(out returnValue);
            return returnValue;
        }

        public bool IsQueueEmpty()
        {
            return FilePathQueue.IsEmpty;
        }

        public void ResetProperties()
        {

            CurrentSendingFileName = "";
            CurrentSendingFilePercentage = 0;
            CurrentSendingFileName = "";
            CurrentSendingFileSize = "";
            CurrentSendingFileSpeed = "";
        }



    }
}
