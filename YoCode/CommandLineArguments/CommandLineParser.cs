using System.Collections.Generic;
using System.Linq;

namespace YoCode
{
    public class CommandLineParser
    {
        private readonly List<string> implementedCommands = new List<string>() { CommandNames.MODIFIED, CommandNames.HELP,
            CommandNames.NOLOADINGSCREEN, CommandNames.SILENTREPORT, CommandNames.JUNIORTEST, CommandNames.OUTPUT };

        private readonly List<SplitArg> currentCommands;

        public CommandLineParser(string[] args)
        {
            currentCommands = args.Select(CommandExtractor.ArgsSplitter).ToList();
        }

        public InputResult Parse()
        {
            var ires = new InputResult();

            foreach (SplitArg arg in currentCommands)
            {
                switch (arg.command)
                {
                    case CommandNames.MODIFIED:
                        ires.ModifiedFilePath = arg.data;
                        break;
                    case CommandNames.HELP:
                        ires.HelpAsked = arg.command == CommandNames.HELP;
                        break;
                    case CommandNames.SILENTREPORT:
                        ires.Silent = arg.command == CommandNames.SILENTREPORT;
                        break;
                    case CommandNames.NOLOADINGSCREEN:
                        ires.NoLoadingScreen = arg.command == CommandNames.NOLOADINGSCREEN;
                        break;
                    case CommandNames.JUNIORTEST:
                        ires.JuniorTest = arg.command == CommandNames.JUNIORTEST;
                        break;
                    case CommandNames.OUTPUT:
                        ires.OutputFilePath = arg.data;
                        break;
                }
            }

            ires.Errors = CommandErrorChecking.ContainsErrors(currentCommands, implementedCommands);
            return ires;
        }
    }
}