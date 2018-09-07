using System;
using System.IO;
using static YoCode.FeatureRunner;

namespace YoCode
{
    internal class DupFinder : IDupFinder
    {
        private const string cmdToolFileName = "dupfinder.exe";
        private const string arguments = " --discard-cost=5 -o=\"";
        private readonly string processName;
        private readonly string workingDir;

        public DupFinder(string cmdToolsDirConfig)
        {
            var cmdToolsDir = cmdToolsDirConfig;
            processName = Path.Combine(cmdToolsDir, cmdToolFileName);
            workingDir = cmdToolsDir;
        }

        public ProcessOutput Execute(string solutionPath)
        {
            try
            {
                var outputFilePath = Path.GetTempFileName();

                try
                {
                    var proc = new ProcessDetails(processName, workingDir, solutionPath + arguments + outputFilePath);
                    var processOutput = new FeatureRunner().Execute(proc);
                    processOutput.Output = File.ReadAllText(outputFilePath);
                    return processOutput;
                }
                finally
                {
                    File.Delete(outputFilePath);
                }
            }
            catch (IOException)
            {
                return default;
            }
        }
    }
}