using System;
using System.Collections.Generic;
using System.Text;

namespace YoCode
{
    interface IPrint
    {
        void PrintLine(string text);
        void PrintFinalResults(List<FeatureEvidence> featureList);
    }
}
