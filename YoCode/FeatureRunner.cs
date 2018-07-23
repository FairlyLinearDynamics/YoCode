namespace YoCode
{
    public interface IFeatureRunner
    {
        FeatureEvidence Execute(ProcessDetails processDetails);
    }

    public class FeatureRunner : IFeatureRunner
    {
        public FeatureEvidence Execute(ProcessDetails processDetails)
        {
            var pr = new ProcessRunner(processDetails.ProcessName, processDetails.WorkingDir, processDetails.Arguments);
            pr.ExecuteTheCheck();
            var evidence = new FeatureEvidence
            {
                Output = pr.Output,
            };

            if (pr.TimedOut)
            {
                evidence.SetFailed("Timed out");
            }
            return evidence;
        }
    }
}