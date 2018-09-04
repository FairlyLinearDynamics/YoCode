using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace YoCode
{
    internal class TestCountCheck : ICheck
    {
        private readonly string processName;
        private readonly string workingDir;
        private readonly string arguments;

        public string StatLine { get; set; }
        public string Output { get; set; }
        public string ErrorOutput { get; set; }

        private const int TestCountThreshold = 10;

        private TestStats stats;
        private List<int> tempStats;

        private const int TitleColumnFormatter = -25;

        public TestCountCheck(ICheckConfig checkConfig)
        {
            workingDir = Path.Combine(checkConfig.PathManager.ModifiedTestDirPath, "UnitConverterTests");

            if (!Directory.Exists(workingDir))
            {
                UnitTestEvidence.SetInconclusive(new SimpleEvidenceBuilder($"{workingDir} not found"));
                return;
            }

            UnitTestEvidence.Feature = Feature.TestCountCheck;
            UnitTestEvidence.HelperMessage = messages.TestCountCheck;
            processName = "dotnet";
            arguments = "test";
            ExecuteTheCheck();
        }

        private void ExecuteTheCheck()
        {
            var pr = new ProcessDetails(processName, workingDir, arguments);
            var evidence = new FeatureRunner().Execute(pr);

            if (evidence.Inconclusive)
            {
                UnitTestEvidence.SetInconclusive(evidence.Evidence);
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

                var featureImplemented = stats.PercentagePassed >= 100 && stats.totalTests > TestCountThreshold;
                if (featureImplemented)
                {
                    UnitTestEvidence.SetPassed(new SimpleEvidenceBuilder(StructuredOutput()));
                }
                else
                {
                    UnitTestEvidence.SetFailed(new SimpleEvidenceBuilder(StructuredOutput()));
                }
            }
            else
            {
                UnitTestEvidence.SetInconclusive(new SimpleEvidenceBuilder("Error while getting tests from applicant's project"));
            }
        }

        public static List<string> GetTestKeyWords()
        {
            return new List<string> { "Total tests:" };
        }

        public double GetTestCountCheckRating()
        {
            double rating = Convert.ToDouble(stats.testsPassed) / stats.totalTests;

            if(stats.totalTests >= TestCountThreshold)
            {
                return rating;
            }
            double deduction = (TestCountThreshold - stats.totalTests) * (1 / Convert.ToDouble(TestCountThreshold));
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
            builder.AppendLine(String.Format($"{"Minimum test count:",TitleColumnFormatter}{TestCountThreshold}"));

            return builder.ToString();
        }

        private FeatureEvidence UnitTestEvidence { get; } = new FeatureEvidence();

        public Task<List<FeatureEvidence>> Execute()
        {
            return Task.FromResult(new List<FeatureEvidence>{UnitTestEvidence});
        }
    }
}
