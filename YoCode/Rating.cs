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
                elem.FeatureWeighting = 1;

                elem.FeatureRating = Math.Round((elem.FeatureRating * elem.FeatureWeighting),2);

                Console.WriteLine("Feature: " + elem.FeatureTitle + " Score: " + elem.FeatureRating);
                FinalScore += elem.FeatureRating;   
            }
        }

    }
}
