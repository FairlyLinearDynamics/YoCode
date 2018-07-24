using System;
using System.Collections.Generic;
using System.Linq;

namespace YoCode
{
    public class ConsoleWriter/* : IPrint*/
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

        public void AddIntro(string intro)
        {
            throw new NotImplementedException();
        }

        public void AddFeature(FeatureData data)
        {
            throw new NotImplementedException();
        }

        public void AddMessage(string msg)
        {
            throw new NotImplementedException();
        }

        public void AddErrs(string err)
        {
            throw new NotImplementedException();
        }

        public void WriteReport()
        {
            throw new NotImplementedException();
        }
    }
}
