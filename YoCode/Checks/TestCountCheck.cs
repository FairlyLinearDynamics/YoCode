using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

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
            UnitTestEvidence.Feature = Feature.TestCountCheck;
            UnitTestEvidence.HelperMessage = messages.TestCountCheck;
            processName = "dotnet";
            arguments = "test";
            ExecuteTheCheck();
        }

        private void ExecuteTheCheck()
        {
            var pr = new ProcessDetails(processName, workingDir, arguments);
            var evidence = featureRunner.Execute(pr);

            if (evidence.Inconclusive)
            {
                UnitTestEvidence.SetInconclusive(evidence.Evidence.First());
                return;
            }

            Output = evidence.Output;
            ErrorOutput = evidence.ErrorOutput;
            StatLine = Output.GetLineWithAllKeywords(GetTestKeyWords());
            tempStats = StatLine.GetNumbersInALine();
            StoreCalculations(tempStats);
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

                var featureImplemented = stats.PercentagePassed >= 100 && stats.totalTests > TestCountTreshold;
                if (featureImplemented)
                {
                    UnitTestEvidence.SetPassed(StructuredOutput());
                }
                else
                {
                    UnitTestEvidence.SetFailed(StructuredOutput());
                }
            }
            else
            {
                UnitTestEvidence.SetInconclusive("Error while getting tests from applicant's project");
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

        private string StructuredOutput()
        {
            var builder = new StringBuilder();

            builder.AppendLine(messages.ParagraphDivider);
            builder.AppendLine(String.Format($"{"Total tests: ",TitleColumnFormatter}{stats.totalTests}"));
            builder.AppendLine(String.Format($"{"Passed:",TitleColumnFormatter}{stats.testsPassed}"));
            builder.AppendLine(String.Format($"{"Failed:",TitleColumnFormatter}{stats.testsFailed}"));
            builder.AppendLine(String.Format($"{"Skipped:",TitleColumnFormatter}{stats.testsSkipped}"));
            builder.AppendLine(String.Format($"{"Percentage:",TitleColumnFormatter}{stats.PercentagePassed}"));
            builder.AppendLine(messages.ParagraphDivider);
            builder.AppendLine(String.Format($"{"Minimum test count:",TitleColumnFormatter}{TestCountTreshold}"));

            return builder.ToString();
        }

        public FeatureEvidence UnitTestEvidence { get; } = new FeatureEvidence();
    }
}
