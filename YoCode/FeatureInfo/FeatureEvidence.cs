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
        public string Output { get; set; }
        public string ErrorOutput { get; set; }

        public double FeatureRating { get; set; }
        public double FeatureWeighting { get; set; }
        public double WeightedRating { get; set; }

        public string HelperMessage { get; set; }

        public IEvidence Evidence { get; set; } = new SimpleEvidenceBuilder("");

        public void GiveEvidence(IEvidence reason)
        {
            Evidence = reason;
        }

        public void SetInconclusive(IEvidence reason)
        {
            FeatureImplemented = null;
            FeatureWeighting = 0;
            Evidence = reason;
        }

        public void SetFailed(IEvidence reason)
        {
            FeatureImplemented = false;
            Evidence = reason;
        }
    }
}
