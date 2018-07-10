using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoCode
{
    public class FeatureEvidence
    {
        public bool EvidencePresent => Evidence.Any();

        public List<string> Evidence { get; set; } = new List<string>();

        public void GiveEvidence(string evidence)
        {
            Evidence.Add(evidence);
        }
    }
}
