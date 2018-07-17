
using System;
using System.Linq;
using System.IO;

namespace YoCode
{
    public class ProjectRunner
    {
        private string Process { get; } = "dotnet";

        // TODO: find other way of running .dll file instead of hardcoding the name 
        // TODO: find a way to specify location of appsettings.json file when running
        private string Argument { get; } = @"bin\Debug\";   //      netcoreapp2.0\UnitConverterWebApp.dll";
        public string Output { get; }
        private string ErrorOutput { get; }

        public ProjectRunner(string workingDir)
        {
            workingDir += @"\UnitConverterWebApp";

            Argument = Argument + (Path.GetFileName(Directory.GetDirectories(workingDir + "\\" + Argument).First()))+"\\UnitConverterWebApp.dll";
  
            ProcessRunner processRunner = new ProcessRunner(Process, workingDir, Argument);
            processRunner.ExecuteTheCheck("Application started.");
            Output = processRunner.Output;
            ErrorOutput = processRunner.ErrorOutput;
            ProjectRunEvidence.FeatureTitle = "Project Run";
            ProjectRunEvidence.FeatureImplemented = ApplicationStarted();

            if (processRunner.TimedOut)
            {
                ProjectRunEvidence.GiveEvidence("Timed out");
                return;
            }

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
