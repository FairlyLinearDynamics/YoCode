using Xunit;
using FluentAssertions;
using YoCode;
using System.IO;

namespace YoCodeAutomatedTests
{
    public class SuccessfulBuildTests
    {
        [Fact]
        public void RunProject1()
        {
            var helper = new TestHelperMethods();

            string argument = $"YoCode.dll --original={helper.testPath}\\TestProjects\\junior-test " +
                $"--modified={helper.testPath}\\TestProjects\\Project1";

            ProcessRunner pr = new ProcessRunner("dotnet", helper.dllPath, argument);
            pr.ExecuteTheCheck("Minimum test count:");

            var actualPath = helper.testPath+@"\Outputs\P1ActualOutput.txt";
            var expectedPath =helper.testPath+ @"\Outputs\P1.txt";

            var actualOutput = pr.Output;
            var expectedOutput = File.ReadAllText(expectedPath);

            TestHelperMethods.WriteToFile(actualPath, actualOutput);

            TestHelperMethods.FilesAreDifferent(actualPath, expectedPath).Should().BeFalse();
            
            //(pr.Output).Should().BeEquivalentTo(expectedOutput + Environment.NewLine);
        }
    }
}

