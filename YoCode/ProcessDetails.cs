namespace YoCode
{
    public class ProcessDetails
    {
        public ProcessDetails(string processName, string workingDir, string arguments)
        {
            ProcessName = processName;
            WorkingDir = workingDir;
            Arguments = arguments;
        }

        public string ProcessName { get; }
        public string WorkingDir { get; }
        public string Arguments { get; }
    }
}