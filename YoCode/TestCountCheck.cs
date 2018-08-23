using System;
using System.Collections.Generic;
using System.IO;

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

        private const int TitleColumnFormatter = -25;


        public TestCountCheck(string repositoryPath, FeatureRunner featureRunner)
        {
            workingDir = Path.Combine(repositoryPath, "UnitConverterTests");

            if (!Directory.Exists(workingDir))
            {
                UnitTestEvidence.SetFailed($"{workingDir} not found");
                return;
            }

            this.featureRunner = featureRunner;
            UnitTestEvidence.FeatureTitle = "All unit tests have passed";
            UnitTestEvidence.Feature = Feature.TestCountCheck;
            processName = "dotnet";
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
            StructuredOutput();
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
            double deduction = (TestCountTreshold - stats.totalTests) * (1 / Convert.ToDouble(TestCountTreshold));
            return rating - deduction;
        }

        public void StructuredOutput()
        {
            UnitTestEvidence.GiveEvidence(messages.ParagraphDivider);
            UnitTestEvidence.GiveEvidence(String.Format($"{"Total tests: ",TitleColumnFormatter}{stats.totalTests}"));
            UnitTestEvidence.GiveEvidence(String.Format($"{"Passed:",TitleColumnFormatter}{stats.testsPassed}"));
            UnitTestEvidence.GiveEvidence(String.Format($"{"Failed:",TitleColumnFormatter}{stats.testsFailed}"));
            UnitTestEvidence.GiveEvidence(String.Format($"{"Skipped:",TitleColumnFormatter}{stats.testsSkipped}"));
            UnitTestEvidence.GiveEvidence(String.Format($"{"Percentage:",TitleColumnFormatter}{stats.PercentagePassed}"));
            UnitTestEvidence.GiveEvidence(messages.ParagraphDivider);
            UnitTestEvidence.GiveEvidence(String.Format($"{"Minimum test count:",TitleColumnFormatter}{TestCountTreshold}"));
        }

        public FeatureEvidence UnitTestEvidence { get; } = new FeatureEvidence();
    }
}
