using System;
using System.Linq;
using System.IO;

namespace YoCode
{
    // TODO: find other way of running .dll file instead of hardcoding the name 
    internal class ProjectRunner
    {
        internal string Output { get; }

        private string Process { get; } = "dotnet";
        private string Argument { get; set; } = @"bin\Debug\";
        private string ErrorOutput { get; }
        private const string projectFolder = @"\UnitConverterWebApp";
        private readonly FeatureRunner featureRunner;

        public ProjectRunner(string workingDir, FeatureRunner featureRunner)
        {
            this.featureRunner = featureRunner;
            ProjectRunEvidence.FeatureTitle = "Project Run";

            workingDir += projectFolder;
            if (!Directory.Exists(workingDir))
            {
                ProjectRunEvidence.SetFailed("UnitConverterWebApp not found");
                return;
            }

            Argument = CreateArgument(workingDir);

            var processDetails = new ProcessDetails(Process, workingDir, Argument);

            var evidence = featureRunner.Execute(processDetails, "Application started. Press Ctrl+C to shut down.", false);
            Output = evidence.Output;
            ErrorOutput = evidence.ErrorOutput;

            ProjectRunEvidence.FeatureImplemented = ApplicationStarted();
            ProjectRunEvidence.FeatureRating = ApplicationStarted() ? 1 : 0;

            if (ProjectRunEvidence.FeatureImplemented)
            {
                ProjectRunEvidence.GiveEvidence($"Port: {GetPort()}");
            }
            else
            {
                ProjectRunEvidence.SetFailed($"Error Output: {ErrorOutput}");
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
            featureRunner.EndProcess();
        }

        public FeatureEvidence ProjectRunEvidence { get; } = new FeatureEvidence();
    }
}
