using System.Collections.Generic;
using System.Linq;

namespace YoCode
{
    public class InputResult : IInputResult
    {
        public List<string> Errors { get; set; }
        public bool HasErrors => Errors.Any();
        public bool HelpAsked { get; set; }
        public bool Silent { get; set; }
        public bool NoLoadingScreen { get; set; }
        public string originalFilePath;
        public string modifiedFilePath;
    }
}