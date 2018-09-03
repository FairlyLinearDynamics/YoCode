using System.IO;

namespace YoCode
{
    internal class CodeCoverageCheck
    {
        private string Argument { get; }
        private string ProcessName { get; } = "dotCover.exe";

        private string ReportName { get; } = "report.json";
        private string FullReportPath { get; }
        private const int passPerc = 45;
        private const string testFolder = "UnitConverterTests";

        public CodeCoverageCheck(string dotCoverDir, string workingDir, FeatureRunner featureRunner)
        {
            CodeCoverageEvidence.FeatureTitle = "Code Coverage";
            CodeCoverageEvidence.Feature = Feature.CodeCoverageCheck;
            CodeCoverageEvidence.HelperMessage = messages.CodeCoverageCheck;

            FullReportPath = Path.Combine(dotCoverDir, ReportName);

            var targetWorkingDir = Path.Combine(workingDir, testFolder);

            if (!Directory.Exists(targetWorkingDir))
            {
                CodeCoverageEvidence.SetInconclusive(new SimpleEvidenceBuilder($"{testFolder} Directory Not Found"));
                return;
            }

            Argument = CreateArgument("C:\\Program Files\\dotnet", targetWorkingDir);

            featureRunner.Execute(CreateProcessDetails(dotCoverDir));

            var report = ReadReport();
            CleanUp();

            var coverage = GetCodeCoverage(report);

            if (coverage == 0)
            {
                CodeCoverageEvidence.SetInconclusive(new SimpleEvidenceBuilder("Code Coverage Not Found"));
            }
            else if (coverage == -1)
            {
                CodeCoverageEvidence.SetInconclusive(new SimpleEvidenceBuilder("Code Coverage Not Found"));
            }
            else
            {
                CodeCoverageEvidence.FeatureRating = ( (double) GetCodeCoverage(report) ) / 100;
                var featureImplemented = coverage >= passPerc;
                var evidence = $"Code Coverage: {coverage}%";
                if (featureImplemented)
                {
                    CodeCoverageEvidence.SetPassed(new SimpleEvidenceBuilder(evidence));
                }
                else
                {
                    CodeCoverageEvidence.SetFailed(new SimpleEvidenceBuilder(evidence));
                }
            }
        }

        private string CreateArgument(string dotnetDir, string targetWorkingDir)
        {
            var dotnetExecutablePath = Path.Combine(dotnetDir, "dotnet.exe");

            return $"analyse /TargetExecutable=\"{dotnetExecutablePath}\" /TargetArguments=\"test\" /TargetWorkingDir=\"{targetWorkingDir}\"" +
                $" /ReportType=\"JSON\" /Output=\"{ReportName}\"";
        }

        private ProcessDetails CreateProcessDetails(string dotCoverDir)
        {
            var processPath = Path.Combine(dotCoverDir, ProcessName);

            return new ProcessDetails(processPath, dotCoverDir, Argument);
        }

        private string ReadReport()
        {
            if (File.Exists(FullReportPath))
            {
                using (StreamReader sr = File.OpenText(FullReportPath))
                {
                    return sr.ReadToEnd();
                }
            }
            else
            {
                return "";
            }
        }

        private int GetCodeCoverage(string json)
        {
            return DotCover.CalculateCoverageFromJsonReport(json);
        }

        private void CleanUp()
        {
            File.Delete(FullReportPath);
        }

        public FeatureEvidence CodeCoverageEvidence { get; } = new FeatureEvidence();
    }
}
