using System;
using System.Collections.Generic;
using System.Text;

namespace YoCode
{
    interface IPrint
    {
        void PrintIntroduction();
        void PrintFinalResults(List<FeatureEvidence> featureList);
    }
}
