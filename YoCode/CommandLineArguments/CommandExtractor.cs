using System;

namespace YoCode
{
    static class CommandExtractor
    {
        public static SplitArg ArgsSplitter(string arg)
        {
            var argParts = arg.Split(new[] { CommandIdentifiers.commandIdentifier, CommandIdentifiers.dataIdentifier }, StringSplitOptions.RemoveEmptyEntries);
            return new SplitArg
            {
                command =  argParts[0] ,
                data = (argParts.Length>1) ? argParts[1] : null,
            };
        }
    }
}
