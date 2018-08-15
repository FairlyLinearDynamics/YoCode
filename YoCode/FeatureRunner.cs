namespace YoCode
{
    internal class FeatureRunner
    {
        private ProcessRunner pr;

        public FeatureEvidence Execute(ProcessDetails processDetails, string waitForMessage = null, bool kill =true)
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
            pr.KillCurrentProcessWithChildren();
        }
    }
}