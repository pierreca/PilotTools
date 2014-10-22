using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using PilotTools.Helpers;
using System.Threading.Tasks;
using PilotTools.DataSources;

namespace PilotTools.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private ObservableCollection<DataSourceViewModel> dataSources;
        private PersonalMinimums minimums;

        public SettingsViewModel(IDataSourceManager sourceManager)
            : base (sourceManager)
        { }

        public ObservableCollection<DataSourceViewModel> DataSources
        {
            get { return this.dataSources; }
            set { this.SetProperty<ObservableCollection<DataSourceViewModel>>(ref this.dataSources, value); }
        }

        public PersonalMinimums Minimums
        {
            get { return this.minimums; }
            set { this.SetProperty<PersonalMinimums>(ref this.minimums, value); }
        }

        public async Task LoadAsync()
        {
            this.DataSources = new ObservableCollection<DataSourceViewModel>(App.DataSourceManager.DataSources.Select(s => new DataSourceViewModel(App.DataSourceManager, s.Key)));
            this.Minimums = await PersonalMinimums.LoadAsync();
        }

        public async Task SaveSettings()
        {
            await this.Minimums.SaveAsync();
        }
    }
}
