using PilotTools.Common;
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
        private bool downloadable = false;
        private bool downloading = false;

        public DataSourceViewModel(IDataSourceManager sourceManager, DataSourceContentType sourceContent)
            : base(sourceManager)
        {
            try
            {
                this.dataSource = sourceManager.DataSources[sourceContent];
                this.name = this.dataSource.Name;
                this.Downloadable = this.dataSource.Type == DataSourceOrigin.OnlineWithCache;

                this.Download = new RelayCommand(async arg =>
                    {
                        this.Downloading = true;
                        await this.dataSource.LoadAsync();
                        this.Downloading = false;
                    });
            }
            catch(Exception ex)
            {
                // TODO: Surface this error to the UI.
                Debug.WriteLine("Could not find a data source of type " + sourceContent.ToString() + ": " + ex.Message);
            }
        }

        public bool Downloadable
        {
            get { return this.downloadable; }
            set { this.SetProperty<bool>(ref this.downloadable, value); }
        }

        public bool Downloading
        {
            get { return this.downloading; }
            set { this.SetProperty<bool>(ref this.downloading, value); }
        }

        public string Name
        {
            get { return this.name; }
            set { this.SetProperty<string>(ref this.name, value); }
        }

        public RelayCommand Download { get; set; }
    }
}
