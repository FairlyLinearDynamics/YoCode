
using System;

namespace YoCode
{
    public class ProjectRunner
    {
        private string Process { get; } = "dotnet";

        // TODO: find other way of running .dll file instead of hardcoding the name 
        // TODO: find a way to specify location of appsettings.json file when running
        private string Argument { get; } = @"bin\Debug\netcoreapp1.1\UnitConverterWebApp.dll";
        public string Output { get; }
        private string ErrorOutput { get; }

        public ProjectRunner(string workingDir)
        {
            ProcessRunner processRunner = new ProcessRunner(Process, workingDir, Argument);
            processRunner.ExecuteTheCheck("Application started.");
            Output = processRunner.Output;
            ErrorOutput = processRunner.ErrorOutput;
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
    }
}
