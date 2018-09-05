using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace YoCode
{
    // TODO: find other way of running .dll file instead of hardcoding the name 
    internal class ProjectRunner : ICheck
    {
        internal string Output { get; set; }

        private string ProcessName { get; } = "dotnet";
        private string Argument { get; set; } = @"bin\Debug\";
        private string ErrorOutput { get; set; }
        private const string projectFolder = @"\UnitConverterWebApp";
        private readonly FeatureRunner featureRunner;
        private readonly string workingDir;

        public ProjectRunner(string workingDir, FeatureRunner featureRunner)
        {
            this.featureRunner = featureRunner;

            this.workingDir = workingDir + projectFolder;
        }

        public Task<List<FeatureEvidence>> Execute()
        {
            return Task.Run(() =>
            {
                var projectRunEvidence = new FeatureEvidence {Feature = Feature.ProjectRunner};

                if (!Directory.Exists(workingDir))
                {
                    projectRunEvidence.SetInconclusive(new SimpleEvidenceBuilder($"{workingDir} not found"));
                    return new List<FeatureEvidence> {projectRunEvidence};
                }

                Argument = CreateArgument(workingDir);

                var processDetails = new ProcessDetails(ProcessName, workingDir, Argument);

                var evidence = featureRunner.Execute(processDetails, "Application started. Press Ctrl+C to shut down.", false);
                Output = evidence.Output;
                ErrorOutput = evidence.ErrorOutput;

                // TODO: Refactor Project Runner
                const string portKeyword = "Now listening on: ";
                var line = Output.GetLineWithOneKeyword(portKeyword);
                var splitLine = line.Split(portKeyword, StringSplitOptions.None);
                var port = splitLine.Length > 1 ? splitLine[1] : "";

                if (String.IsNullOrEmpty(port))
                {
                    projectRunEvidence.SetInconclusive(new SimpleEvidenceBuilder(messages.BadPort));
                    return new List<FeatureEvidence> {projectRunEvidence};
                }

                var applicationStarted = ApplicationStarted();

                if (applicationStarted)
                {
                    projectRunEvidence.SetPassed(new SimpleEvidenceBuilder($"Port: {GetPort()}"));
                    projectRunEvidence.FeatureRating = 1;
                }
                else
                {
                    projectRunEvidence.SetFailed(new SimpleEvidenceBuilder($"Error Output: {ErrorOutput}"));
                    projectRunEvidence.FeatureRating = 0;
                }

                return new List<FeatureEvidence> {projectRunEvidence};
            });
        }

        private string CreateArgument(string workingDir)
        {
            var binDebugFolder = Path.Combine(workingDir, Argument);
            var netCoreOutputFolder = Directory.GetDirectories(binDebugFolder).First();
            return Argument = Path.Combine(Argument, Path.GetFileName(netCoreOutputFolder), "UnitConverterWebApp.dll");
        }

        public bool ApplicationStarted()
        {
            return Output.GetLineWithOneKeyword("Application started.")?.Length != 0;
        }

        public string GetPort()
        {
            const string portKeyword = "Now listening on: ";
            var line = Output.GetLineWithOneKeyword(portKeyword);
            var splitLine = line.Split(portKeyword, StringSplitOptions.None);
            return splitLine.Length > 1 ? splitLine[1] : "";
        }

        public void KillProject()
        {
            try
            {
                featureRunner.EndProcess();
            }
            catch (NullReferenceException) { }
        }

        public void ReportLefOverProcess()
        {
            featureRunner.FindLeftOverProcess();
        }
    }
}
