using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace YoCode
{
    internal static class CommandErrorChecking
    {
        public static List<string> ContainsErrors(List<SplitArg> currentCommands, List<string> implementedCommands)
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

            if (!currentCommands.Any(a => a.command == CommandNames.INPUT))
            {
                errList.Add(nameof(ArgErrorType.WrongInputDirectory));
            }

            foreach (SplitArg arg in currentCommands)
            {
                if (!Directory.Exists(arg.data) && arg.command == CommandNames.INPUT)
                {
                    errList.Add(nameof(ArgErrorType.WrongInputDirectory));
                }
            }

            return errList;
        }
    }
}
