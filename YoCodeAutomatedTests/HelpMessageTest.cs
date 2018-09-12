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

            File.Delete(helper.DefaultYoCodeReportPath);

            const string argument = "YoCode.dll --help --silent";

            helper.OutputTestDebugInfo(xunitOutput, argument);

            TestHelperMethods.RunProcessAndGatherOutput("dotnet", helper.DllPath, argument, xunitOutput);

            var actualPath = Path.Combine(helper.TestPath, "ActualOutputs\\helpMessage.html");
            var expectedPath = Path.Combine(helper.TestPath, "ExpectedOutputs\\helpMessage.html");

            File.Copy(helper.DefaultYoCodeReportPath, actualPath, true);

            TestHelperMethods.FilesAreDifferent(actualPath, expectedPath).Should().BeFalse($"{actualPath} was different to {expectedPath}");
        }
    }
}
