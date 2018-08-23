using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace YoCode
{
    class FeatureDetailsStorage
    {
        Dictionary<Feature, double> WeightingsFromJson;
        TestType mode;

        string JuniorPath = @"..\..\..\JuniorWeightings.json";
        string OriginalPath =  @"..\..\..\OriginalWeightings.json";

        public FeatureDetailsStorage(TestType mode)
        {
            WeightingsFromJson = InitializeJSONFile();
            this.mode = mode;
        }

        public Dictionary<Feature,double> InitializeJSONFile()
        {
            using (StreamReader r = new StreamReader(ReturnPathByMode(mode)))
            {
                string json = r.ReadToEnd();

                return JsonConvert.DeserializeObject<Dictionary<Feature, double>>(json);

            }
        }

        public void AssignWeightings(Dictionary<Feature,FeatureDetails> localDictionary)
        {
            foreach(var local in localDictionary.ToList())
            {
                foreach(var json in WeightingsFromJson.ToList())
                {
                    if(local.Key == json.Key)
                    {
                        localDictionary[local.Key] = new FeatureDetails
                        {
                            FeatureTitle = local.Value.FeatureTitle,
                            FeatureWeighting = json.Value
                        };
                    }
                }

            }
        }

        public Dictionary<Feature,FeatureDetails> InitializeJuniorDetails()
        {
            var JuniorTestDetails = new Dictionary<Feature, FeatureDetails>();

            JuniorTestDetails.Add(
                Feature.BadInputCheck, 
                new FeatureDetails {
                    FeatureTitle = "Bad input crashes have been fixed",
                    FeatureWeighting = 2.39 });

            JuniorTestDetails.Add(
                Feature.CodeCoverageCheck, 
                new FeatureDetails {
                    FeatureTitle = "Code Coverage",
                    FeatureWeighting = 0 });

            JuniorTestDetails.Add(
                Feature.DuplicationCheck, 
                new FeatureDetails {
                    FeatureTitle = "Code quality improvement",
                    FeatureWeighting = 1.69 });

            JuniorTestDetails.Add(
                Feature.FilesChangedCheck, 
                new FeatureDetails {
                    FeatureTitle = "Files changed",
                    FeatureWeighting = 0 });

            JuniorTestDetails.Add(
                Feature.FrontEndCheck, 
                new FeatureDetails {
                    FeatureTitle = "New feature found in front-end implementation",
                    FeatureWeighting = 0 });

            JuniorTestDetails.Add(
                Feature.GitCheck, 
                new FeatureDetails {
                    FeatureTitle = "Git was used",
                    FeatureWeighting = 1 });

            JuniorTestDetails.Add(
                Feature.ProjectBuilder, 
                new FeatureDetails {
                    FeatureTitle = "Project Build",
                    FeatureWeighting = 1.107 });

            JuniorTestDetails.Add(
                Feature.ProjectRunner, 
                new FeatureDetails {
                    FeatureTitle = "Project Run",
                    FeatureWeighting = 1.033 });

            JuniorTestDetails.Add(
                Feature.SolutionFileExists, 
                new FeatureDetails {
                    FeatureTitle = "Solution File Exists",
                    FeatureWeighting = 0 });

            JuniorTestDetails.Add(
                Feature.TestCountCheck, 
                new FeatureDetails {
                    FeatureTitle = "All unit tests have passed",
                    FeatureWeighting = 1.355 });

            JuniorTestDetails.Add(
                Feature.UICheck, 
                new FeatureDetails {
                    FeatureTitle = "Evidence present in UI",
                    FeatureWeighting = 1.03 });

            JuniorTestDetails.Add(
                Feature.UnitConverterCheck, 
                new FeatureDetails {
                    FeatureTitle = "Units were converted successfully",
                    FeatureWeighting = 1.09 });

            return JuniorTestDetails;
        }
        
        public Dictionary<Feature,FeatureDetails> InitializeOriginalDetails()
        {
            var OriginalTestDetails = new Dictionary<Feature, FeatureDetails>();

            OriginalTestDetails.Add(
                Feature.BadInputCheck, 
                new FeatureDetails {
                    FeatureTitle = "Bad input crashes have been fixed",
                    FeatureWeighting = 2.39 });

            OriginalTestDetails.Add(
                Feature.CodeCoverageCheck, 
                new FeatureDetails {
                    FeatureTitle = "Code Coverage",
                    FeatureWeighting = 0 });

            OriginalTestDetails.Add(
                Feature.DuplicationCheck, 
                new FeatureDetails {
                    FeatureTitle = "Code quality improvement",
                    FeatureWeighting = 1.69 });

            OriginalTestDetails.Add(
                Feature.FilesChangedCheck, 
                new FeatureDetails {
                    FeatureTitle = "Files changed",
                    FeatureWeighting = 0 });

            OriginalTestDetails.Add(
                Feature.FrontEndCheck, 
                new FeatureDetails {
                    FeatureTitle = "New feature found in front-end implementation",
                    FeatureWeighting = 0});

            OriginalTestDetails.Add(
                Feature.GitCheck, 
                new FeatureDetails {
                    FeatureTitle = "Git was used",
                    FeatureWeighting = 1 });

            OriginalTestDetails.Add(
                Feature.ProjectBuilder, 
                new FeatureDetails {
                    FeatureTitle = "Project Build",
                    FeatureWeighting = 1.107 });

            OriginalTestDetails.Add(
                Feature.ProjectRunner, 
                new FeatureDetails {
                    FeatureTitle = "Project Run",
                    FeatureWeighting = 1.033 });

            OriginalTestDetails.Add(
                Feature.SolutionFileExists, 
                new FeatureDetails {
                    FeatureTitle = "Solution File Exists",
                    FeatureWeighting = 0 });

            OriginalTestDetails.Add(
                Feature.TestCountCheck, 
                new FeatureDetails {
                    FeatureTitle = "All unit tests have passed",
                    FeatureWeighting = 1.355 });

            OriginalTestDetails.Add(
                Feature.UICheck, 
                new FeatureDetails {
                    FeatureTitle = "Evidence present in UI",
                    FeatureWeighting = 1.03 });

            OriginalTestDetails.Add(
                Feature.UnitConverterCheck, 
                new FeatureDetails {
                    FeatureTitle = "Units were converted successfully",
                    FeatureWeighting = 1.09 });

            return OriginalTestDetails;
        }

        public Dictionary<Feature,FeatureDetails> ReturnDetailsByMode(TestType mode)
        {
            return mode == TestType.Junior ? InitializeJuniorDetails() : InitializeOriginalDetails();  
        }

        public string ReturnPathByMode(TestType mode)
        {
            return mode == TestType.Junior ? JuniorPath : OriginalPath;
        }




    }
}
