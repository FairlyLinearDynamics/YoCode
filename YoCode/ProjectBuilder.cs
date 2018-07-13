using System;
using System.Collections.Generic;

namespace YoCode
{
    // TODO: populate evidence object in ProjectBuilder class
    public class ProjectBuilder
    {
        public ProcessRunner ProcessRunner { get; }
        private string ProcessName { get; } = "dotnet";
        private string Arguments { get; } = "build";

        public ProjectBuilder(string workingDir)
        {
            ProcessRunner = new ProcessRunner(ProcessName, workingDir, Arguments);
            ProcessRunner.ExecuteTheCheck();

            ProjectBuilderEvidence.FeatureTitle = "Project build";
            ProjectBuilderEvidence.FeatureImplemented = BuildSuccessful();
            ProjectBuilderEvidence.GiveEvidence($"Warning count: {GetNumberOfWarnings()}\nError count: {GetNumberOfErrors()}");
            if (GetNumberOfErrors() > 0)
            {
                ProjectBuilderEvidence.GiveEvidence($"Error message: {GetErrorOutput(ProcessRunner.Output)}");
            }
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
            string buildLine = GetLineWithOneKeyword("Build succeeded");

            return buildLine != "";
        }

        public int GetNumberOfWarnings()
        {
            string warningLine = GetLineWithOneKeyword("Warning(s)");
            List<int> numbers = warningLine.GetNumbersInALine();

            try { return numbers[0]; }
            catch (ArgumentOutOfRangeException) { }
            return -1;
        }

        public int GetNumberOfErrors()
        {
            string errorLine = GetLineWithOneKeyword("Error(s)");
            List<int> numbers = errorLine.GetNumbersInALine();

            try { return numbers[0]; }
            catch (ArgumentOutOfRangeException) { }
            return -1;
        }

        private string GetLineWithOneKeyword(string keyword)
        {
            return ProcessRunner.Output.GetLineWithAllKeywords(new List<string> { keyword });
        }

        public FeatureEvidence ProjectBuilderEvidence { get; private set; } = new FeatureEvidence();
    }
}
