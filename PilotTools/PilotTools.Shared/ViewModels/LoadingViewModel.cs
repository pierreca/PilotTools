using PilotTools.DataSources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PilotTools.ViewModels
{
    public class LoadingViewModel : ViewModelBase
    {
        private string loadingPhase;
        public string LoadingPhase
        {
            get { return this.loadingPhase; }
            set { this.SetProperty<string>(ref this.loadingPhase, value); }
        }

        public LoadingViewModel(IDataSourceManager sourceManager)
            : base(sourceManager)
        {

        }

        public async Task StartLoading()
        {
            foreach(var s in this.SourceManager.DataSources.Values)
            {
                this.LoadingPhase = s.Name;
                await s.LoadAsync();
            }

            this.LoadingPhase = "All Done!";
        }
    }
}
