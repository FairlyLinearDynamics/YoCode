using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace YoCode
{
    class CommandLineParser
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

        public ResultData Parse()
        {
            var ires = new ResultData();

            foreach (SplitArg arg in CommandsList)
            {
                ires.originalFilePath = (arg.command == ORIGIN) ? arg.data : ires.originalFilePath;
                ires.modifiedFilePath = (arg.command == MODIFIED) ? arg.data : ires.modifiedFilePath;
                ires.helpAsked = arg.command == HELP;
            }

            ires.hasErrors = ContainsErrors(ires);

            return ires;
        }

        private bool ContainsErrors(ResultData res)
        {
            if(CommandsList.Count() == 0)
            {
                res.errType = ArgErrorType.NoArgs;
                return true;
            }

            if(CommandsList.Any(arg => !implementedCommands.Contains(arg.command)))
            {
                res.errType = ArgErrorType.WrongCommand;
                return true;
            }

            foreach (SplitArg arg in CommandsList)
            {
                if (CommandsList.Exists(a => a.command.Equals(ORIGIN) || a.command.Equals(MODIFIED)))
                {
                    if (!Directory.Exists(arg.data) && arg.command == MODIFIED)
                    {
                        res.errType = ArgErrorType.WrongModifiedDirectory;
                        return true;
                    }
                    else if (!Directory.Exists(arg.data) && arg.command == ORIGIN)
                    {
                        res.errType = ArgErrorType.WrongModifiedDirectory;
                        return true;
                    }
                }
            }

            return false;
        }

        public SplitArg ArgsSpliter (string arg)
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

public struct SplitArg
{
    public string command;
    public string data;
}

public enum ArgErrorType
{
    NoArgs,
    WrongOriginalDirectory,
    WrongModifiedDirectory,
    WrongCommand
}

public struct ResultData{
    public bool hasErrors;
    public ArgErrorType errType;
    public bool helpAsked;
    public string originalFilePath;
    public string modifiedFilePath;
}
