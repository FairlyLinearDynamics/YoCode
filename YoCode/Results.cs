using System;
using System.Collections.Generic;
using System.Linq;

namespace YoCode
{
    class Results
    {
        public double FinalScore { get; set; }
        public double MaximumScore { get; set;}

        public Results(List<FeatureEvidence> list,TestType mode)
        {
            var storage = new FeatureDetailsStorage(mode);

            var thisDictionary = storage.ReturnDetailsByMode(mode);

            storage.InitializeJSONFile();
            storage.AssignWeightingsFromJSON(thisDictionary);

            AssignWeightings(list, thisDictionary);
            CalculateWeightedRatings(list);
            CalculateFinalScore();

        }

        public void AssignWeightings(List<FeatureEvidence> list,Dictionary<Feature,FeatureDetails> xTestDetails)
        {
            list.ForEach(e => e.FeatureWeighting = xTestDetails[e.Feature].FeatureWeighting);
        }

        public void CalculateWeightedRatings(List<FeatureEvidence> list)
        {
            ApplySpecialCases(list);

            foreach (var elem in list)
            {
                elem.WeightedRating = Math.Round((elem.FeatureRating * elem.FeatureWeighting), 2);

                Console.WriteLine(elem.FeatureWeighting);

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
            double badInputWeighting = 0;

            badInputWeighting = list.Find(e => e.Feature == Feature.BadInputCheck && e.FeatureRating == 0)?.FeatureWeighting ?? 0;
            list.Find(e => e.Feature == Feature.FrontEndCheck).FeatureWeighting = badInputWeighting;

            var unitConverterCheck = list.Find(e => e.Feature == Feature.UnitConverterCheck);

            if (unitConverterCheck.FeatureFailed)
            {
                MaximumScore -= unitConverterCheck.FeatureWeighting;
                unitConverterCheck.FeatureWeighting = 0;
            }

        }
    }
}
