using System.Collections.Generic;
using System.Linq;

namespace YoCode
{
    internal class FeatureEvidence
    {
        public string FeatureTitle { get; set; }
        public Feature Feature { get; set; }

        public bool FeatureImplemented { get; set; }
        public bool EvidencePresent => Evidence.Any();
        public string Output { get; set; }
        public string ErrorOutput { get; set; }
        public bool FeatureFailed { get; private set; }

        public double FeatureRating { get; set; }
        public double FeatureWeighting { get; set; }
        public double WeightedRating { get; set; }

        public List<string> Evidence { get; set; } = new List<string>();

        public void GiveEvidence(string evidence)
        {
            Evidence.Add(evidence);
        }

        public void GiveEvidence(params FeatureEvidence[] evidences)
        {
            foreach (var evidence in evidences)
            {
                Evidence.AddRange(evidence.Evidence);
            }
        }

        public void SetFailed(string reason)
        {
            FeatureImplemented = false;
            FeatureFailed = true;
            GiveEvidence(reason);
        }
    }
}
