using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;


namespace YoCode
{
    public class CommandLineParser
    {
        List<string> implementedCommands = new List<string>() { CommandNames.ORIGIN, CommandNames.MODIFIED, CommandNames.HELP };
        List<SplitArg> currentCommands;
        
        public CommandLineParser(string[] args)
        {
            currentCommands = args.Select(arg => CommandExtractor.ArgsSplitter(arg)).ToList();
        }

        public InputResult Parse()
        {
            var ires = new InputResult();

            foreach (SplitArg arg in currentCommands)
            {
                ires.originalFilePath = (arg.command == CommandNames.ORIGIN) ? arg.data : ires.originalFilePath;
                ires.modifiedFilePath = (arg.command == CommandNames.MODIFIED) ? arg.data : ires.modifiedFilePath;
                ires.helpAsked = arg.command == CommandNames.HELP;
            }

            ires.errors = CommandErrorChecking.ContainsErrors(currentCommands, implementedCommands);
            return ires;
        }
    }
}