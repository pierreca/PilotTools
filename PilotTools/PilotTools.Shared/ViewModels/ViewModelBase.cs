using PilotTools.DataSources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace PilotTools.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public ViewModelBase(IDataSourceManager sourceManager)
        {
            this.SourceManager = sourceManager;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected IDataSourceManager SourceManager { get; private set; }

        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if(!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                if(PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
    }
}
