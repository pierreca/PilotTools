using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AirportData.OurAirports
{
    // TODO: LastUpdated field.
    // TODO: Refactor to make it clear this is an implementation specific to ourairports.com
    public class AirportDirectory : IAirportDirectory
    {
        private const string AirportsCSV = "http://ourairports.com/data/airports.csv";
        private const string RunwaysCSV = "http://ourairports.com/data/runways.csv";
        private const string AirportsFileName = "airports.csv";
        private const string RunwaysFileName = "runways.csv";

        private IEnumerable<IAirport> airports;
        private IEnumerable<IRunway> runways;

        /// <summary>
        /// Parse the aiport data from a stream.
        /// </summary>
        /// <param name="data">Stream to parse airports data from.</param>
        /// <returns>An enumeration of airports.</returns>
        private IEnumerable<IAirport> ParseAirportData(Stream data)
        {
            var newAirports = new List<Airport>(50000);

            using (var sr = new StreamReader(data))
            {
                while (!sr.EndOfStream)
                {
                    try
                    {
                        newAirports.Add(Airport.CreateFromString(sr.ReadLine()));

                    }
                    catch (FormatException)
                    {
                        throw new AirportDirectoryException("Impossible to parse an airport from the database");
                    }
                }
            }

            return newAirports;
        }

        private void ParseRunwayData(Stream data)
        {
            using (var sr = new StreamReader(data))
            {
                while (!sr.EndOfStream)
                {
                    try
                    {
                        var newRunway = Runway.CreateFromString(sr.ReadLine());
                        var potentialAirports = this.airports.Where(a => a.Id == newRunway.AirportId);
                        if(potentialAirports != null && potentialAirports.Count() == 1)
                        {
                            var newRunwayList = potentialAirports.First().Runways.ToList();
                            newRunwayList.Add(newRunway);
                            potentialAirports.First().Runways = newRunwayList;
                        }
                        else
                        {
                            throw new AirportDirectoryException("Cannot find the airport corresponding to this runway");
                        }
                    }
                    catch (FormatException)
                    {
                        throw new AirportDirectoryException("Impossible to parse a runway from the database");
                    }
                }
            }
        }

        /// <summary>
        /// Downloads a new database of airports.
        /// </summary>
        public async Task DownloadAndSaveAsync()
        {
            var httpClient = new HttpClient();
            var airportResponse = await httpClient.GetAsync(AirportsCSV);
            var folder = FileSystem.Current.LocalStorage;

            if (airportResponse.IsSuccessStatusCode)
            {
                using (var sr = new StreamReader(await airportResponse.Content.ReadAsStreamAsync()))
                {
                    var file = await folder.CreateFileAsync(AirportsFileName, CreationCollisionOption.ReplaceExisting);
                    await file.WriteAllTextAsync(await sr.ReadToEndAsync());
                }

                var runwaysResponse = await httpClient.GetAsync(RunwaysCSV);
                if (runwaysResponse.IsSuccessStatusCode)
                {
                    using (var sr = new StreamReader(await runwaysResponse.Content.ReadAsStreamAsync()))
                    {
                        var file = await folder.CreateFileAsync(RunwaysFileName, CreationCollisionOption.ReplaceExisting);
                        await file.WriteAllTextAsync(await sr.ReadToEndAsync());
                    }
                }
                else
                {
                    throw new AirportDirectoryException("Could not download runways from OurAirports.com");
                }
            }
            else
            {
                throw new AirportDirectoryException("Could not download airports from OurAirports.com");
            }
        }

        /// <summary>
        /// Get an aiport from the Airports local database (that must have been previously loaded)
        /// </summary>
        /// <param name="ICAO">ICAO identifier of the airport</param>
        /// <returns>The airport corresponding to the ICAO code, or null if none has been found.</returns>
        public IAirport GetAirportData(string ICAO)
        {
            if (this.airports == null)
            {
                throw new InvalidOperationException("Database of airports is empty!");
            }

            var results = this.airports.Where(a => a.ICAO == ICAO);

            if (results == null)
            {
                return null;
            }
            else if (results.Count() == 1)
            {
                return results.First();
            }
            else
            {
                throw new AirportDirectoryException("Multiple airports with the same identifier: database is probably corrupt");
            }
        }

        /// <summary>
        /// Loads Airport data from the local storage.
        /// </summary>
        /// <returns>Task that loads the airport data.</returns>
        public async Task LoadAsync()
        {
            var folder = FileSystem.Current.LocalStorage;
            var file = await folder.GetFileAsync(AirportsFileName);

            using (var s = await file.OpenAsync(FileAccess.Read))
            {
                this.airports = this.ParseAirportData(s);
            }

            file = await folder.GetFileAsync(RunwaysFileName);
            using (var s = await file.OpenAsync(FileAccess.Read))
            {
                this.ParseRunwayData(s);
            }
        }
    }
}
