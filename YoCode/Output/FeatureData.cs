using LibGit2Sharp;
using System.Collections.Generic;

namespace YoCode
{
    public struct FeatureData
    {
        public string title;
        public bool? featurePass;
        public List<string> evidence;
        public double score;
        public string featureResult;
        public string featureHelperMessage;
        public Patch FilesChanged { get; set; }
    }
}
