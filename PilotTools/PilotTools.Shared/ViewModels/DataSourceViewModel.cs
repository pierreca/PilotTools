using PilotTools.DataSources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PilotTools.ViewModels
{
    public class DataSourceViewModel : ViewModelBase
    {
        private IDataSource dataSource;
        private string name;

        public DataSourceViewModel(IDataSourceManager sourceManager, DataSourceContent sourceContent)
            : base(sourceManager)
        {
            try
            {
                this.dataSource = sourceManager.DataSources[sourceContent];
                this.name = this.dataSource.Name;

                this.Download = new RelayCommand(async arg => await this.dataSource.LoadAsync());
            }
            catch(Exception ex)
            {
                // TODO: Surface this error to the UI.
                Debug.WriteLine("Could not find a data source of type " + sourceContent.ToString() + ": " + ex.Message);
            }
        }

        public string Name
        {
            get { return this.name; }
            set { this.SetProperty<string>(ref this.name, value); }
        }

        public RelayCommand Download { get; set; }
    }
}
