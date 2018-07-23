using System;
using System.Collections.Generic;
using System.Text;

namespace YoCode
{
    interface IOutputWriter
    {
        bool AddNewLine(string text);
        void WriteAndFlush();
        void AddDiv();
    }
}
