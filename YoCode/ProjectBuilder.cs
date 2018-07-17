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
                ProjectBuilderEvidence.FeatureImplemented = false;
                ProjectBuilderEvidence.GiveEvidence("Timed Out");
                return;
            }

            ProjectBuilderEvidence.FeatureImplemented = BuildSuccessful();
            ProjectBuilderEvidence.GiveEvidence($"Warning count: {GetNumberOfWarnings()}\nError count: {GetNumberOfErrors()}");
            if (GetNumberOfErrors() > 0)
            {
                ProjectBuilderEvidence.GiveEvidence($"Error message: {GetErrorOutput(Process.Output)}");
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

        public FeatureEvidence ProjectBuilderEvidence { get; set; } = new FeatureEvidence();
    }
}
