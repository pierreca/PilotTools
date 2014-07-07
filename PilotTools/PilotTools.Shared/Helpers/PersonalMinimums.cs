using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace PilotTools.Helpers
{
    [DataContract]
    public class PersonalMinimums
    {
        private const string PersonalMinimumsFile = "minimums.txt";

        [DataMember]
        public int Ceiling { get; set; }

        [DataMember]
        public int Visibility { get; set; }
        [DataMember]
        public int Crosswind { get; set; }
        [DataMember]
        public int RunwayLength { get; set; }
        [DataMember]
        public int RunwayWidth { get; set; }

        public static async Task<PersonalMinimums> LoadAsync()
        {
            var roamingFolder = Windows.Storage.ApplicationData.Current.RoamingFolder;
            var deleteFile = false;
            PersonalMinimums result = new PersonalMinimums();

            try
            {
                var file = await roamingFolder.GetFileAsync(PersonalMinimumsFile);

                using (var stream = await file.OpenStreamForReadAsync())
                {
                    var serializer = new DataContractSerializer(typeof(PersonalMinimums));
                    result = serializer.ReadObject(stream) as PersonalMinimums;                
                }
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("No minimums file.");
            }
            catch (SerializationException)
            {
                Debug.WriteLine("Personal Minimums file corruption detected - deleting.");
                deleteFile = true;
            }

            if (deleteFile)
            {
                try
                {
                    var file = await roamingFolder.GetFileAsync(PersonalMinimumsFile);
                    await file.DeleteAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Could not delete Minimums file: " + ex.Message);
                }
            }

            return result;
        }

        public async Task SaveAsync()
        {
            var roamingFolder = Windows.Storage.ApplicationData.Current.RoamingFolder;
            var minFile = await roamingFolder.CreateFileAsync(PersonalMinimumsFile, CreationCollisionOption.ReplaceExisting);

            using (var stream = await minFile.OpenStreamForWriteAsync())
            {
                var serializer = new DataContractSerializer(typeof(PersonalMinimums));
                serializer.WriteObject(stream, this);
            }
        }
    }
}
