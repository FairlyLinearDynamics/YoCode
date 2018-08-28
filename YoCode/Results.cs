using System;
using System.Collections.Generic;
using System.Linq;

namespace YoCode
{
    internal class Results
    {
        public double FinalScore { get; set; }
        public double MaximumScore { get; set; }

        public Results(List<FeatureEvidence> list, TestType mode)
        {
            var storage = new FeatureDetailsStorage(mode);

            var thisDictionary = storage.ReturnDetailsByMode(mode);

            storage.DeserializeJSONFile();
            thisDictionary = storage.AssignWeightingsFromJSON(thisDictionary);

            AssignWeightings(list, thisDictionary);
            CalculateWeightedRatings(list);
            CalculateFinalScore();

        }

        public void AssignWeightings(List<FeatureEvidence> list, Dictionary<Feature, FeatureDetails> xTestDetails)
        {
            list.FindAll(e=>e.FeatureImplemented.HasValue).ForEach(e => e.FeatureWeighting = xTestDetails[e.Feature].FeatureWeighting);
        }

        public void CalculateWeightedRatings(List<FeatureEvidence> list)
        {
            ApplySpecialCases(list);

            foreach (var elem in list)
            {
                elem.WeightedRating = Math.Round((elem.FeatureRating * elem.FeatureWeighting), 2);
                MaximumScore += elem.FeatureWeighting;
                FinalScore += elem.WeightedRating;
                elem.FeatureRating = Math.Round(elem.FeatureRating * 100);
            }
        }

        public void CalculateFinalScore()
        {
            FinalScore = Math.Round((FinalScore / MaximumScore) * 100);
        }

        public void ApplySpecialCases(List<FeatureEvidence> list)
        {
            if (list.Count > 1)
            {
                double badInputWeighting = 0;

                badInputWeighting = list.Find(e => e.Feature == Feature.BadInputCheck && e.FeatureRating == 0)?.FeatureWeighting ?? 0;
                list.Find(e => e.Feature == Feature.FrontEndCheck).FeatureWeighting = badInputWeighting;

                var badInputBackEnd = list.Find(e => e.Feature == Feature.BadInputCheck);
                var badInputUI = list.Find(e => e.Feature == Feature.FrontEndCheck);

                if (badInputBackEnd.FeatureImplemented==null && badInputUI.FeatureImplemented == true) 
                {
                    badInputUI.FeatureWeighting = badInputBackEnd.FeatureWeighting;
                    badInputBackEnd.FeatureWeighting = 0;
                }

                var unitConverterCheck = list.Find(e => e.Feature == Feature.UnitConverterCheck);

                IgnoreWeighting(list.Find(e => e.Feature == Feature.UnitConverterCheck));
                IgnoreWeighting(list.Find(e => e.Feature == Feature.UICheck));
            }
        }

        public void IgnoreWeighting(FeatureEvidence evidence)
        {
            if (evidence.FeatureFailed)
            {
                MaximumScore -= evidence.FeatureWeighting;
                evidence.FeatureWeighting = 0;
            }
        }

    }
}
