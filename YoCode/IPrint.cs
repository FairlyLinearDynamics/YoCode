using System;
using System.Collections.Generic;
using System.Text;

namespace YoCode
{
    interface IPrint
    {
        void AddIntro(string intro);
        void AddFeature(FeatureData data);
        void AddMessage(string msg);
        void AddErr(string err);
        void WriteReport();
    }
}
