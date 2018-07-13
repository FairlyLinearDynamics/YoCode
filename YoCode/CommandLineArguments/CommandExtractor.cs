using System;
using System.Collections.Generic;
using System.Text;

namespace YoCode
{
    class CommandExtractor
    {
        public static SplitArg ArgsSplitter(string arg)
        {
            return new SplitArg
            {
                command = (ExtractSuffix(arg,CommandIdentifiers.dataIdentifier) != null) 
                ? ExtractPrefix(arg.Substring(0, arg.IndexOf(CommandIdentifiers.dataIdentifier)), CommandIdentifiers.commandIdentifier) 
                : ExtractPrefix(arg, CommandIdentifiers.commandIdentifier),

                data = ExtractSuffix(arg, CommandIdentifiers.dataIdentifier),
            };
        }

        private static string ExtractPrefix(string key, string commandIdentifier)
        {
            return (key.Contains(commandIdentifier) && key != null) ? key.Remove(0, commandIdentifier.Length) : null;
        }

        private static string ExtractSuffix(string key, string dataIdentifier)
        {
            return (key.Contains(dataIdentifier) && key != null) ? key.Remove(0, key.IndexOf(dataIdentifier) + 1) : null;
        }
    }
}
