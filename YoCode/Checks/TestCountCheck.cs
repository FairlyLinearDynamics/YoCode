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
        private readonly IPathManager pathManager;

        private const int TitleColumnFormatter = -25;

        public TestCountCheck(ICheckConfig checkConfig)
        {
            workingDir = Path.Combine(checkConfig.PathManager.ModifiedTestDirPath, "UnitConverterTests");
            pathManager = checkConfig.PathManager;

            UnitTestEvidence.Feature = Feature.TestCountCheck;
            UnitTestEvidence.HelperMessage = messages.TestCountCheck;
            processName = "dotnet";
            arguments = "test --no-build";
        }

        private void ExecuteTheCheck()
        {
            var pr = new ProcessDetails(processName, workingDir, arguments);
            var processOutput = new FeatureRunner().Execute(pr);
            

            if (processOutput.Output == null)
            {
                UnitTestEvidence.SetInconclusive(UnitTestEvidence.Evidence);
                return;
            }

            Output = processOutput.Output;
            ErrorOutput = processOutput.ErrorOutput;
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

            if (NumberOfUnfixedTests(GetFileText()) > 0)
            {
                builder.AppendLine(string.Format($"{"Broken tests not fixed:",TitleColumnFormatter}{NumberOfUnfixedTests(GetFileText())}"));
            }

            return builder.ToString();
        }

        public static int NumberOfUnfixedTests(List<string[]> files)
        {
            var unfixedTests = 0;

            var keywords = new List<string>()
            {
                "[Theory]",
                "[InlineData("
            };

            foreach (var file in files)
            {
                for (int i = 1; i < file.Length; i++)
                {
                    if (file[i].Contains(keywords[1]) && !file[i - 1].ContainsAny(keywords))
                    {
                        unfixedTests++;
                    }
                }
            }
            return unfixedTests;
        }

        private List<string[]> GetFileText()
        {
            var csUris = pathManager.GetFilesInDirectory(pathManager.ModifiedTestDirPath, FileTypes.cs);

            List<string[]> fileText = new List<string[]>();

            csUris.Where(a => a.Contains("UnitConverterTests")).ToList().ForEach(i => fileText.Add(File.ReadAllLines(i)));

            return fileText;
        }

        private FeatureEvidence UnitTestEvidence { get; } = new FeatureEvidence();

        public Task<List<FeatureEvidence>> Execute()
        {
            return Task.Run(() =>
            {
                if (!Directory.Exists(workingDir))
                {
                    UnitTestEvidence.SetInconclusive(new SimpleEvidenceBuilder($"{workingDir} not found"));
                    return new List<FeatureEvidence> {UnitTestEvidence};
                }

                ExecuteTheCheck();

                return new List<FeatureEvidence> {UnitTestEvidence};
            });
        }
    }
}
