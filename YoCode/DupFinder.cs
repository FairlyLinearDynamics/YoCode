using System;
using System.IO;

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

        public FeatureEvidence Execute(string featureTitle, string solutionPath)
        {
            try
            {
                var outputFilePath = Path.GetTempFileName();

                try
                {
                    var proc = new ProcessDetails(processName, workingDir, solutionPath + arguments + outputFilePath);
                    var evidence = new FeatureRunner().Execute(proc);
                    evidence.Output = File.ReadAllText(outputFilePath);
                    return evidence;
                }
                finally
                {
                    File.Delete(outputFilePath);
                }
            }
            catch (IOException e)
            {
                var featureEvidence = new FeatureEvidence();
                featureEvidence.SetInconclusive(new SimpleEvidenceBuilder(e.Message));
                return featureEvidence;
            }
        }
    }
}