using Xunit;
using FluentAssertions;
using YoCode;
using System.IO;

namespace YoCodeAutomatedTests
{
    public class HelpMessageTest
    {
        [Fact]
        public void CheckHelpMessage()
        {
            var helper = new TestHelperMethods();

            const string argument = "YoCode.dll --help";

            ProcessRunner pr = new ProcessRunner("dotnet", helper.DllPath, argument);
            pr.ExecuteTheCheck("Minimum test count:");

            var actualPath = Path.Combine(helper.TestPath, "ActualOutputs\\helpMessage.txt");
            var expectedPath = Path.Combine(helper.TestPath, "ExpectedOutputs\\helpMessage.txt");

            var actualOutput = pr.Output.Trim();

            helper.WriteToFile(actualPath, actualOutput);

            helper.FilesAreDifferent(actualPath, expectedPath).Should().BeFalse();
        }
    }
}
