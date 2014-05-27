using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ENG.WMOCodes.Downloaders;


namespace WeatherData
{
    public class MetarDecoder : IMetarDecoder
    {
        public async Task<Metar> GetMetarAsync(string icao)
        {
            var retriever = new ENG.WMOCodes.Downloaders.Retrievers.Metar.NoaaGovRetriever();
            var result = await Downloader.DownloadAsync(icao, retriever);
            var decoder = new ENG.WMOCodes.Decoders.MetarDecoder();
            var metar = decoder.Decode(result.Result);

            var ret = new Metar();

            if (result.IsSuccessful)
            {
                ret.IsValid = true;
                ret.Raw = result.Result;
                ret.MetarObj = metar;
            }

            return ret;
        }
    }
}
