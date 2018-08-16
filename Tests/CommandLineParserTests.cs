using Xunit;
using FluentAssertions;
using YoCode;

namespace YoCode_XUnit
{
    public class CommandLineParserTests
    {
        [Fact]
        public void CommandLineParser_WrongOriginalPathInput()
        {
            var cmd = new CommandLineParser(new string[] { "--original=WRONGPATH" });

            var cmdResult = cmd.Parse();

            cmdResult.Errors.Should().Contain(nameof(ArgErrorType.WrongOriginalDirectory));
        }

        [Fact]
        public void CommandLineParser_WrongModifiedPathInput()
        {
            var cmd = new CommandLineParser(new string[] { "--modified=WRONGPATH" });

            var cmdResult = cmd.Parse();

            cmdResult.Errors.Should().Contain(nameof(ArgErrorType.WrongModifiedDirectory));
        }

        [Fact]
        public void CommandLineParser_WrongCommand()
        {
            var cmd = new CommandLineParser(new string[] { "--wrongCommand=/" });

            var cmdResult = cmd.Parse();

            cmdResult.Errors.Should().Contain(nameof(ArgErrorType.WrongCommand));
        }

        [Fact]
        public void CommandLineParser_NoArgsPassed()
        {
            var cmd = new CommandLineParser(new string[] { });

            var cmdResult = cmd.Parse();

            cmdResult.Errors.Should().Contain(nameof(ArgErrorType.NoArguments));
        }

        [Fact]
        public void CommandLineParser_HelpMe()
        {
            var cmd = new CommandLineParser(new string[] { "--help" });

            var cmdResult = cmd.Parse();

            cmdResult.HelpAsked.Should().Be(true);
        }

        [Fact]
        public void CommandLineParser_ExpectedOriginalFilePath()
        {
            var cmd = new CommandLineParser(new string[] { "--original=/" });

            var cmdResult = cmd.Parse();

            cmdResult.originalFilePath.Should().Be("/");
        }

        [Fact]
        public void CommandLineParser_ExpectedModifiedFilepath()
        {
            var cmd = new CommandLineParser(new string[] { "--modified=/" });

            var cmdResult = cmd.Parse();

            cmdResult.modifiedFilePath.Should().Be("/");
        }

        [Fact]
        public void CommandLineParser_OnlyOriginalFileParsingCommand()
        {
            var cmd = new CommandLineParser(new string[] { "--original=/" });

            var cmdResult = cmd.Parse();

            cmdResult.Errors.Should().Contain(nameof(ArgErrorType.WrongModifiedDirectory));
        }

        [Fact]
        public void CommandLineParser_OnlyModifiedFileParsingCommand()
        {
            var cmd = new CommandLineParser(new string[] { "--modified=/" });

            var cmdResult = cmd.Parse();

            cmdResult.Errors.Should().Contain(nameof(ArgErrorType.WrongOriginalDirectory));
        }
    }
}
