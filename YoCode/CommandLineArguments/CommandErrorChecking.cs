using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Permissions;

namespace YoCode
{
    internal static class CommandErrorChecking
    {
        public static List<string> ContainsErrors(List<SplitArg> currentCommands,List<string> implementedCommands)
        {
            var errList = new List<string>();

            if (!currentCommands.Any())
            {
                errList.Add(nameof(ArgErrorType.NoArguments));
            }

            if (currentCommands.Any(arg => !implementedCommands.Contains(arg.command)))
            {
                errList.Add(nameof(ArgErrorType.WrongCommand));
            }

            if (!currentCommands.Any(a => a.command == CommandNames.MODIFIED))
            {
                errList.Add(nameof(ArgErrorType.WrongModifiedDirectory));
            }

            foreach (SplitArg arg in currentCommands)
            {
                if (!Directory.Exists(arg.data) && arg.command == CommandNames.MODIFIED)
                {
                    errList.Add(nameof(ArgErrorType.WrongModifiedDirectory));
                }
            }

            return errList;
        }
    }
}
