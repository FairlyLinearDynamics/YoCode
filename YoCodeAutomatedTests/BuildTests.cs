using Xunit;
using FluentAssertions;
using System.IO;
using Xunit.Abstractions;
using System.Threading;
using System;

namespace YoCodeAutomatedTests
{
    //Compares projects against unmodified project
    public class BuildTests
    {
        private readonly ITestOutputHelper xunitOutput;

        public BuildTests(ITestOutputHelper xunitOutput)
        {
            this.xunitOutput = xunitOutput;
        }

        [Theory]
        [InlineData("P1.html", "\\Project1")]
        [InlineData("P2.html", "\\Project2")]
        [InlineData("P3.html", "\\Project3")]
        [InlineData("P4.html", "\\Project4")]
        public void CompareProjects(string outputFile, string project)
        {
            var helper = new TestHelperMethods();

            var actualDir = Path.Combine(helper.TestPath, "ActualOutputs");
            var actualPath = Path.Combine(actualDir, helper.OutputFilename);

            string argument = $"YoCode.dll --input={helper.TestPath}\\TestProjects{project} --noloading --silent --output={actualDir}";

            helper.OutputTestDebugInfo(xunitOutput, argument);

            File.Delete(actualPath);

            TestHelperMethods.RunProcessAndGatherOutput("dotnet", helper.DllPath, argument, xunitOutput);

            var actualWantedPath = Path.Combine(actualDir, outputFile);
            var expectedPath = Path.Combine(helper.TestPath, "ExpectedOutputs", outputFile);

            File.Copy(actualPath, actualWantedPath, true);

            TestHelperMethods.FilesAreDifferent(actualPath, expectedPath).Should().BeFalse($"{actualPath} was different to {expectedPath}");
        }
    }
}

