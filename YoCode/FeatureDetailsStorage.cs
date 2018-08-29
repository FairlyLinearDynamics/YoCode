using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace YoCode
{
    internal class FeatureDetailsStorage
    {
        private readonly Dictionary<Feature, double> WeightingsFromJson;
        private readonly TestType mode;

        public FeatureDetailsStorage(TestType mode)
        {
            this.mode = mode;
            WeightingsFromJson = DeserializeJSONFile();
        }

        public Dictionary<Feature, double> DeserializeJSONFile()
        {
            AppSettingsBuilder builder = new AppSettingsBuilder();

            using (StreamReader r = new StreamReader(builder.ReturnPathByMode(mode)))
            {
                string json = r.ReadToEnd();

                return JsonConvert.DeserializeObject<Dictionary<Feature, double>>(json);
            }
        }

        public Dictionary<Feature, FeatureDetails> AssignWeightingsFromJSON(Dictionary<Feature, FeatureDetails> localDictionary)
        {
            return localDictionary.ToDictionary(kv => kv.Key, kv => ReturnFeatureDetails(kv.Value.FeatureTitle, WeightingsFromJson[kv.Key]));
        }

        public Dictionary<Feature, FeatureDetails> InitializeJuniorDetails()
        {
            return new Dictionary<Feature, FeatureDetails>
            {
                {
                    Feature.BadInputCheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "Bad input crashes have been fixed"
                    }
                },

                {
                    Feature.CodeCoverageCheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "Code Coverage"
                    }
                },

                {
                    Feature.DuplicationCheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "Code quality improvement"
                    }
                },
                {
                    Feature.FilesChangedCheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "Files changed"
                    }
                 },

                {
                    Feature.GitCheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "Git was used"
                    }
                },

                {
                    Feature.TestCountCheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "All unit tests have passed"
                    }
                },

                {
                    Feature.UICodeCheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "Evidence present in UI"
                    }
                },

                {
                Feature.UnitConverterCheck,
                new FeatureDetails {
                    FeatureTitle = "Units were converted successfully",
                }
                },

                {
                Feature.UIFeatureImplemeneted,
                new FeatureDetails {
                    FeatureTitle = "Found feature evidence in user interface",
                }
                },

                {
                Feature.UIBadInputCheck,
                new FeatureDetails {
                    FeatureTitle = "Bad input crashes have been fixed in the UI",
                }
                },

                {
                Feature.UIConversionCheck,
                new FeatureDetails {
                    FeatureTitle = "Units were converted successfully using UI",
                }
                },

                {
                    Feature.UnitConverterCheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "Units were converted successfully"
                    }
                }
            };
        }

        public Dictionary<Feature, FeatureDetails> InitializeOriginalDetails()
        {
            return new Dictionary<Feature, FeatureDetails>
            {
                {
                    Feature.BadInputCheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "Bad input crashes have been fixed"
                    }
                },

                {
                    Feature.CodeCoverageCheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "Code Coverage"
                    }
                },

                {
                    Feature.DuplicationCheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "Code quality improvement"
                    }
                },

                {
                    Feature.FilesChangedCheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "Files changed"
                    }
                 },

                {
                Feature.GitCheck,
                new FeatureDetails {
                    FeatureTitle = "Git was used",
                }
                },

                {
                    Feature.TestCountCheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "All unit tests have passed"
                    }
                },

                {
                Feature.UICodeCheck,
                new FeatureDetails {
                    FeatureTitle = "Found feature keyword in UI implementation",
                }
                },

                {
                Feature.UnitConverterCheck,
                new FeatureDetails {
                    FeatureTitle = "Units were converted successfully",
                }
                },

                {
                Feature.UIFeatureImplemeneted,
                new FeatureDetails {
                    FeatureTitle = "Found feature evidence in user interface",
                }
                },

                {
                Feature.UIBadInputCheck,
                new FeatureDetails {
                    FeatureTitle = "Bad input crashes have been fixed in the UI",
                }
                },

                {
                Feature.UIConversionCheck,
                new FeatureDetails {
                    FeatureTitle = "Units were converted successfully using UI",
                }
                },

            };
        }

        public Dictionary<Feature, FeatureDetails> ReturnDetailsByMode(TestType mode)
        {
            return mode == TestType.Junior ? InitializeJuniorDetails() : InitializeOriginalDetails();
        }

        public FeatureDetails ReturnFeatureDetails(string featureTitle, double featureWeighting)
        {
            return new FeatureDetails
            {
                FeatureTitle = featureTitle,
                FeatureWeighting = featureWeighting
            };
        }
    }
}
