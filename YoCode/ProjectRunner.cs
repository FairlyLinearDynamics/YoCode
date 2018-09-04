using System;
using System.Linq;
using System.IO;

namespace YoCode
{
    // TODO: find other way of running .dll file instead of hardcoding the name 
    internal class ProjectRunner
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
            ProjectRunEvidence.Feature = Feature.ProjectRunner;

            this.workingDir = workingDir + projectFolder;
        }

        public void Execute()
        {
            if (!Directory.Exists(workingDir))
            {
                ProjectRunEvidence.SetInconclusive(new SimpleEvidenceBuilder($"{workingDir} not found"));
                return;
            }

            Argument = CreateArgument(workingDir);

            var processDetails = new ProcessDetails(ProcessName, workingDir, Argument);

            var evidence = featureRunner.Execute(processDetails, "Application started. Press Ctrl+C to shut down.", false);
            Output = evidence.Output;
            ErrorOutput = evidence.ErrorOutput;

            // TODO: Refactor Project Runner
            var portKeyword = "Now listening on: ";
            var line = Output.GetLineWithOneKeyword(portKeyword);
            var splitLine = line.Split(portKeyword, StringSplitOptions.None);
            var port = splitLine.Length > 1 ? splitLine[1] : "";

            if (String.IsNullOrEmpty(port))
            {
                ProjectRunEvidence.SetInconclusive(new SimpleEvidenceBuilder(messages.BadPort));
                return;
            }

            var applicationStarted = ApplicationStarted();

            if (applicationStarted)
            {
                ProjectRunEvidence.SetPassed(new SimpleEvidenceBuilder($"Port: {GetPort()}"));
                ProjectRunEvidence.FeatureRating = 1;
            }
            else
            {
                ProjectRunEvidence.SetFailed(new SimpleEvidenceBuilder($"Error Output: {ErrorOutput}"));
                ProjectRunEvidence.FeatureRating = 0;
            }
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

        public FeatureEvidence ProjectRunEvidence { get; } = new FeatureEvidence();
    }
}
