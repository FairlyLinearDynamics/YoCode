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
            foreach(var elem in list)
            {
                foreach(var featuredetail in xTestDetails)
                {
                    if(elem.Feature == featuredetail.Key)
                    {
                        elem.FeatureWeighting = featuredetail.Value.FeatureWeighting;
                    }
                }
            }
        }

        public void CalculateWeightedRatings(List<FeatureEvidence> list)
        {
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

    }
}
