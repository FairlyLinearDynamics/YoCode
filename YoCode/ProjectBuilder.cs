using System;

namespace YoCode
{
    // TODO: populate evidence object in ProjectBuilder class
    // TODO: handle error return values (-1 for GetNumberOfErrors and GetNumberOfWarnings)
    public class ProjectBuilder
    {
        public ProcessRunner Process { get; }
        private string ProcessName { get; } = "dotnet";
        private string Arguments { get; } = "build";
        private readonly string Output;

        public ProjectBuilder(string workingDir)
        {
            Process = new ProcessRunner(ProcessName, workingDir, Arguments);
            Process.ExecuteTheCheck();

            Output = Process.Output;

            ProjectBuilderEvidence.FeatureTitle = "Project build";

            if(Process.TimedOut || GetNumberOfErrors() == -1 || GetNumberOfWarnings() == -1)
            {
                ProjectBuilderEvidence.SetFailed("Timed Out");
                return;
            }

            ProjectBuilderEvidence.FeatureImplemented = BuildSuccessful();
            ProjectBuilderEvidence.GiveEvidence($"Warning count: {GetNumberOfWarnings()}\nError count: {GetNumberOfErrors()}");
            if (GetNumberOfErrors() > 0)
            {
                ProjectBuilderEvidence.SetFailed($"Error message: {GetErrorOutput(Process.Output)}");
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

        private bool BuildSuccessful()
        {
            var buildLine = Output.GetLineWithOneKeyword("Build succeeded");

            return buildLine != "";
        }

        private int GetNumberOfWarnings()
        {
            return GetReportedNumber("Warning(s)");
        }

        private int GetNumberOfErrors()
        {
            return GetReportedNumber("Error(s)");
        }

        private int GetReportedNumber(string keyword)
        {
            var errorLine = Output.GetLineWithOneKeyword(keyword);
            var numbers = errorLine.GetNumbersInALine();

            return numbers.Count > 0 ? numbers[0] : -1;
        }

        public FeatureEvidence ProjectBuilderEvidence { get; set; } = new FeatureEvidence();
    }
}
