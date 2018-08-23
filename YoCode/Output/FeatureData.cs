using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoCode
{
    public struct FeatureData
    {
        public string title;
        public bool? featurePass;
        public List<string> evidence;
        public double score;
        public string featureResult;
    }
}
