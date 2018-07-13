using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace YoCode
{
    class CommandErrorChecking
    {
        public static List<string> ContainsErrors(List<SplitArg> currentCommands,List<string> implementedCommands)
        {
            var errList = new List<string>();

            if (currentCommands.Count() == 0)
            {
                errList.Add(nameof(ArgErrorType.NoArguments));
            }

            if (currentCommands.Any(arg => !implementedCommands.Contains(arg.command)))
            {
                errList.Add(nameof(ArgErrorType.WrongCommand));
            }

            foreach (SplitArg arg in currentCommands)
            {
                if (!Directory.Exists(arg.data) && arg.command == CommandNames.MODIFIED)
                {
                    errList.Add(nameof(ArgErrorType.WrongModifiedDirectory));
                }
                else if (!Directory.Exists(arg.data) && arg.command == CommandNames.ORIGIN)
                {
                    Console.Write("INVALID DIRECTORY");
                    errList.Add(nameof(ArgErrorType.WrongOriginalDirectory));
                }

                if (arg.command == CommandNames.MODIFIED && !currentCommands.Exists(a => a.command.Equals(CommandNames.ORIGIN)))
                {
                    errList.Add(nameof(ArgErrorType.WrongOriginalDirectory));
                }
                else if (arg.command == CommandNames.ORIGIN && !currentCommands.Exists(a => a.command.Equals(CommandNames.MODIFIED)))
                {
                    errList.Add(nameof(ArgErrorType.WrongModifiedDirectory));
                }
            }

            return errList;
        }
    }
}
