using Xunit;
using FluentAssertions;
using YoCode;
using System.Collections.Generic;
using Moq;
using System;
using System.IO;

namespace YoCode_XUnit
{
    public class RunParameterCheckerTests
    {
        private readonly Output fakeOutput;
        private InputResult result;

        private readonly Mock<IAppSettingsBuilder> readMock = new Mock<IAppSettingsBuilder>();

        private readonly IAppSettingsBuilder appsb;

        public RunParameterCheckerTests()
        {
            var outputs = new List<IPrint> { new ConsoleWriter() };
            fakeOutput = new Output(new CompositeWriter(outputs));

            appsb = readMock.Object;
        }

        private static InputResult SetupMockInputResult(string args)
        {
            string[] arg = { args };

            var commandLineHandler = new CommandLineParser(arg);
            return commandLineHandler.Parse();
        }

        [Fact]
        public void TestIfHelpAsked()
        {
            result = SetupMockInputResult("--help");

            var rpc = new RunParameterChecker(fakeOutput, result, appsb);

            rpc.ParametersAreValid().Should().BeFalse();
            rpc.Errs.Count.Should().Be(0);
        }

        [Fact]
        public void TestWrongArgument()
        {
            List<string> errorList = new List<string>()
            {
               nameof(ArgErrorType.WrongCommand),
               nameof(ArgErrorType.WrongInputDirectory)
            };

            result = SetupMockInputResult("--privet druzja");

            var rpc = new RunParameterChecker(fakeOutput, result, appsb);

            rpc.ParametersAreValid().Should().BeFalse();
            rpc.Errs.Should().BeEquivalentTo(errorList);
        }

        [Fact]
        public void TestIfFileNotFoundExceptionIsHandled()
        {
            result = SetupMockInputResult("--lalala");
            result.HelpAsked = false;
            result.Errors.Clear();

            readMock.Setup(x => x.ReadJSONFile()).Throws(new FileNotFoundException());

            var rpc = new RunParameterChecker(fakeOutput, result, appsb);

            rpc.ParametersAreValid().Should().BeFalse();
            rpc.Errs.Should().BeEquivalentTo("Did not find appsettings file");
        }

        [Fact]
        public void TestIfFormatExceptionIsHandled()
        {
            result = SetupMockInputResult("--lalala");
            result.HelpAsked = false;
            result.Errors.Clear();

            readMock.Setup(x => x.ReadJSONFile()).Throws(new FormatException());

            var rpc = new RunParameterChecker(fakeOutput, result, appsb);

            rpc.ParametersAreValid().Should().BeFalse();
            rpc.Errs.Should().BeEquivalentTo("Error reading JSON file");
        }
    }
}
