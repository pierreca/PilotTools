using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ENG.WMOCodes.Downloaders;
using PilotTools.Common;


namespace WeatherData
{
    public class MetarSource : IMetarDecoder, IDataSource
    {
        private Dictionary<string, Metar> Cache = new Dictionary<string, Metar>();

        public async Task<Metar> GetMetarAsync(string icao)    
        {
            return await this.GetMetarAsync(icao, false);
        }

        public async Task<Metar> GetMetarAsync(string icao, bool invalidateCache)
        {
            var ret = new Metar();

            var cacheHit = this.Cache.Keys.Contains(icao);

            if (cacheHit)
            {
                this.Cache[icao] = ret;
            }
            else
            {
                this.Cache.Add(icao, ret);
            }

            var needDownload = !cacheHit
                               || invalidateCache
                               || (cacheHit && !this.Cache[icao].IsValid)
                               || (cacheHit && this.Cache[icao].TimePublished - DateTime.Now < TimeSpan.FromHours(1));

            if (!needDownload)
            {
                ret = this.Cache[icao];
            }
            else
            {
                var retriever = new ENG.WMOCodes.Downloaders.Retrievers.Metar.NoaaGovRetriever();
                var result = await Downloader.DownloadAsync(icao, retriever);
                
                if (!result.IsSuccessful)
                {
                    ret.IsValid = false;
                    ret.MetarObj = null;
                }
                else
                {
                    var decoder = new ENG.WMOCodes.Decoders.MetarDecoder();
                    ENG.WMOCodes.Codes.Metar metar = null;

                    try
                    {
                        metar = decoder.Decode(result.Result);
                    }
                    catch (ENG.WMOCodes.Decoders.Internal.DecodeException)
                    {

                    }

                    ret.Raw = result.Result;

                    if (metar == null)
                    {
                        ret.IsValid = false;
                    }
                    else
                    {
                        ret.IsValid = true;
                        ret.DewPoint = metar.DewPoint;
                        ret.Temperature = metar.Temperature;
                        ret.Visibility = (int)metar.Visibility.Distance;
                        ret.Wind = new Wind();
                        ret.Wind.IsVariable = metar.Wind.IsVariable;
                        if (!metar.Wind.IsVariable)
                        {
                            ret.Wind.Direction = metar.Wind.Direction.Value;
                        }

                        ret.Wind.Speed = metar.Wind.Speed.Value;

                        var layers = new List<CloudLayer>();
                        foreach (var cl in metar.Clouds)
                        {
                            var layer = new CloudLayer();
                            layer.Altitude = cl.Altitude * 100;
                            switch (cl.Type)
                            {
                                case ENG.WMOCodes.Types.Cloud.eType.FEW:
                                    layer.Type = CloudLayerType.FEW;
                                    layer.IsCeiling = false;
                                    break;
                                case ENG.WMOCodes.Types.Cloud.eType.SCT:
                                    layer.Type = CloudLayerType.SCT;
                                    layer.IsCeiling = false;
                                    break;
                                case ENG.WMOCodes.Types.Cloud.eType.BKN:
                                    layer.Type = CloudLayerType.BKN;
                                    layer.IsCeiling = true;
                                    break;
                                case ENG.WMOCodes.Types.Cloud.eType.OVC:
                                    layer.Type = CloudLayerType.OVC;
                                    layer.IsCeiling = true;
                                    break;
                            }

                            layers.Add(layer);
                        }

                        ret.Clouds = layers;
                        ret.MetarObj = metar;

                        ret.ComputeFlightRules();
                    }
                }
            }

            return ret;
        }

        public string Name
        {
            get { return "NOAA Metars"; }
        }

        public DataSourceOrigin Type
        {
            get { return DataSourceOrigin.OnlineOnly; }
        }

        public async Task LoadAsync()
        {
            // Nothing to do here.
            return;
        }

        public async Task DownloadAsync()
        {
            // Nothing to do here.
            return;
        }
    }
}
