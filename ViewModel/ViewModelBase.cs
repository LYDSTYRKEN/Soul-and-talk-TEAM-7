using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Soul_and_talk.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}