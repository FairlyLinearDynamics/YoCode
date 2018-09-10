using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace YoCode
{
    internal class Results
    {
        public double FinalScore { get; set; }
        public double MaximumScore { get; set; }
        public double FinalScorePercentage { get; set; }

        public Results(List<FeatureEvidence> list, string jsonFilePath)
        {
            AssignWeightings(list, FeatureWeightingsReader.ReadFromJSON(jsonFilePath));
            CalculateWeightedRatings(list);
            CalculateFinalScoreInPerc();
        }

        private static void AssignWeightings(IReadOnlyCollection<FeatureEvidence> evidenceToBeWeighted, IReadOnlyDictionary<Feature, double> weightingsFromFile)
        {
            foreach (var evidence in evidenceToBeWeighted)
            {
                evidence.FeatureWeighting = weightingsFromFile.ContainsKey(evidence.Feature) ? weightingsFromFile[evidence.Feature] : 0;
            }
        }

        public void CalculateWeightedRatings(List<FeatureEvidence> list)
        {
            ApplySpecialCases(list);
            foreach (var elem in list)
            {
                elem.WeightedRating = Math.Round(elem.FeatureRating * elem.FeatureWeighting, 2);
                MaximumScore += elem.FeatureWeighting;
                FinalScore += elem.WeightedRating;
                elem.FeatureRating = Math.Round(elem.FeatureRating,2);
            }
        }

        private void CalculateFinalScoreInPerc()
        {
            FinalScorePercentage = Math.Round((FinalScore * 100 ) / MaximumScore);
        }

        private static void ApplySpecialCases(List<FeatureEvidence> list)
        {
            AssignToEquivalentCheck(list, Feature.BadInputCheck, Feature.UIBadInputCheck);
            AssignToEquivalentCheck(list, Feature.UnitConverterCheck, Feature.UIConversionCheck);
            AssignToEquivalentCheck(list, Feature.UIFeatureImplemented, Feature.UICodeCheck);
        }

        private static void AssignToEquivalentCheck(List<FeatureEvidence> list, Feature oldCheckFeature, Feature newCheckFeature)
        {
            if (!list.Any(e => e.Feature == oldCheckFeature) || !list.Any(e => e.Feature == newCheckFeature))
            {
                return;
            }

            var newCheck = list.Find(e => e.Feature == newCheckFeature);
            var oldCheck = list.Find(e => e.Feature == oldCheckFeature);

            newCheck.FeatureWeighting = oldCheck.FeatureWeighting;

            if (oldCheck.Inconclusive)
            {
                newCheck.FeatureWeighting = oldCheck.FeatureWeighting * 2 ;
                oldCheck.FeatureWeighting = 0;
            }
        }
    }
}
