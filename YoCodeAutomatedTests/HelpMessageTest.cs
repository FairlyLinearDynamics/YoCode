using Xunit;
using FluentAssertions;
using System.IO;
using Xunit.Abstractions;

namespace YoCodeAutomatedTests
{
    public class HelpMessageTest
    {
        private readonly ITestOutputHelper xunitOutput;

        public HelpMessageTest(ITestOutputHelper xunitOutput)
        {
            this.xunitOutput = xunitOutput;
        }

        [Fact]
        public void CheckHelpMessage()
        {
            var helper = new TestHelperMethods();

            const string argument = "YoCode.dll --help --silent";

            helper.OutputTestDebugInfo(xunitOutput, argument);

            var processOutput = helper.RunProcess("dotnet", helper.DllPath, argument);

            var actualPath = Path.Combine(helper.TestPath, "ActualOutputs\\helpMessage.txt");
            var expectedPath = Path.Combine(helper.TestPath, "ExpectedOutputs\\helpMessage.txt");

            var actualOutput = processOutput.Trim();

            helper.WriteToFile(actualPath, actualOutput);

            helper.FilesAreDifferent(actualPath, expectedPath).Should().BeFalse($"{actualPath} was different to {expectedPath}");
        }
    }
}
