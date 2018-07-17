using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;


namespace YoCode
{
    public class CommandLineParser
    {
        List<string> implementedCommands = new List<string>() { CommandNames.ORIGIN, CommandNames.MODIFIED, CommandNames.HELP };
        private List<SplitArg> currentCommands;
        
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
                    case CommandNames.ORIGIN:
                        ires.originalFilePath = arg.data;
                        break;
                    case CommandNames.MODIFIED:
                        ires.modifiedFilePath = arg.data;
                        break;
                    case CommandNames.HELP:
                        ires.helpAsked = arg.command == CommandNames.HELP;
                        break;
                }
            }

            ires.errors = CommandErrorChecking.ContainsErrors(currentCommands, implementedCommands);
            return ires;
        }
    }
}