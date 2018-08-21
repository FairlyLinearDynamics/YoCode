using System;
using System.Collections.Generic;
using System.Text;

namespace YoCode
{
    class NullErrorObject : IErrorReporter
    {
        public void PrintErrors(IEnumerable<string> errors)
        {
            return;
        }
    }
}
