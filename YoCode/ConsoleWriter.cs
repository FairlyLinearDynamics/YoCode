using System;
using System.Collections.Generic;
using System.Linq;

namespace YoCode
{
    public class ConsoleWriter /*: IPrint*/
    {
        string textToPrint;

        public bool AddNewLine(string text)
        {
            textToPrint += text + Environment.NewLine;
            return true;
        }

        public void WriteAndFlush()
        {
            if (textToPrint != null)
            {
                Console.Write(textToPrint);
                textToPrint = null;
            }
        }

        public void AddDiv()
        {
            textToPrint += messages.Divider;
        }
    }
}
