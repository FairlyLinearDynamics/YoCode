using System;
using System.Linq;
using System.IO;

namespace YoCode
{
    // TODO: find other way of running .dll file instead of hardcoding the name 
    public class ProjectRunner
    {
        internal string Output { get; }

        private string Process { get; } = "dotnet";
        private string Argument { get; } = @"bin\Debug\";
        private string ErrorOutput { get; }
        private const string projectFolder = @"\UnitConverterWebApp";

        public ProjectRunner(string workingDir)
        {
            ProjectRunEvidence.FeatureTitle = "Project Run";
            workingDir += projectFolder;
            if (!Directory.Exists(workingDir))
            {
                ProjectRunEvidence.SetFailed("UnitConverterWebApp not found");
                return;
            }

            var binDebugFolder = Path.Combine(workingDir, Argument);
            var netCoreOutputFolder = Directory.GetDirectories(binDebugFolder).First();
            Argument = Path.Combine(Argument, Path.GetFileName(netCoreOutputFolder), "UnitConverterWebApp.dll");

            var processRunner = new ProcessRunner(Process, workingDir, Argument);
            processRunner.ExecuteTheCheck("Application started. Press Ctrl+C to shut down.");
            Output = processRunner.Output;
            ErrorOutput = processRunner.ErrorOutput;

            if (processRunner.TimedOut)
            {
                ProjectRunEvidence.SetFailed("Timed Out");
                return;
            }

            ProjectRunEvidence.FeatureImplemented = ApplicationStarted();

            if(ProjectRunEvidence.FeatureImplemented)
            {
                ProjectRunEvidence.GiveEvidence($"Port: {GetPort()}");
            }
            else
            {
                ProjectRunEvidence.SetFailed($"Error Output: {ErrorOutput}");
            }
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

        public FeatureEvidence ProjectRunEvidence { get; } = new FeatureEvidence();
    }
}
