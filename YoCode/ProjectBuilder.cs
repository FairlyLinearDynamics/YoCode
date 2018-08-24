﻿using System;
using System.IO;

namespace YoCode
{
    internal class ProjectBuilder
    {
        private string ProcessName { get; } = "dotnet";
        private string Arguments { get; } = "build";
        private readonly string Output;
        private const string projectFolder = "UnitConverterWebApp";

        public ProjectBuilder(string workingDir, FeatureRunner featureRunner)
        {
            CleanBuildOutput(workingDir);

            ProjectBuilderEvidence.FeatureTitle = "Project build";
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

            if (Output.Contains(""))
            {

            }

            bool ErrorGettingErrorsOrWarnings = GetNumberOfErrors() == -1 || GetNumberOfWarnings() == -1;

            if (evidence.FeatureImplemented == null|| ErrorGettingErrorsOrWarnings)
            {
                ProjectBuilderEvidence.SetInconclusive("Timed Out");
                return;
            }
            ProjectBuilderEvidence.FeatureImplemented = BuildSuccessful();
            ProjectBuilderEvidence.FeatureRating = BuildSuccessful() ? 1 : 0;

            ProjectBuilderEvidence.GiveEvidence($"Warning count: {GetNumberOfWarnings()}\nError count: {GetNumberOfErrors()}");
            if (GetNumberOfErrors() > 0)
            {
                ProjectBuilderEvidence.SetFailed($"Error message: {GetErrorOutput(Output)}");
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

        private static void CleanBuildOutput(string workingDir)
        {
            var pr = new ProcessRunner("dotnet", workingDir, "clean");
            pr.ExecuteTheCheck();
        }

        public FeatureEvidence ProjectBuilderEvidence { get; set; } = new FeatureEvidence();
    }
}
