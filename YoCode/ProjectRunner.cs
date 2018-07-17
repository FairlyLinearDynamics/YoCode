using System;
using System.Linq;
using System.IO;

namespace YoCode
{
    public class ProjectRunner
    {
        private string Process { get; } = "dotnet";

        // TODO: find other way of running .dll file instead of hardcoding the name 
        private string Argument { get; } = @"bin\Debug\";
        public string Output { get; }
        private string ErrorOutput { get; }

        private const string PROJECT_FOLDER = @"\UnitConverterWebApp";

        public ProjectRunner(string workingDir)
        {
            ProjectRunEvidence.FeatureTitle = "Project Run";
            workingDir += PROJECT_FOLDER;
            if (!Directory.Exists(workingDir))
            {
                ProjectRunEvidence.FeatureImplemented = false;
                ProjectRunEvidence.GiveEvidence("UnitConverterWebApp not found");
                return;
            }

            Argument = Argument + (Path.GetFileName(Directory.GetDirectories(workingDir + "\\" + Argument).First()))+"\\UnitConverterWebApp.dll";

            ProcessRunner processRunner = new ProcessRunner(Process, workingDir, Argument);
            processRunner.ExecuteTheCheck("Application started.");
            Output = processRunner.Output;
            ErrorOutput = processRunner.ErrorOutput;
            
            if (processRunner.TimedOut)
            {
                ProjectRunEvidence.FeatureImplemented = false;
                ProjectRunEvidence.GiveEvidence("Timed Out");
                return;
            }

            ProjectRunEvidence.FeatureImplemented = ApplicationStarted();

            if(ProjectRunEvidence.FeatureImplemented)
            {
                ProjectRunEvidence.GiveEvidence($"Port: {GetPort()}");
            }
            else
            {
                ProjectRunEvidence.GiveEvidence($"Error Output: {GetErrorOutput()}");
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

        public string GetErrorOutput()
        {
            return ErrorOutput;
        }

        public FeatureEvidence ProjectRunEvidence { get; } = new FeatureEvidence();
    }
}
