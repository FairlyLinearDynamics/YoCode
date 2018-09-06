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
                elem.FeatureRating = Math.Round(elem.FeatureRating,2);
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
                AssignToEquivalentCheck(
                    list.Find(e => e.Feature == Feature.BadInputCheck),
                    list.Find(e => e.Feature == Feature.UIBadInputCheck)
                    );

                AssignToEquivalentCheck(
                    list.Find(e => e.Feature == Feature.UnitConverterCheck),
                    list.Find(e => e.Feature == Feature.UIConversionCheck)
                    );

                AssignToEquivalentCheck(
                    list.Find(e => e.Feature == Feature.UIFeatureImplemented),
                    list.Find(e => e.Feature == Feature.UICodeCheck)
                    );
            }
        }

        public void AssignToEquivalentCheck(FeatureEvidence oldCheck,FeatureEvidence newCheck)
        {
            if (oldCheck.Inconclusive)
            {
                newCheck.FeatureWeighting = oldCheck.FeatureWeighting * 2 ;
                oldCheck.FeatureWeighting = 0;
            }
        }


    }
}
