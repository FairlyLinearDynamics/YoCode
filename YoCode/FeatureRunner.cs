using System;
using System.IO;

namespace YoCode
{
    public interface IFeatureRunner
    {
        FeatureEvidence Execute(ProcessDetails processDetails, string featureTitle);
    }

    public class FeatureRunner : IFeatureRunner
    {
        public FeatureEvidence Execute(ProcessDetails processDetails, string featureTitle)
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