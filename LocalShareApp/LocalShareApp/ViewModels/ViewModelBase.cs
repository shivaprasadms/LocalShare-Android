using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace LocalShareApp.ViewModels
{
    
        public class ViewModelBase : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
            {
                if (Equals(backingField, value))
                    return false;

                backingField = value;
                OnPropertyChanged(propertyName);
                return true;
            }
        }
    
}
