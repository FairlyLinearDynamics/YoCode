using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace YoCode
{
    internal class ResultSummary
    {
        List<FeatureEvidence> list;

        public double TotalFeatureRating { get; set; }
        public double TotalFeatureWeighting { get; set; }
        public double TotalWeightedRating { get; set; }
        public double FinalScore { get; set; }
        public double PassPerc { get; set; }

        public ResultSummary(List<FeatureEvidence> list)
        {
            this.list = list;
            ResultEvidence.FeatureTitle = "Scoring summary";

            TotalFeatureRating = list.Sum(e => e.FeatureRating);
            TotalFeatureWeighting = list.Sum(e => e.FeatureWeighting);
            TotalWeightedRating = list.Sum(e => e.WeightedRating);

            FinalScore = Math.Round((TotalWeightedRating / TotalFeatureWeighting), 2) * 100;

            CheckAndSetRating();
            ResultEvidence.GiveEvidence(PrintResultTable());
        }

        private void CheckAndSetRating()
        {
            if (FinalScore >= PassPerc)
            {
                ResultEvidence.SetPassed("This person should get an interview!");
                ResultEvidence.FeatureRating = 1;
            }
            else
            {
                ResultEvidence.SetFailed("This person should not get an interview!");
                ResultEvidence.FeatureRating = 0;
            }
        }

        public string PrintResultTable()
        {
            var builder = new StringBuilder();

            builder.AppendLine(String.Format("{0,-50}{1,-15}{2,-15}{3,-15}", "Feature", "Rating", "Weighting","Weighted Rating"));
            builder.AppendLine(messages.ParagraphDivider);

            foreach(var elem in list) {
                builder.AppendLine(String.Format("{0,-50}{1,-15}{2,-15}{3,-15}",elem.FeatureTitle,elem.FeatureRating,elem.FeatureWeighting,elem.WeightedRating));
            }

            builder.AppendLine(messages.ParagraphDivider);
            builder.AppendLine(String.Format("{0,-50}{1,-15}{2,-15}{3,-15}", "Total",TotalFeatureRating, TotalFeatureWeighting,TotalWeightedRating));
            builder.AppendLine(String.Format("{0,-50}{1,-15}","YoScore",FinalScore));

            return builder.ToString();
        }
        public FeatureEvidence ResultEvidence { get; } = new FeatureEvidence();

    }
}
