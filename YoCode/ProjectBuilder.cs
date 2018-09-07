using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YoCode
{
    internal class ProjectBuilder : ICheck
    {
        private readonly string workingDir;
        private readonly FeatureRunner featureRunner;
        private string ProcessName { get; } = "dotnet";
        private string Arguments { get; } = "build";
        private string Output;
        private bool buildSuccessful;

        public ProjectBuilder(string workingDir, FeatureRunner featureRunner)
        {
            this.workingDir = workingDir;
            this.featureRunner = featureRunner;
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
                ProjectBuilderEvidence.SetInconclusive(new SimpleEvidenceBuilder("Could not build the project. It is being used by another process"));
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

        private FeatureEvidence ProjectBuilderEvidence { get; } = new FeatureEvidence {Feature = Feature.ProjectBuilder};

        public Task<List<FeatureEvidence>> Execute()
        {
            return Task.Run(() =>
            {
                CleanBuildOutput(workingDir);

                var processDetails = new ProcessDetails(ProcessName, workingDir, Arguments);

                var processOutput = featureRunner.Execute(processDetails);

                if (processOutput.Output != null)
                {
                    ProjectBuilderEvidence.SetInconclusive(new SimpleEvidenceBuilder("No outputs were found in the process"));
                    return null;
                }

                Output = processOutput.Output;

                var errs = processOutput.ErrorOutput;
                CheckBuildSuccess();

                var errorGettingErrorsOrWarnings = GetNumberOfErrors() == -1 || GetNumberOfWarnings() == -1;

                if (ProjectBuilderEvidence.Inconclusive || errorGettingErrorsOrWarnings)
                {
                    ProjectBuilderEvidence.SetInconclusive(new SimpleEvidenceBuilder($"Could not find output from build process confirming success or failure.\nBuild process error output:\n{errs} "));
                    return new List<FeatureEvidence> { ProjectBuilderEvidence };
                }

                var buildOutput = $"Warning count: {GetNumberOfWarnings()}\nError count: {GetNumberOfErrors()}";
                if (buildSuccessful)
                {
                    ProjectBuilderEvidence.SetPassed(new SimpleEvidenceBuilder(buildOutput));
                    ProjectBuilderEvidence.FeatureRating = 1;
                }
                else
                {
                    ProjectBuilderEvidence.SetFailed(new SimpleEvidenceBuilder($"Error message: {GetErrorOutput(Output)}"));
                    ProjectBuilderEvidence.FeatureRating = 0;
                }

                return new List<FeatureEvidence> { ProjectBuilderEvidence };
            });
        }
    }
}
