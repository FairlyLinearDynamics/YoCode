using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;


namespace YoCode
{
    public class CommandLineParser
    { 

        const string ORIGIN = "original";
        const string MODIFIED = "modified";
        const string HELP = "help";

        List<string> implementedCommands = new List<string>() { ORIGIN, MODIFIED, HELP };

        string commandIdentifier = "--";
        string dataIdentifier = "=";

        public CommandLineParser(string[] args)
        {
            CommandsList = args.Select(arg => ArgsSpliter(arg)).ToList();
        }

        public InputResult Parse()
        {
            var ires = new InputResult();

            foreach (SplitArg arg in CommandsList)
            {
                ires.originalFilePath = (arg.command == ORIGIN) ? arg.data : ires.originalFilePath;
                ires.modifiedFilePath = (arg.command == MODIFIED) ? arg.data : ires.modifiedFilePath;
                ires.helpAsked = arg.command == HELP;
            }

            ires.errors = ContainsErrors();
            return ires;
        }

        private List<string> ContainsErrors()
        {
            var errList = new List<string>();
            if(CommandsList.Count() == 0)
            {
                errList.Add(nameof(ArgErrorType.NoArguments));
            }

            if(CommandsList.Any(arg => !implementedCommands.Contains(arg.command)))
            {
                errList.Add(nameof(ArgErrorType.WrongCommand));
            }

            foreach (SplitArg arg in CommandsList)
            {
                if (!Directory.Exists(arg.data) && arg.command == MODIFIED)
                {
                    errList.Add(nameof(ArgErrorType.WrongModifiedDirectory));
                }
                else if (!Directory.Exists(arg.data) && arg.command == ORIGIN)
                {
                    errList.Add(nameof(ArgErrorType.WrongOriginalDirectory));
                }

                if (arg.command == MODIFIED && !CommandsList.Exists(a => a.command.Equals(ORIGIN))){
                    errList.Add(nameof(ArgErrorType.WrongOriginalDirectory));
                }
                else if (arg.command == ORIGIN && !CommandsList.Exists(a => a.command.Equals(MODIFIED)))
                {
                    errList.Add(nameof(ArgErrorType.WrongModifiedDirectory));
                }
            }
            return errList;
        }

        private SplitArg ArgsSpliter (string arg)
        {
            return new SplitArg
            {
                command = (ExtractSuffix(arg) != null) ? ExtractPrefix(arg.Substring(0,arg.IndexOf(dataIdentifier))): ExtractPrefix(arg),
                data = ExtractSuffix(arg),
            };
        }

        private string ExtractPrefix(string key)
        {
            return (key.Contains(commandIdentifier)&&key!=null) ? key.Remove(0, commandIdentifier.Length) : null;
        }

        private string ExtractSuffix(string key)
        {
            return (key.Contains(dataIdentifier)&&key!=null) ? key.Remove(0, key.IndexOf(dataIdentifier)+1) : null;
        }

        private List<SplitArg> CommandsList { get; set; }
    }
}
