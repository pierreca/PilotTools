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
            ENG.WMOCodes.Codes.Metar metar = null;
            
            try
            {
                metar = decoder.Decode(result.Result);
            }
            catch (ENG.WMOCodes.Decoders.Internal.DecodeException)
            {

            }

            var ret = new Metar();

            if (result.IsSuccessful)
            {
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

            return ret;
        }
    }
}
