﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;


namespace YoCode
{
    public class CommandLineParser
    {
        List<string> implementedCommands = new List<string>() { CommandNames.ORIGIN, CommandNames.MODIFIED, CommandNames.HELP,
            CommandNames.NOLOADINGSCREEN, CommandNames.SILENTREPORT };
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
                        ires.HelpAsked = arg.command == CommandNames.HELP;
                        break;
                    case CommandNames.SILENTREPORT:
                        ires.Silent = arg.command == CommandNames.SILENTREPORT;
                        break;
                    case CommandNames.NOLOADINGSCREEN:
                        ires.NoLoadingScreen = arg.command == CommandNames.NOLOADINGSCREEN;
                        break;
                }
            }

            ires.Errors = CommandErrorChecking.ContainsErrors(currentCommands, implementedCommands);
            return ires;
        }
    }
}