using System;
using System.Collections.Generic;
using System.Text;

namespace YoCode
{
    interface IPrint
    {
        void AddIntro(string intro);
        void AddFeature(FeatureData data);
        void AddMessage(string message);
        void AddErrs(IEnumerable<string> errors);
        void AddBanner();
        void WriteReport();
    }
}
