using System;
using System.IO;

namespace YoCode
{
    public class FeatureRunner
    {
        ProcessRunner pr;

        public FeatureEvidence Execute(ProcessDetails processDetails)
        {
            pr = new ProcessRunner(processDetails.ProcessName, processDetails.WorkingDir, processDetails.Arguments);
            pr.ExecuteTheCheck();
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