using Xunit;
using FluentAssertions;
using YoCode;
using System.IO;
using System;

namespace YoCodeAutomatedTests
{
    //Compares projects against junior-test unmodified project
    public class BuildTests
    {
        [Theory]
        [InlineData("\\P1.txt", "\\Project1")]
        [InlineData("\\P2.txt", "\\Project2")]
        [InlineData("\\P3.txt", "\\Project3")]
        public void CompareProjects(string outputFile, string project)
        {
            var helper = new TestHelperMethods();

            string argument = $"YoCode.dll --original={helper.testPath}\\TestProjects\\junior-test " +
                $"--modified={helper.testPath}\\TestProjects{project}";

            ProcessRunner pr = new ProcessRunner("dotnet", helper.dllPath, argument);
            pr.ExecuteTheCheck("Minimum test count:");

            var actualPath = helper.testPath+"\\ActualOutputs"+outputFile;
            var expectedPath =helper.testPath+"\\ExpectedOutputs"+outputFile;

            var actualOutput = pr.Output.Trim();
            var expectedOutput = File.ReadAllText(expectedPath);

            TestHelperMethods.WriteToFile(actualPath, actualOutput);

            TestHelperMethods.FilesAreDifferent(actualPath, expectedPath).Should().BeFalse();
        }
    }
}

