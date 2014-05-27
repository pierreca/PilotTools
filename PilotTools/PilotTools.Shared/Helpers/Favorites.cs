using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace PilotTools.Helpers
{
    public static class Favorites
    {
        private const string FavsFileName = "favorites.txt";

        /// <summary>
        /// Save the list of airport codes in the favorites file.
        /// </summary>
        /// <param name="favAirports"></param>
        /// <returns></returns>
        public static async Task SaveAsync(IEnumerable<string> favAirports)
        {
            var roamingFolder = Windows.Storage.ApplicationData.Current.RoamingFolder;
            var favsFile = await roamingFolder.CreateFileAsync(FavsFileName, CreationCollisionOption.ReplaceExisting);

            using (var stream = await favsFile.OpenStreamForWriteAsync())
            {
                using (var writer = new StreamWriter(stream))
                {
                    foreach(var fav in favAirports)
                    {
                        writer.WriteLine(fav);
                    }
                }
            }
        }

        /// <summary>
        /// Get the airport codes stored in the favorites file.
        /// </summary>
        /// <returns>A list of airport codes.</returns>
        public static async Task<IEnumerable<string>> LoadAsync()
        {
            var roamingFolder = Windows.Storage.ApplicationData.Current.RoamingFolder;
            StorageFile favsFile = null;
            bool fileCreationNeeded = false;

            var result = new List<string>();

            try
            {
                favsFile = await roamingFolder.GetFileAsync(FavsFileName);


                using (var stream = await favsFile.OpenStreamForReadAsync())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            if (!string.IsNullOrWhiteSpace(line))
                            {
                                result.Add(line);
                            }
                        }
                    }
                }
            }
            catch (FileNotFoundException fnfex)
            {
                Debug.WriteLine("No favs file. Creating one.");
                fileCreationNeeded = true;
            }

            if (fileCreationNeeded)
            {
                favsFile = await roamingFolder.CreateFileAsync(FavsFileName);
            }

            return result;
        }
    }
}
