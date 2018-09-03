using System.Collections.Generic;
using System.Linq;

namespace YoCode
{
    internal class CommandLineParser
    {
        private readonly List<string> implementedCommands = new List<string>() { CommandNames.INPUT, CommandNames.HELP,
            CommandNames.NOLOADINGSCREEN, CommandNames.SILENTREPORT, CommandNames.JUNIORTEST, CommandNames.OUTPUT, CommandNames.NOHTML };

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
                    case CommandNames.INPUT:
                        ires.InputFilePath = arg.data;
                        break;
                    case CommandNames.HELP:
                        ires.HelpAsked = true;
                        break;
                    case CommandNames.SILENTREPORT:
                        ires.OpenHtmlReport = false;
                        break;
                    case CommandNames.NOLOADINGSCREEN:
                        ires.NoLoadingScreen = true;
                        break;
                    case CommandNames.JUNIORTEST:
                        ires.JuniorTest = true;
                        break;
                    case CommandNames.OUTPUT:
                        ires.OutputFilePath = arg.data;
                        break;
                    case CommandNames.NOHTML:
                        ires.CreateHtmlReport = false;
                        break;
                }
            }

            ires.Errors = CommandErrorChecking.ContainsErrors(currentCommands, implementedCommands);
            return ires;
        }
    }
}