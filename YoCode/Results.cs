using System;
using System.Collections.Generic;

namespace YoCode
{
    class Results
    {
        public double FinalScore { get; set; }

        Dictionary<Feature, FeatureDetails> JuniorTestDetails;
        Dictionary<Feature, FeatureDetails> OriginalTestDetails;

        public Results(List<FeatureEvidence> list)
        {
            InitializeDataStructures();
            AssignWeightings(list, JuniorTestDetails);
            CalculateWeightedRatings(list);
        }

        public void InitializeDataStructures()
        {
            JuniorTestDetails = new Dictionary<Feature, FeatureDetails>();
            OriginalTestDetails = new Dictionary<Feature, FeatureDetails>();

            JuniorTestDetails.Add(
                Feature.BadInputCheck, 
                new FeatureDetails {
                    FeatureTitle = "Bad input crashes have been fixed",
                    FeatureWeighting = 2.39 });

            JuniorTestDetails.Add(
                Feature.CodeCoverageCheck, 
                new FeatureDetails {
                    FeatureTitle = "Code Coverage",
                    FeatureWeighting = 1 });

            JuniorTestDetails.Add(
                Feature.DuplicationCheck, 
                new FeatureDetails {
                    FeatureTitle = "Code quality improvement",
                    FeatureWeighting = 1.4172 });

            JuniorTestDetails.Add(
                Feature.FilesChangedCheck, 
                new FeatureDetails {
                    FeatureTitle = "Files changed",
                    FeatureWeighting = 0 });

            JuniorTestDetails.Add(
                Feature.FrontEndCheck, 
                new FeatureDetails {
                    FeatureTitle = "New feature found in front-end implementation",
                    FeatureWeighting = 1 });

            JuniorTestDetails.Add(
                Feature.GitCheck, 
                new FeatureDetails {
                    FeatureTitle = "Git was used",
                    FeatureWeighting = 1 });

            JuniorTestDetails.Add(
                Feature.ProjectBuilder, 
                new FeatureDetails {
                    FeatureTitle = "Project Build",
                    FeatureWeighting = 1.107 });

            JuniorTestDetails.Add(
                Feature.ProjectRunner, 
                new FeatureDetails {
                    FeatureTitle = "Project Run",
                    FeatureWeighting = 1.033 });

            JuniorTestDetails.Add(
                Feature.SolutionFileExists, 
                new FeatureDetails {
                    FeatureTitle = "Solution File Exists",
                    FeatureWeighting = 0 });

            JuniorTestDetails.Add(
                Feature.TestCountCheck, 
                new FeatureDetails {
                    FeatureTitle = "All unit tests have passed",
                    FeatureWeighting = 1.355 });

            JuniorTestDetails.Add(
                Feature.UICheck, 
                new FeatureDetails {
                    FeatureTitle = "Evidence present in UI",
                    FeatureWeighting = 1.03 });

            JuniorTestDetails.Add(
                Feature.UnitConverterCheck, 
                new FeatureDetails {
                    FeatureTitle = "Units were converted successfully",
                    FeatureWeighting = 1.09 });
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

                Console.WriteLine("Feature: " + elem.FeatureTitle);
                Console.WriteLine("Rating: " + elem.FeatureRating);
                Console.WriteLine("Weighted Rating: " + elem.WeightedRating);
                Console.WriteLine(messages.Divider);
                FinalScore += elem.WeightedRating;
            }
        }

    }
}
