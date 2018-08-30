using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace YoCode
{
    internal static class FeatureWeightingsReader
    {
        public static Dictionary<Feature, double> ReadFromJSON(string jsonFilePath)
        {
            using (StreamReader r = new StreamReader(jsonFilePath))
            {
                string json = r.ReadToEnd();

                return JsonConvert.DeserializeObject<Dictionary<Feature, double>>(json);
            }
        }
    }
}
