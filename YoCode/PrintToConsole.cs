using System;
using System.Collections.Generic;
using System.Linq;

namespace YoCode
{
    public class PrintToConsole : IPrint
    {
        string textToPrint;

        public void AddNewLine(string text)
        {
            textToPrint += text + Environment.NewLine;
        }

        public void PrintMessage()
        {
            Console.Write(textToPrint);
            textToPrint = null;
        }

        public void PrintDiv()
        {
            textToPrint += messages.Divider;
        }
    }
}
