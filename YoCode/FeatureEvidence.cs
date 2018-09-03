using System.Collections.Generic;
using System.Linq;

namespace YoCode
{
    internal class FeatureEvidence
    {
        public string FeatureTitle { get; set; }
        public Feature Feature { get; set; }
        public bool Failed => featureImplemented.HasValue && !featureImplemented.Value;
        public bool Passed => featureImplemented.HasValue && featureImplemented.Value;
        public bool Inconclusive => !featureImplemented.HasValue;
        public bool EvidencePresent => Evidence.Any();
        public string Output { get; set; }
        public string ErrorOutput { get; set; }

        public double FeatureRating { get; set; }
        public double FeatureWeighting { get; set; }
        public double WeightedRating { get; set; }

        public string HelperMessage { get; set; }

        public List<string> Evidence { get; } = new List<string>();

        private bool? featureImplemented;

        public void GiveEvidence(string evidence)
        {
            Evidence.Add(evidence);
        }

        public void SetInconclusive(params string[] reasons)
        {
            featureImplemented = null;
            FeatureWeighting = 0;
            reasons.ToList().ForEach(GiveEvidence);
        }

        public void SetPassed(string reason)
        {
            featureImplemented = true;
            GiveEvidence(reason);
        }

        public void SetFailed(string reason)
        {
            featureImplemented = false;
            GiveEvidence(reason);
        }
    }
}
