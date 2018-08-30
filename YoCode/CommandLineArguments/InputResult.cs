using System.Collections.Generic;
using System.Linq;

namespace YoCode
{
    public class InputResult : IInputResult
    {
        public List<string> Errors { get; set; }
        public bool HasErrors => Errors.Any();
        public bool HelpAsked { get; set; }
        public bool NoLoadingScreen { get; set; }
        public bool JuniorTest { get; set; }
        public string InputFilePath { get; set; }
        public string OutputFilePath { get; set; }
        public bool CreateHtmlReport { get; set; } = true;
        public bool OpenHtmlReport { get; set; } = true;
    }
}