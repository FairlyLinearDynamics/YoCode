﻿using Newtonsoft.Json;
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
                        FeatureTitle = "Bad input crashes have been fixed",
                        FeatureWeighting = 0
                    }
                },

                {
                    Feature.CodeCoverageCheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "Code Coverage",
                        FeatureWeighting = 0
                    }
                },

                {
                    Feature.DuplicationCheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "Code quality improvement",
                        FeatureWeighting = 0
                    }
                },

                {
                    Feature.FrontEndCheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "New feature found in front-end implementation",
                        FeatureWeighting = 0
                    }
                },
                {
                    Feature.FilesChangedCheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "Files changed",
                        FeatureWeighting = 0
                    }
                 },

                {
                    Feature.GitCheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "Git was used",
                        FeatureWeighting = 0
                    }
                },

                {
                    Feature.TestCountCheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "All unit tests have passed",
                        FeatureWeighting = 0
                    }
                },

                {
                    Feature.UICheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "Evidence present in UI",
                        FeatureWeighting = 0
                    }
                },

                {
                    Feature.UnitConverterCheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "Units were converted successfully",
                        FeatureWeighting = 0
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
                        FeatureTitle = "Bad input crashes have been fixed",
                        FeatureWeighting = 0
                    }
                },

                {
                    Feature.CodeCoverageCheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "Code Coverage",
                        FeatureWeighting = 0
                    }
                },

                {
                    Feature.DuplicationCheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "Code quality improvement",
                        FeatureWeighting = 0
                    }
                },

                {
                    Feature.FilesChangedCheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "Files changed",
                        FeatureWeighting = 0
                    }
                 },

                {
                    Feature.FrontEndCheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "New feature found in front-end implementation",
                        FeatureWeighting = 0
                    }
                },

                {
                    Feature.GitCheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "Git was used",
                        FeatureWeighting = 0
                    }
                },

                {
                    Feature.TestCountCheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "All unit tests have passed",
                        FeatureWeighting = 0
                    }
                },

                {
                    Feature.UICheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "Evidence present in UI",
                        FeatureWeighting = 0
                    }
                },

                {
                    Feature.UnitConverterCheck,
                    new FeatureDetails
                    {
                        FeatureTitle = "Units were converted successfully",
                        FeatureWeighting = 0
                    }
                }
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
