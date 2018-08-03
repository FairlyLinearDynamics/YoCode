using Xunit;
using FluentAssertions;
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

            var Output = helper.RunProcess("dotnet", helper.DllPath, argument);

            var actualPath = Path.Combine(helper.TestPath, "ActualOutputs\\helpMessage.txt");
            var expectedPath = Path.Combine(helper.TestPath, "ExpectedOutputs\\helpMessage.txt");

            var actualOutput = Output.Trim();

            helper.WriteToFile(actualPath, actualOutput);

            helper.FilesAreDifferent(actualPath, expectedPath).Should().BeFalse($"{actualPath} was different to {expectedPath}");
        }
    }
}
