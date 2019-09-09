using System;
using System.IO;

namespace YoCode
{
    internal class DupFinder : IDupFinder
    {
        private readonly ToolPath toolPath;
        private const string arguments = " --discard-cost=5 -o=\"";

        public DupFinder(ToolPath toolPath)
        {
            this.toolPath = toolPath;
        }

        public ProcessOutput Execute(string solutionPath)
        {
            try
            {
                var outputFilePath = Path.GetTempFileName();

                try
                {
                    var proc = new ProcessDetails(toolPath.FullPath, Path.GetTempPath(), solutionPath + arguments + outputFilePath);
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