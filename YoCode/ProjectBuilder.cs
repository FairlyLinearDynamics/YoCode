using System;
using System.IO;

namespace YoCode
{
    internal class ProjectBuilder
    {
        private string ProcessName { get; } = "dotnet";
        private string Arguments { get; } = "build";
        private readonly string Output;
        private const string projectFolder = "UnitConverterWebApp";
        private bool buildSuccessful;

        public ProjectBuilder(string workingDir, FeatureRunner featureRunner)
        {
            CleanBuildOutput(workingDir);

            ProjectBuilderEvidence.Feature = Feature.ProjectBuilder;

            workingDir = Path.Combine(workingDir, projectFolder);
            if (!Directory.Exists(workingDir))
            {
                ProjectBuilderEvidence.SetInconclusive($"{workingDir} not found");
                return;
            }


            var processDetails = new ProcessDetails(ProcessName, workingDir, Arguments);

            var evidence = featureRunner.Execute(processDetails);
            Output = evidence.Output;

            var errs = evidence.ErrorOutput;
            CheckBuildSuccess();

            var errorGettingErrorsOrWarnings = GetNumberOfErrors() == -1 || GetNumberOfWarnings() == -1;

            if (evidence.Inconclusive || errorGettingErrorsOrWarnings)
            {
                ProjectBuilderEvidence.SetInconclusive($"Could not find output from build process confirming success or failure.\nBuild process error output:\n{errs} ");
                return;
            }

            var buildOutput = $"Warning count: {GetNumberOfWarnings()}\nError count: {GetNumberOfErrors()}";
            if (buildSuccessful)
            {
                ProjectBuilderEvidence.SetPassed(buildOutput);
                ProjectBuilderEvidence.FeatureRating = 1;
            }
            else
            {
                ProjectBuilderEvidence.GiveEvidence(buildOutput);
                ProjectBuilderEvidence.SetFailed($"Error message: {GetErrorOutput(Output)}");
                ProjectBuilderEvidence.FeatureRating = 0;
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

        private void CheckBuildSuccess()
        {
            if(Output.Contains("is being used by another process"))
            {
                ProjectBuilderEvidence.SetInconclusive("Could not build the project. It is being used by another process");
                return;
            }
            buildSuccessful = Output.Contains("Build succeeded");
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

        private static void CleanBuildOutput(string workingDir)
        {
            var pr = new ProcessRunner("dotnet", workingDir, "clean");
            pr.ExecuteTheCheck();
        }

        public FeatureEvidence ProjectBuilderEvidence { get; set; } = new FeatureEvidence();
    }
}
