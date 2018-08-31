using System;
using System.Collections.Generic;
using System.Linq;

namespace YoCode
{
    internal class Results
    {
        public double FinalScore { get; set; }
        public double MaximumScore { get; set; }

        public Results(List<FeatureEvidence> list, string jsonFilePath)
        {
            AssignWeightings(list, FeatureWeightingsReader.ReadFromJSON(jsonFilePath));
            CalculateWeightedRatings(list);
            CalculateFinalScore();
        }

        private static void AssignWeightings(List<FeatureEvidence> list, IReadOnlyDictionary<Feature, double> xTestDetails)
        {
            list.ForEach(e => e.FeatureWeighting = xTestDetails[e.Feature]);
        }

        public void CalculateWeightedRatings(List<FeatureEvidence> list)
        {
            ApplySpecialCases(list);

            foreach (var elem in list)
            {
                elem.WeightedRating = Math.Round(elem.FeatureRating * elem.FeatureWeighting, 2);
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
                var badInputBackEnd = list.Find(e => e.Feature == Feature.BadInputCheck);
                var badInputUI = list.Find(e => e.Feature == Feature.UIBadInputCheck);

                if (badInputBackEnd.Inconclusive) 
                {
                    badInputUI.FeatureWeighting = badInputBackEnd.FeatureWeighting;
                    badInputBackEnd.FeatureWeighting = 0;
                }

                CheckAndIgnoreWeighting(list.Find(e => e.Feature == Feature.UnitConverterCheck));
                CheckAndIgnoreWeighting(list.Find(e => e.Feature == Feature.UICodeCheck));
            }
        }

        public void CheckAndIgnoreWeighting(FeatureEvidence evidence)
        {
            if (evidence.Inconclusive)
            {
                evidence.FeatureWeighting = 0;
            }
        }
    }
}
