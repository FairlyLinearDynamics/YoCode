using System;
using System.Collections.Generic;
using System.Linq;
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
        public string ErrorOutput { get; set; }

        private readonly int TestCountTreshold = 10;

        private TestStats stats;
        private List<int> tempStats;

        private const int TitleColumnFormatter = -25;

        public TestCountCheck(string repositoryPath, FeatureRunner featureRunner)
        {
            workingDir = Path.Combine(repositoryPath, "UnitConverterTests");

            if (!Directory.Exists(workingDir))
            {
                UnitTestEvidence.SetInconclusive($"{workingDir} not found");
                return;
            }

            this.featureRunner = featureRunner;
            UnitTestEvidence.FeatureTitle = "All unit tests have passed";
            UnitTestEvidence.Feature = Feature.TestCountCheck;
            UnitTestEvidence.HelperMessage = messages.TestCountCheck;
            processName = "dotnet";
            arguments = "test";
            ExecuteTheCheck();
        }

        public void ExecuteTheCheck()
        {
            var pr = new ProcessDetails(processName, workingDir, arguments);
            var evidence = featureRunner.Execute(pr);

            if (evidence.FeatureImplemented == null)
            {
                UnitTestEvidence.SetInconclusive(evidence.Evidence.First());
                return;
            }

            Output = evidence.Output;
            ErrorOutput = evidence.ErrorOutput;
            StatLine = Output.GetLineWithAllKeywords(GetTestKeyWords());
            tempStats = StatLine.GetNumbersInALine();
            StoreCalculations(tempStats);

            if (UnitTestEvidence.FeatureImplemented == null)
                return;

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

                UnitTestEvidence.FeatureImplemented = stats.PercentagePassed == 100 && stats.totalTests > TestCountTreshold;
            }
            else
            {
                UnitTestEvidence.SetInconclusive("Error while getting tests from applicant's project");
            }
        }

        private string BuildErrorOutput()
        {
            return $"Error Running Tests: {ErrorOutput}";
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
