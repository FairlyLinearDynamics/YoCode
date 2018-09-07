using System.Collections.Generic;

namespace YoCode
{
    internal struct FinalResultsData
    {
        public IEnumerable<FeatureEvidence> featureList;
        public bool isJunior;
        public double finalScore;
        public double finalScorePercentage;
    }
}
