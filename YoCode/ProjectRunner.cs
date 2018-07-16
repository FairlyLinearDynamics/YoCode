
using System;

namespace YoCode
{
    public class ProjectRunner
    {
        private string Process { get; } = "dotnet";

        // TODO: find other way of running .dll file instead of hardcoding the name 
        // TODO: find a way to specify location of appsettings.json file when running
        private string Argument { get; } = @"bin\Debug\netcoreapp1.1\UnitConverterWebApp.dll";
        private string Output { get; }
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
            string portKeyword = "Now listening on: ";
            string line = Output.GetLineWithOneKeyword(portKeyword);
            string[] splitLine = line.Split(portKeyword, StringSplitOptions.None);
            try { return splitLine[1]; }
            catch(IndexOutOfRangeException) { }
            return "";
        }

        public string GetErrorOutput()
        {
            return ErrorOutput;
        }
    }
}
