using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace YoCode
{
    internal class ResultSummary
    {
        private List<FeatureEvidence> list;

        public double TotalFeatureRating { get; set; }
        public double TotalFeatureWeighting { get; set; }
        public double TotalWeightedRating { get; set; }
        public double FinalScore { get; set; }
        public double PassPerc { get; set; }

        // Formatting

        private const int titleFormatter = -50;
        private const int featureFormatter = -15;

        public ResultSummary(List<FeatureEvidence> list)
        {
            this.list = list;
            ResultEvidence.FeatureTitle = "Scoring summary";

            TotalFeatureRating = list.Sum(e => e.FeatureRating);
            TotalFeatureWeighting = list.Sum(e => e.FeatureWeighting);
            TotalWeightedRating = list.Sum(e => e.WeightedRating);

            FinalScore = Math.Round((TotalWeightedRating / TotalFeatureWeighting), 2) * 100;

            CheckAndSetRating();
            ResultEvidence.GiveEvidence(GetResultTable());
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

        private string GetResultTable()
        {
            var builder = new StringBuilder();

            builder.AppendLine(String.Format($"{"Feature",titleFormatter}{"Rating",featureFormatter}{"Weighting",featureFormatter}{"Weighted Rating",featureFormatter}"));
            builder.AppendLine(messages.ParagraphDivider);

            foreach(var elem in list) {
                builder.AppendLine(String.Format($"{elem.FeatureTitle,titleFormatter}{elem.FeatureRating,featureFormatter}{elem.FeatureWeighting,featureFormatter}{elem.WeightedRating,featureFormatter}"));
            }

            builder.AppendLine(messages.ParagraphDivider);
            builder.AppendLine(String.Format($"{"Total",titleFormatter}{TotalFeatureRating,featureFormatter}{TotalFeatureWeighting,featureFormatter}"));

            builder.AppendLine(String.Format($"{"YoScore",titleFormatter}{FinalScore,featureFormatter}"));

            return builder.ToString();
        }
        public FeatureEvidence ResultEvidence { get; } = new FeatureEvidence();
    }
}
