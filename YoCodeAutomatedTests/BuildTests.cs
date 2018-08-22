﻿using Xunit;
using FluentAssertions;
using System.IO;

namespace YoCodeAutomatedTests
{
    //Compares projects against junior-test unmodified project
    public class BuildTests
    {
        [Theory]
        [InlineData("P1.txt", "\\Project1")]
        [InlineData("P2.txt", "\\Project2")]
        [InlineData("P3.txt", "\\Project3")]
        [InlineData("P4.txt", "\\Project4")]
        public void CompareProjects(string outputFile, string project)
        {
            var helper = new TestHelperMethods();

            string argument = $"YoCode.dll --modified={helper.TestPath}\\TestProjects{project} --noloading --silent";

            var Output = helper.RunProcess("dotnet", helper.DllPath, argument);

            var actualPath = Path.Combine(helper.TestPath, "ActualOutputs", outputFile);
            var expectedPath = Path.Combine(helper.TestPath, "ExpectedOutputs", outputFile);

            var actualOutput = Output.Trim();

            helper.WriteToFile(actualPath, actualOutput);

            helper.FilesAreDifferent(actualPath, expectedPath).Should().BeFalse($"{actualPath} was different to {expectedPath}");
        }
    }
}

