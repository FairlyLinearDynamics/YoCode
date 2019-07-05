using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YoCode
{
    internal class TestCountCheck : ICheck
    {
        public TestStats Stats { get; set; }

        private readonly Task<List<FeatureEvidence>> projectBuildTask;
        private readonly string processName;
        private readonly string workingDir;
        private readonly string arguments;

        private string Output { get; set; }

        private const int TestCountThreshold = 10;

        private readonly IPathManager pathManager;

        private const int TitleColumnFormatter = -25;

        public TestCountCheck(ICheckConfig checkConfig, Task<List<FeatureEvidence>> projectBuildTask)
        {
            this.projectBuildTask = projectBuildTask;
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

            ProcessResult(Output);
        }

        public void ProcessResult(string output)
        {
            var tempStats = new List<MatchCollection>();
            foreach (var outputLine in output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
            {
                tempStats.AddRange(GetTestKeyWordPatterns().Select(r => r.Matches(outputLine)).Where(m => m.Any()));
            }

            StoreCalculations(tempStats);
        }

        private void StoreCalculations(List<MatchCollection> tempStats)
        {
            try
            {
                Stats = new TestStats
                {
                    totalTests = GetValueForMatch(tempStats, "Total tests:"),
                    testsPassed = GetValueForMatch(tempStats, "Passed:"),
                    testsFailed = GetValueForMatch(tempStats, "Failed:"),
                    testsSkipped = GetValueForMatch(tempStats, "Skipped:")
                };

                UnitTestEvidence.FeatureRating = GetTestCountCheckRating();

                var featureImplemented = Stats.PercentagePassed >= 100 && Stats.totalTests > TestCountThreshold;
                if (featureImplemented)
                {
                    UnitTestEvidence.SetPassed(new SimpleEvidenceBuilder(StructuredOutput()));
                }
                else
                {
                    UnitTestEvidence.SetFailed(new SimpleEvidenceBuilder(StructuredOutput()));
                }
            }
            catch
            {
                UnitTestEvidence.SetInconclusive(new SimpleEvidenceBuilder("Error while getting tests from applicant's project"));
            }
        }

        private int GetValueForMatch(List<MatchCollection> matches, string keyword)
        {
            foreach (var match in matches)
            {
                if (match[0].Groups[1].Value == keyword)
                {
                    return int.Parse(match[0].Groups[2].Value);
                }
            }

            return 0;
        }

        private static IEnumerable<Regex> GetTestKeyWordPatterns()
        {
            return new[] { @"(Total tests:) (\d+)", @"(Passed:) (\d+)", @"(Failed:) (\d+)", @"(Skipped:) (\d+)" }.Select(s => new Regex(s));
        }

        private double GetTestCountCheckRating()
        {
            double rating = Convert.ToDouble(Stats.testsPassed) / Stats.totalTests;

            if(Stats.totalTests >= TestCountThreshold)
            {
                return rating;
            }
            double deduction = (TestCountThreshold - Stats.totalTests) * (1 / Convert.ToDouble(TestCountThreshold));
            return rating - deduction;
        }

        private string StructuredOutput()
        {
            var builder = new StringBuilder();

            builder.AppendLine(messages.ParagraphDivider);
            builder.AppendLine(String.Format($"{"Total tests: ",TitleColumnFormatter}{Stats.totalTests}"));
            builder.AppendLine(String.Format($"{"Passed:",TitleColumnFormatter}{Stats.testsPassed}"));
            builder.AppendLine(String.Format($"{"Failed:",TitleColumnFormatter}{Stats.testsFailed}"));
            builder.AppendLine(String.Format($"{"Skipped:",TitleColumnFormatter}{Stats.testsSkipped}"));
            builder.AppendLine(String.Format($"{"Percentage:",TitleColumnFormatter}{Stats.PercentagePassed}"));
            builder.AppendLine(messages.ParagraphDivider);
            builder.AppendLine(String.Format($"{"Minimum test count:",TitleColumnFormatter}{TestCountThreshold}"));

            if (NumberOfUnfixedTests(GetFileText()) > 0)
            {
                builder.AppendLine(string.Format($"{"Broken tests not fixed:",TitleColumnFormatter}{NumberOfUnfixedTests(GetFileText())}"));
            }

            return builder.ToString();
        }

        private static int NumberOfUnfixedTests(List<string[]> files)
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
            return projectBuildTask.ContinueWith(task =>
            {
                if (!task.Result.All(evidence => evidence.Passed))
                {
                    UnitTestEvidence.SetInconclusive(new SimpleEvidenceBuilder("Project build failed, unable to perform check."));
                    return new List<FeatureEvidence> { UnitTestEvidence };
                }

                if (!Directory.Exists(workingDir))
                {
                    UnitTestEvidence.SetInconclusive(new SimpleEvidenceBuilder($"{workingDir} not found"));
                    return new List<FeatureEvidence> {UnitTestEvidence};
                }

                ExecuteTheCheck();

                return new List<FeatureEvidence> {UnitTestEvidence};
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }
    }
}
