using System;
using System.Collections.Generic;

namespace YoCode
{
    // TODO: populate evidence object in ProjectBuilder class
    // TODO: handle error return values (-1 for GetNumberOfErrors and GetNumberOfWarnings)
    public class ProjectBuilder
    {
        public ProcessRunner ProcessRunner { get; }
        private string ProcessName { get; } = "dotnet";
        private string Arguments { get; } = "build";
        private string Output;

        public ProjectBuilder(string workingDir)
        {
            ProcessRunner = new ProcessRunner(ProcessName, workingDir, Arguments);
            ProcessRunner.ExecuteTheCheck();
            Output = ProcessRunner.Output;
        }

        public static string GetErrorOutput(string output)
        {
            string[] buildKeywords = { "Build succeeded.",
                "Build FAILED." + Environment.NewLine + Environment.NewLine };
            var properOutput = output.Split(buildKeywords, StringSplitOptions.None);

            try
            {
                var result = properOutput[1].Split(Environment.NewLine + "    ");

                return result[0];
            }
            catch (IndexOutOfRangeException) { }
            return "";
        }

        public bool BuildSuccessful()
        {
            var buildLine = Output.GetLineWithOneKeyword("Build succeeded");

            return buildLine != "";
        }

        public int GetNumberOfWarnings()
        {
            var warningLine = Output.GetLineWithOneKeyword("Warning(s)");
            var numbers = warningLine.GetNumbersInALine();

            return numbers.Count > 0 ? numbers[0] : -1;
        }

        public int GetNumberOfErrors()
        {
            var errorLine = Output.GetLineWithOneKeyword("Error(s)");
            var numbers = errorLine.GetNumbersInALine();

            return numbers.Count > 0 ? numbers[0] : -1;
        }
    }
}
