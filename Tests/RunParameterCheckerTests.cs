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
        Output fakeOutput;
        InputResult result;

        Mock<IAppSettingsBuilder> readMock = new Mock<IAppSettingsBuilder>();

        IAppSettingsBuilder appsb;

        public RunParameterCheckerTests()
        {
            var outputs = new List<IPrint> { new WebWriter(), new ConsoleWriter() };
            fakeOutput = new Output(new CompositeWriter(outputs));
            appsb = readMock.Object;


        }

        [Fact]
        public void TestIfHelpAsked()
        {
            string[] args = { "--help" };

            var commandLinehandler = new CommandLineParser(args);
            result = commandLinehandler.Parse();

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
               nameof(ArgErrorType.WrongOriginalDirectory),
               nameof(ArgErrorType.WrongModifiedDirectory)
            };

            string[] args = { "--privet druzja" };
            var commandLinehandler = new CommandLineParser(args);
            result = commandLinehandler.Parse();

            var rpc = new RunParameterChecker(fakeOutput, result, appsb);

            rpc.ParametersAreValid().Should().BeFalse();
            rpc.Errs.Should().BeEquivalentTo(errorList);
        }

        [Fact]
        public void TestIfFileNotFoundExceptionIsHandled()
        {
            string[] args = { "--let's pretend this is a valid argument" };
            var commandLinehandler = new CommandLineParser(args);

            var resultMock = new Mock<IInputResult>();
            
            var emptyList = new List<string>();

            readMock.Setup(x => x.ReadJSONFile()).Throws(new FileNotFoundException());

            resultMock.Setup(x => x.HelpAsked).Returns(false);
            resultMock.Setup(x => x.Errors).Returns(emptyList);

            var resultObj = resultMock.Object;

            var rpc = new RunParameterChecker(fakeOutput, resultObj, appsb);
            rpc.ParametersAreValid();

            rpc.Errs.Should().BeEquivalentTo("Did not find appsettings file");
        }

        [Fact]
        public void TestIfFormatExceptionIsHandled()
        {
            string[] args = { "--let's pretend this is a valid argument" };
            var commandLinehandler = new CommandLineParser(args);
            result = commandLinehandler.Parse();

            result.Errors.Clear();

            readMock.Setup(x => x.ReadJSONFile()).Throws(new FormatException());

            var rpc = new RunParameterChecker(fakeOutput, result, appsb);

            rpc.ParametersAreValid();

            rpc.Errs.Should().BeEquivalentTo("Error reading JSON file");
        }
    }
}
