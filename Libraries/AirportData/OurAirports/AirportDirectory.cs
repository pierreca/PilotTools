using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using AviationMath;

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
            var newAirports = new List<IAirport>(50000);

            using (var sr = new StreamReader(data))
            {
                sr.ReadLine(); // drop the first line
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

        private IEnumerable<IRunway> ParseRunwayData(Stream data)
        {
            var newRunways = new List<IRunway>(50000);
            using (var sr = new StreamReader(data))
            {
                while (!sr.EndOfStream)
                {
                    try
                    {
                        newRunways.Add(Runway.CreateFromString(sr.ReadLine()));
                    }
                    catch (FormatException)
                    {
                        throw new AirportDirectoryException("Impossible to parse a runway from the database");
                    }
                }
            }

            return newRunways;
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
            IAirport airport;
            if (this.airports == null)
            {
                throw new InvalidOperationException("Database of airports is empty!");
            }

            var results = this.airports.Where(a => string.Compare(a.ICAO, ICAO, StringComparison.CurrentCultureIgnoreCase) == 0);

            if (results == null)
            {
                return null;
            }
            else if (results.Count() == 0)
            {
                throw new AirportDirectoryException("Airport not found");
            }
            else if (results.Count() == 1)
            {
                airport = results.First();
                airport.Runways = this.runways.Where(r => r.AirportId == airport.Id);
                return airport;
            }
            else
            {
                throw new AirportDirectoryException("Multiple airports with the same identifier: database is probably corrupt");
            }
        }

        /// <summary>
        /// Gets a collection of airports within a circle around a position.
        /// </summary>
        /// <param name="position">Center of the circle.</param>
        /// <param name="searchRadius">Radius of the circle in nautical miles.</param>
        /// <returns>A collection of airports within this circle.</returns>
        public async Task<IEnumerable<IAirport>> GetAirportsAroundAsync(BasicGeoposition position, int searchRadius)
        {
            if (this.airports == null)
            {
                throw new InvalidOperationException("Database of airports is empty!");
            }

            var results = await Task.Factory.StartNew<IEnumerable<IAirport>>(() => 
                {
                    var r = from a in this.airports
                            let distance = a.Position.GetDistanceToNM(position)
                            where a.IsPositionValid && distance < searchRadius && a.Type == AirportType.Airport
                            orderby distance ascending
                            select a;

                    return r.ToList();
                });
            
            return results;
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
                this.runways = this.ParseRunwayData(s);
            }
        }
    }
}
