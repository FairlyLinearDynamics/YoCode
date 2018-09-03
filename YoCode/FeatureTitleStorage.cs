using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace YoCode
{

    static class FeatureTitleStorage
    {
        private static Dictionary<Feature, string> map;

        static FeatureTitleStorage()
        {
            map = InitializeMap();
        }

        public static Dictionary<Feature,string> InitializeMap()
        {
            var map = new Dictionary<Feature, string>();

            map.Add(Feature.AppDuplicationCheck, "Duplication improvement: UnitConverterWebApp");
            map.Add(Feature.TestDuplicationCheck, "Duplication improvement: UnitConverterTests");
            map.Add(Feature.FilesChangedCheck, "Files changed");
            map.Add(Feature.UICodeCheck, "Found feature keyword in UI implementation");
            map.Add(Feature.GitCheck, "Git was used");
            map.Add(Feature.CodeCoverageCheck, "Check Code Coverage");
            map.Add(Feature.TestCountCheck, "All unit tests have passed");
            map.Add(Feature.UIFeatureImplemented, "Found feature evidence in user interface");
            map.Add(Feature.UIBadInputCheck, "Bad input crashes have been fixed in the UI");
            map.Add(Feature.UIConversionCheck, "Units were converted successfully using UI");
            map.Add(Feature.UnitConverterCheck, "Units were converted successfully");
            map.Add(Feature.BadInputCheck, "Bad input crashes have been fixed");

            return map;
        }

        public static string GetFeatureTitle(Feature lookup)
        {
            return map.ToList().Find(e => e.Key == lookup).Value;
        }
    }
}

