using System;
using System.IO;

namespace YoCode
{
    public static class FeatureRunner
    {
        public static FeatureEvidence Execute(ProcessDetails processDetails, string featureTitle)
        {
            var pr = new ProcessRunner(processDetails.ProcessName, processDetails.WorkingDir, processDetails.Arguments);
            pr.ExecuteTheCheck();
            var evidence = new FeatureEvidence
            {
                Output = pr.Output,
                FeatureTitle = featureTitle
            };

            if (pr.TimedOut)
            {
                evidence.SetFailed("Timed out");
            }
            return evidence;
        }
    }
}