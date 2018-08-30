using System;
using System.Diagnostics;
using Xunit;
using FluentAssertions;
using System.IO;
using Xunit.Abstractions;

namespace YoCodeAutomatedTests
{
    //Compares projects against junior-test unmodified project
    public class BuildTests
    {
        private readonly ITestOutputHelper xunitOutput;

        public BuildTests(ITestOutputHelper xunitOutput)
        {
            this.xunitOutput = xunitOutput;
        }

        [Theory]
        [InlineData("P1.txt", "\\Project1")]
        [InlineData("P2.txt", "\\Project2")]
        [InlineData("P3.txt", "\\Project3")]
        [InlineData("P4.txt", "\\Project4")]
        public void CompareProjects(string outputFile, string project)
        {
            var helper = new TestHelperMethods();

            string argument = $"YoCode.dll --input={helper.TestPath}\\TestProjects{project} --noloading --nohtml";

            helper.OutputTestDebugInfo(xunitOutput, argument);

            var processOutput = helper.RunProcess("dotnet", helper.DllPath, argument);

            var actualPath = Path.Combine(helper.TestPath, "ActualOutputs", outputFile);
            var expectedPath = Path.Combine(helper.TestPath, "ExpectedOutputs", outputFile);

            var actualOutput = processOutput.Trim();

            helper.WriteToFile(actualPath, actualOutput);

            helper.FilesAreDifferent(actualPath, expectedPath).Should().BeFalse($"{actualPath} was different to {expectedPath}");
        }
    }
}

