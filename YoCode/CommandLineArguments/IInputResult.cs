using System.Collections.Generic;

namespace YoCode
{
    public interface IInputResult
    {
        bool HasErrors { get; }
        List<string> Errors { get; set; }
        bool HelpAsked { get; set; }
        bool CreateHtmlReport { get; set; }
        bool OpenHtmlReport { get; set; }
    }
}