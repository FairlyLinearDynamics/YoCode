using Xunit;
using FluentAssertions;
using YoCode;
using System.IO;
using System;

namespace YoCodeAutomatedTests
{
    public class HelpMessageTest
    {
        [Fact]
        public void CheckHelpMessage()
        {
            var helper = new TestHelperMethods();

            const string argument = "YoCode.dll --help";

            ProcessRunner pr = new ProcessRunner("dotnet", helper.dllPath, argument);
            pr.ExecuteTheCheck("Minimum test count:");

            var actualPath = Path.Combine(helper.testPath, "ActualOutputs\\helpMessage.txt");
            var expectedPath = Path.Combine(helper.testPath, "ExpectedOutputs\\helpMessage.txt");

            var actualOutput = pr.Output.Trim();

            TestHelperMethods.WriteToFile(actualPath, actualOutput);

            TestHelperMethods.FilesAreDifferent(actualPath, expectedPath).Should().BeFalse();
        }
    }
}
