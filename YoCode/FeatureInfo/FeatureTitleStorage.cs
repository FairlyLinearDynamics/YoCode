using System.Collections.Generic;

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

            map.Add(Feature.SolutionFileExists, "Solution File Exists");
            map.Add(Feature.AppDuplicationCheck, "Duplication improvement: UnitConverterWebApp");
            map.Add(Feature.TestDuplicationCheck, "Duplication improvement: UnitConverterTests");
            map.Add(Feature.FilesChangedCheck, "Files changed");
            map.Add(Feature.UICodeCheck, "New Feature Found: Back End");
            map.Add(Feature.GitCheck, "Git was used");
            map.Add(Feature.CodeCoverageCheck, "Code Coverage");
            map.Add(Feature.TestCountCheck, "Unit Tests Passed");
            map.Add(Feature.UIFeatureImplemented, "New Feature Found: Front End");
            map.Add(Feature.UIBadInputCheck, "Bad Input Crash Fix: Front End");
            map.Add(Feature.UIConversionCheck, "Unit Conversion: Front End");
            map.Add(Feature.UnitConverterCheck, "Unit Conversion: Back End");
            map.Add(Feature.BadInputCheck, "Bad Input Crash Fix: Back End");
            map.Add(Feature.ProjectBuilder, "Does the project build?");
            map.Add(Feature.ProjectRunner, "Does the project run?");
            return map;
        }

        public static string GetFeatureTitle(Feature lookup)
        {
            return map[lookup];
        }
    }
}

