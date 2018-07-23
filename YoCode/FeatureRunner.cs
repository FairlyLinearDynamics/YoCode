using System;
using System.IO;

namespace YoCode
{
    public static class FeatureRunner
    {
        public static FeatureEvidence Execute(ProcessDetails processDetails, string featureTitle)
        {
            pr = new ProcessRunner(processDetails.ProcessName, processDetails.WorkingDir, processDetails.Arguments);
            pr.ExecuteTheCheck(waitForMessage, kill);
            var evidence = new FeatureEvidence
            {
                Output = pr.Output,
                ErrorOutput = pr.ErrorOutput
            };

            if (pr.TimedOut)
            {
                evidence.SetFailed("Timed out");
            }
            return evidence;
        }

        public void EndProcess()
        {
            pr.KillLiveProcess();
        }
    }
}