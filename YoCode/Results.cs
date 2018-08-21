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
                if (elem.Feature == Feature.BadInputCheck && elem.FeatureRating == 0)
                {
                    applyWeighting = elem.FeatureWeighting;
                }
                if(elem.Feature == Feature.FrontEndCheck)
                {
                    elem.FeatureWeighting = applyWeighting;
                }   

                elem.WeightedRating = Math.Round((elem.FeatureRating * elem.FeatureWeighting), 2);
                MaximumScore += elem.FeatureWeighting;

                Console.WriteLine(elem.FeatureTitle + "  " + elem.WeightedRating);

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
