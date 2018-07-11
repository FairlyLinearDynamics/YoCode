using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoCode
{
    class CommandLineParser
    {
        private ErrorList errorList;

        private List<string> commandList = new List<string> { "--" };

        public CommandLineParser()
        {
            errorList = new ErrorList();
        }

        public InputResults Parse(string[] args)
        {
            return new InputResults
            {
                //hasErrors = ErrsPresent(args),
                testInput = ExtractPrefix(args.First()),
                //originalFilePath = ExtractOriginal(),
                //modifiedFilePath = ExtractModified()
            };
        }

        private bool ErrsPresent(string[] args)
        {
            if (args.Length == 0)
            {
                errorList.NoArgs = true;
                return true;
            }
            foreach(string arg in args)
            {
                return ExtractPrefix(arg) == null ? true : false;
            }
            return false;
        }

        public string ExtractPrefix(string args)
        {
            return commandList.First(args.StartsWith);
        }

        private string ExtractOriginal(string args)
        {

            return "";
        }


        public string HelpMessage { get; set; }

        public void ShowHelp()
        {
            if (HelpMessage != null)
            {
                Console.WriteLine(HelpMessage);
            }
            else
            {
                // TODO: Create default help message
                Console.WriteLine("Default help message");
            }
        }
    }
}

public enum Commands{
    original,
    modified
}

public struct ErrorList
{
    public bool NoArgs;
    public bool WrongOriginalDirectory;
    public bool WrongModifiedDirectory;
    public bool WrongCommand;
}

public struct InputResults{
    public bool hasErrors;
    public string originalFilePath;
    public string modifiedFilePath;
    public string testInput;
}
