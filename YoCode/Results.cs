using System;
using System.Collections.Generic;

namespace YoCode
{
    class Results
    {
        public double FinalScore { get; set; }
        public double MaximumScore { get; set;}

        public Results(List<FeatureEvidence> list,TestType mode)
        {
            var storage = new FeatureDetailsStorage();

            var thisDictionary = storage.ReturnDetailsByMode(mode);

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
            double applyWeighting = 0;

            foreach (var elem in list)
            {
                applyWeighting = (elem.Feature == Feature.BadInputCheck && elem.FeatureRating == 0) ? elem.FeatureWeighting : 0;
                elem.FeatureWeighting = (elem.Feature == Feature.FrontEndCheck) ? applyWeighting : elem.FeatureWeighting;

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

    }
}
