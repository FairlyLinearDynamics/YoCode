using System;
using System.Collections.Generic;
using System.Linq;

namespace YoCode
{
    public class PrintToConsole : IPrint
    {
        public void PrintFinalResults(List<FeatureEvidence> featureList)
        {
            throw new NotImplementedException();
        }

        public void PrintLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}
