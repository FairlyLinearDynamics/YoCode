using System.Collections.Generic;

namespace YoCode
{
    interface IErrorReporter
    {
        void PrintErrors(IEnumerable<string> errors);
    }
}
