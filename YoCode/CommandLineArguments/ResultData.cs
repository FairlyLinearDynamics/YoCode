using System.Collections.Generic;
using System.Linq;

namespace YoCode
{
    public class ResultData
    {
        public List<string> errors;
        public bool hasErrors => errors.Any();
        public string errType;
        public bool helpAsked;
        public string originalFilePath;
        public string modifiedFilePath;
    }
}