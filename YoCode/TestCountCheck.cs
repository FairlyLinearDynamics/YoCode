using System;
using System.Collections.Generic;

namespace YoCode
{
    internal class TestCountCheck
    {
        private readonly string processName;
        private readonly string workingDir;
        private readonly string arguments;
        private readonly FeatureRunner featureRunner;

        public string StatLine { get; set; }
        public string Output { get; set; }

        private readonly int TestCountTreshold = 10;

        private TestStats stats;
        private List<int> tempStats;

        public TestCountCheck(string repositoryPath, FeatureRunner featureRunner)
        {
            this.featureRunner = featureRunner;
            UnitTestEvidence.FeatureTitle = "All unit tests have passed";
            processName = "dotnet";
            workingDir = repositoryPath;
            arguments = "test";
            ExecuteTheCheck();
        }

        public void ExecuteTheCheck()
        {
            var pr = new ProcessDetails(processName, workingDir, arguments);
            var evidence = featureRunner.Execute(pr);
            if (evidence.FeatureFailed)
            {
                return;
            }

            Output = evidence.Output;
            StatLine = Output.GetLineWithAllKeywords(GetTestKeyWords());
            tempStats = StatLine.GetNumbersInALine();
            StoreCalculations(tempStats);

            UnitTestEvidence.FeatureImplemented = stats.PercentagePassed == 100 && stats.totalTests > TestCountTreshold;
            UnitTestEvidence.GiveEvidence(StatLine);
            UnitTestEvidence.GiveEvidence("Percentage: "+ (stats.PercentagePassed).ToString());
            UnitTestEvidence.GiveEvidence("Minimum test count: " + TestCountTreshold);
        }

        public void StoreCalculations(List<int> tempStats)
        {
            if(tempStats.Count == 4)
            {
                stats.totalTests = tempStats[0];
                stats.testsPassed = tempStats[1];
                stats.testsFailed = tempStats[2];
                stats.testsSkipped = tempStats[3];    
                UnitTestEvidence.FeatureRating = GetTestCountCheckRating();

            }
            else
            {
                UnitTestEvidence.SetFailed("Couldn't get information about tests");
                UnitTestEvidence.FeatureRating = 0;
            }
        }

        public static List<string> GetTestKeyWords()
        {
            return new List<string> { "Total tests:" };
        }

        public double GetTestCountCheckRating()
        {
            double rating = Convert.ToDouble(stats.testsPassed) / stats.totalTests;

            if(stats.totalTests >= TestCountTreshold)
            {
                return rating;
            }
            else
            {
                double deduction = (TestCountTreshold - stats.totalTests) * (1 / Convert.ToDouble(TestCountTreshold));
                return rating - deduction;
            }
        }

        public FeatureEvidence UnitTestEvidence { get; } = new FeatureEvidence();
    }
}
