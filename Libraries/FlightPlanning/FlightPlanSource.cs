﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PilotTools.Common;
using AirportData;
using Windows.Storage;

namespace FlightPlanning
{
    public class FlightPlanSource : IDataSource
    {
        private const string DisplayName = "Flight Plans";
        private const string FileName = "FlightPlans.txt";

        private IAirportDirectory airportDirectory;


        public IEnumerable<FlightPlan> FlightPlans { get; set; } 

        public FlightPlanSource(IAirportDirectory directory)
        {
            this.airportDirectory = directory;
        }

        public string Name
        {
            get { return FlightPlanSource.DisplayName; }
        }

        public DataSourceOrigin Type
        {
            get { return DataSourceOrigin.LocalOnly; }
        }

        public async Task LoadAsync()
        {
            var roamingFolder = Windows.Storage.ApplicationData.Current.RoamingFolder;
            StorageFile fpFile = null;
            bool fileCreationNeeded = false;

            var plans = new List<FlightPlan>();

            try
            {
                fpFile = await roamingFolder.GetFileAsync(FlightPlanSource.FileName);

                using (var stream = await fpFile.OpenStreamForReadAsync())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            if (!string.IsNullOrWhiteSpace(line))
                            {
                                plans.Add(FlightPlan.FromString(line, this.airportDirectory));
                            }
                        }
                    }
                }

                this.FlightPlans = plans;
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("No flight plans file. Creating one.");
                fileCreationNeeded = true;
            }

            if (fileCreationNeeded)
            {
                fpFile = await roamingFolder.CreateFileAsync(FlightPlanSource.FileName);
            }
        }

        public Task DownloadAsync()
        {
            throw new InvalidOperationException("This is a local only data source");
        }

        public async Task SaveAsync()
        {
            var roamingFolder = Windows.Storage.ApplicationData.Current.RoamingFolder;
            var fpFile = await roamingFolder.CreateFileAsync(FlightPlanSource.FileName, CreationCollisionOption.ReplaceExisting);

            using (var stream = await fpFile.OpenStreamForWriteAsync())
            {
                using (var writer = new StreamWriter(stream))
                {
                    foreach (var fp in this.FlightPlans)
                    {
                        writer.WriteLine(fp.ToString());
                    }
                }
            }
        }
    }
}
