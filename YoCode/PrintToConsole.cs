using System;
using System.Collections.Generic;
using System.Linq;

namespace YoCode
{
    public class PrintToConsole : IPrint
    {
        string textToPrint;

        public bool AddNewLine(string text)
        {
            textToPrint += text + Environment.NewLine;
            return true;
        }

        public void PrintMessage()
        {
            if (textToPrint != null)
            {
                Console.Write(textToPrint);
                textToPrint = null;
            }
        }

        public void PrintDiv()
        {
            textToPrint += messages.Divider;
        }
    }
}
