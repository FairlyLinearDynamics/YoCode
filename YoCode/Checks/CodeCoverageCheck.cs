using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace YoCode
{
    internal class CodeCoverageCheck : ICheck
    {
        private readonly CheckConfig checkConfig;
        private const string processName = "dotCover.exe";
        private const string reportName = "report.json";
        private const int passPercentage = 45;
        private const string testFolder = "UnitConverterTests";

        public CodeCoverageCheck(CheckConfig checkConfig)
        {
            this.checkConfig = checkConfig;
        }

        private static string CreateArgument(string dotnetDir, string targetWorkingDir)
        {
            var dotnetExecutablePath = Path.Combine(dotnetDir, "dotnet.exe");

            return $"analyse /TargetExecutable=\"{dotnetExecutablePath}\" /TargetArguments=\"test --no-build\" /TargetWorkingDir=\"{targetWorkingDir}\"" +
                $" /ReportType=\"JSON\" /Output=\"{reportName}\"";
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
            return Task.Run(() => {
                var codeCoverageEvidence = RunCodeCoverage(checkConfig);
                return new List<FeatureEvidence> { codeCoverageEvidence };
            });
        }

        private static FeatureEvidence RunCodeCoverage(CheckConfig checkConfig)
        {
            var codeCoverageEvidence = new FeatureEvidence {Feature = Feature.CodeCoverageCheck, HelperMessage = messages.CodeCoverageCheck};

            var dotCoverDir = checkConfig.RunParameters.DotCoverDir;
            var fullReportPath = Path.Combine(dotCoverDir, reportName);

            var targetWorkingDir = Path.Combine(checkConfig.PathManager.ModifiedTestDirPath, testFolder);

            if (!Directory.Exists(targetWorkingDir))
            {
                codeCoverageEvidence.SetInconclusive(new SimpleEvidenceBuilder($"{testFolder} Directory Not Found"));
                return codeCoverageEvidence;
            }

            var argument = CreateArgument("C:\\Program Files\\dotnet", targetWorkingDir);

            new FeatureRunner().Execute(CreateProcessDetails(argument, processName, dotCoverDir));

            var report = ReadReport(fullReportPath);
            File.Delete(fullReportPath);

            var coverage = GetCodeCoverage(report);

            if (coverage == 0)
            {
                codeCoverageEvidence.SetInconclusive(new SimpleEvidenceBuilder("Code Coverage Not Found"));
            }
            else if (coverage == -1)
            {
                codeCoverageEvidence.SetInconclusive(new SimpleEvidenceBuilder("Code Coverage Not Found"));
            }
            else
            {
                codeCoverageEvidence.FeatureRating = ((double)GetCodeCoverage(report)) / 100;
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

            return codeCoverageEvidence;
        }
    }
}
