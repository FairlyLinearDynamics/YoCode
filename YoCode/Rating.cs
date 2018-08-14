using System;
using System.Collections.Generic;
using System.Text;

namespace YoCode
{
    class Rating
    {
        public double FinalScore { get; set; }

        public Rating(List<FeatureEvidence> list) 
        {
            Console.WriteLine("Rating report: ");

            foreach(var elem in list)
            {
                elem.FeatureRating *= elem.FeatureWeighting;

                Console.WriteLine("Feature: " + elem.FeatureTitle + " Score: " + elem.FeatureRating);
                FinalScore += elem.FeatureRating;   
            }
        }

    }
}
