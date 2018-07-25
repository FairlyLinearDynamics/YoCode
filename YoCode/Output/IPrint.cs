using System;
using System.Collections.Generic;
using System.Text;

namespace YoCode
{
    interface IPrint
    {
        void AddIntro(string intro);
        void AddFeature(FeatureData data, bool featurePass);
        void AddMessage(string msg);
        void AddErrs(IEnumerable<string> err);
        void WriteReport();
    }
}
