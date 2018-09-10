using System;
using System.Linq;

namespace YoCode
{

    internal class FeatureEvidence
    {
        public Feature Feature { get; set; }
        public bool Failed => featureImplemented.HasValue && !featureImplemented.Value;
        public bool Passed => featureImplemented.HasValue && featureImplemented.Value;
        public bool Inconclusive => !featureImplemented.HasValue;

        public double FeatureRating { get; set; }
        public double FeatureWeighting { get; set; }
        public double WeightedRating { get; set; }

        public string HelperMessage { get; set; }

        private bool? featureImplemented;
        public IEvidence Evidence { get; private set; } = new SimpleEvidenceBuilder("");

        private void GiveEvidence(IEvidence reason)
        {
            Evidence = reason;
        }

        public void SetInconclusive(IEvidence reason)
        {
            featureImplemented = null;
            FeatureWeighting = 0;
            Evidence = reason;
        }

        public void SetPassed(IEvidence reason)
        {
            featureImplemented = true;
            GiveEvidence(reason);
        }

        public void SetFailed(IEvidence reason)
        {
            featureImplemented = false;
            GiveEvidence(reason);
        }

    }
}
