using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;

namespace YoCode
{
    internal class CodeCoverageCheck
    {
        private string Argument { get; }
        private string ProcessName { get; } = "dotCover.exe";
        private const string testFolder = "UnitConverterTests";
        private string ReportName { get; } = "report.json";
        private string FullReportPath { get; }

        public CodeCoverageCheck(string dotCoverDir, string workingDir, FeatureRunner featureRunner)
        {
            CodeCoverageEvidence.FeatureTitle = "Code Coverage";
            CodeCoverageEvidence.Feature = Feature.CodeCoverageCheck;

            FullReportPath = Path.Combine(dotCoverDir, ReportName);

            var targetWorkingDir = Path.Combine(workingDir, testFolder);

            if (!Directory.Exists(targetWorkingDir))
            {
                CodeCoverageEvidence.SetFailed($"{testFolder} Directory Not Found");
                return;
            }

            Argument = CreateArgument("C:\\Program Files\\dotnet", targetWorkingDir);

            var evidence = featureRunner.Execute(CreateProcessDetails(dotCoverDir));

            var report = ReadReport();
            CleanUp();

            var coverage = GetCodeCoverage(report);

            if (coverage == 0)
            {
                CodeCoverageEvidence.SetFailed("Code Coverage Not Found");
            }
            else if (coverage == -1)
            {
                CodeCoverageEvidence.SetFailed("Failed to Generate/Read Report");
            }
            else
            {
                CodeCoverageEvidence.FeatureImplemented = true;
                CodeCoverageEvidence.GiveEvidence($"Code Coverage: {coverage}%");
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
            try
            {
                var coverage = JObject.Parse(json)["Children"]
                   .Where(c => (string)c["Name"] == "UnitConverterWebApp" && (string)c["Kind"] == "Assembly")
                   .Select(c => (int)c["CoveragePercent"]);

                return coverage.FirstOrDefault();
            }
            catch (ArgumentNullException) { }
            /*If YoCode times out then dotCover also fails and ArgumentNullException occurs*/

            catch (JsonReaderException) { }
            /*If ReadReport method doesn't find a report file to read then it returns
             * an empty string and GetCodeCoverage returns JsonReaderException*/

            return -1;
        }

        private void CleanUp()
        {
            File.Delete(FullReportPath);
        }

        public FeatureEvidence CodeCoverageEvidence { get; } = new FeatureEvidence();
    }
}
