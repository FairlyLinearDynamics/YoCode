using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace YoCode
{
    class CommandExtractor
    {
        public static SplitArg ArgsSplitter(string arg)
        {
            var argParts = arg.Split(new[] { CommandIdentifiers.commandIdentifier, CommandIdentifiers.dataIdentifier }, StringSplitOptions.RemoveEmptyEntries);
            return new SplitArg
            {
                //command = (ExtractSuffix(arg,CommandIdentifiers.dataIdentifier) != null) 
                //? ExtractPrefix(arg.Substring(0, arg.IndexOf(CommandIdentifiers.dataIdentifier)), CommandIdentifiers.commandIdentifier) 
                //: ExtractPrefix(arg, CommandIdentifiers.commandIdentifier),

                //data = ExtractSuffix(arg, CommandIdentifiers.dataIdentifier),
                command =  argParts[0] ,
                data = (argParts.Any()) ? argParts[1] : null,
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
