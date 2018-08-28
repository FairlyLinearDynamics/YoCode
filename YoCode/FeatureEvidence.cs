using System.Collections.Generic;
using System.Linq;

namespace YoCode
{
    internal class FeatureEvidence
    {
        // TODO: Maybe should be 3 methods: SetFailed(reason); SetInconclusive(reason); SetPassed(reason)
        // Each method would set featureImplemented; FeatureRatings and weights; Evidence
        public string FeatureTitle { get; set; }
        public Feature Feature { get; set; }
        public bool? FeatureImplemented { get; set; }
        public bool Failed => FeatureImplemented.HasValue && !FeatureImplemented.Value;
        public bool Passed => FeatureImplemented.HasValue && FeatureImplemented.Value;
        public bool Inconclusive => !FeatureImplemented.HasValue;
        public bool EvidencePresent => Evidence.Any();
        public string Output { get; set; }
        public string ErrorOutput { get; set; }

        public double FeatureRating { get; set; }
        public double FeatureWeighting { get; set; }
        public double WeightedRating { get; set; }

        public List<string> Evidence { get; set; } = new List<string>();

        public void GiveEvidence(string evidence)
        {
            Evidence.Add(evidence);
        }

        public void SetInconclusive(string reason)
        {
            FeatureImplemented = null;
            FeatureWeighting = 0;
            GiveEvidence(reason);
        }

        public void SetFailed(string reason)
        {
            FeatureImplemented = false;
            GiveEvidence(reason);
        }
    }
}
