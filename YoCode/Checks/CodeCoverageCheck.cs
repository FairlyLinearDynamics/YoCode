using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace YoCode
{
    internal class CodeCoverageCheck : ICheck
    {
        private readonly CheckConfig checkConfig;
        private readonly Task<List<FeatureEvidence>> projectBuildTask;
        private const string processName = "dotCover.exe";
        private const int passPercentage = 45;
        private const string testFolder = "UnitConverterTests";

        public CodeCoverageCheck(CheckConfig checkConfig, Task<List<FeatureEvidence>> projectBuildTask)
        {
            this.checkConfig = checkConfig;
            this.projectBuildTask = projectBuildTask;
        }

        private static string CreateArgument(string dotnetDir, string targetWorkingDir, string outputFilePath)
        {
            var dotnetExecutablePath = Path.Combine(dotnetDir, "dotnet.exe");

            return $"analyse /TargetExecutable=\"{dotnetExecutablePath}\" /TargetArguments=\"test --no-build\" /TargetWorkingDir=\"{targetWorkingDir}\"" +
                $" /ReportType=\"JSON\" /Output=\"{outputFilePath}\"";
        }

        private static ProcessDetails CreateProcessDetails(string arguments, string processName, string dotCoverDir)
        {
            var processPath = Path.Combine(dotCoverDir, processName);

            return new ProcessDetails(processPath, dotCoverDir, arguments);
        }

        private static string ReadReport(string fullReportPath)
        {
            if (File.Exists(fullReportPath))
            {
                using (StreamReader sr = File.OpenText(fullReportPath))
                {
                    return sr.ReadToEnd();
                }
            }
            else
            {
                return "";
            }
        }

        private static int GetCodeCoverage(string json)
        {
            return DotCover.CalculateCoverageFromJsonReport(json);
        }

        public Task<List<FeatureEvidence>> Execute()
        {
            return projectBuildTask.ContinueWith(task => {
                if (!task.Result.All(evidence => evidence.Passed))
                {
                    var dependencyFailedEvidence = new FeatureEvidence { Feature = Feature.CodeCoverageCheck, HelperMessage = messages.CodeCoverageCheck };
                    dependencyFailedEvidence.SetInconclusive(new SimpleEvidenceBuilder("Project build failed, unable to perform check."));
                    return new List<FeatureEvidence> { dependencyFailedEvidence };
                }

                var codeCoverageEvidence = RunCodeCoverage(checkConfig);
                return new List<FeatureEvidence> { codeCoverageEvidence };
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        private static FeatureEvidence RunCodeCoverage(CheckConfig checkConfig)
        {
            var codeCoverageEvidence = new FeatureEvidence {Feature = Feature.CodeCoverageCheck, HelperMessage = messages.CodeCoverageCheck};

            var dotCoverDir = checkConfig.RunParameters.DotCoverDir;
            var fullReportPath = Path.GetTempFileName();

            var targetWorkingDir = Path.Combine(checkConfig.PathManager.ModifiedTestDirPath, testFolder);

            if (!Directory.Exists(targetWorkingDir))
            {
                codeCoverageEvidence.SetInconclusive(new SimpleEvidenceBuilder($"{testFolder} Directory Not Found"));
                return codeCoverageEvidence;
            }

            var argument = CreateArgument("C:\\Program Files\\dotnet", targetWorkingDir, fullReportPath);

            new FeatureRunner().Execute(CreateProcessDetails(argument, processName, dotCoverDir));

            var report = ReadReport(fullReportPath);
            File.Delete(fullReportPath);

            try
            {

                var coverage = GetCodeCoverage(report);
                codeCoverageEvidence.FeatureRating = coverage / 100.0;
                var featureImplemented = coverage >= passPercentage;
                var evidence = $"Code Coverage: {coverage}%";
                if (featureImplemented)
                {
                    codeCoverageEvidence.SetPassed(new SimpleEvidenceBuilder(evidence));
                }
                else
                {
                    codeCoverageEvidence.SetFailed(new SimpleEvidenceBuilder(evidence));
                }
            
            }
            catch(Exception e)
            {
                codeCoverageEvidence.SetInconclusive(new SimpleEvidenceBuilder(e.ToString()));
            }

            return codeCoverageEvidence;
        }
    }
}
