namespace YoCode
{
    internal class FeatureRunner
    {
        private ProcessRunner pr;

        public ProcessOutput Execute(ProcessDetails processDetails, string waitForMessage = null, bool kill = true)
        {
            pr = new ProcessRunner(processDetails.ProcessName, processDetails.WorkingDir, processDetails.Arguments);
            pr.ExecuteTheCheck(waitForMessage, kill);
            var a = pr.procinfo;
            var evidence = new ProcessOutput
            {
                Output = pr.Output,
                ErrorOutput = pr.ErrorOutput
            };

            if (pr.TimedOut)
            {
                return default(ProcessOutput);
            }
            return evidence;
        }

        public void EndProcess()
        {
            pr?.KillCurrentProcessWithChildren();
        }

        public void FindLeftOverProcess()
        {
            pr?.FindLeftOverProcess();
        }
    }
}