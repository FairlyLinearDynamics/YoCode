using System.Collections.Generic;
using System.Linq;

namespace YoCode
{
    public class InputResult
    {
        public List<string> errors;
        public bool HasErrors => errors.Any();
        public bool helpAsked;
        public string originalFilePath;
        public string modifiedFilePath;
    }
}