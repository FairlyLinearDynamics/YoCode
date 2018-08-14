using System.IO;

namespace YoCode
{
    internal class DupFinder : IDupFinder
    {
        private readonly string CMDtoolsDir;
        private readonly string CMDtoolFileName = "dupfinder.exe";
        private readonly string outputFile = "report.xml";
        private readonly string outputArg = " -o=\"";
        private readonly string processName;
        private readonly string workingDir;

        public DupFinder(string CMDtoolsDirConfig)
        {
            CMDtoolsDir = CMDtoolsDirConfig;
            processName = Path.Combine(CMDtoolsDir, CMDtoolFileName);
            workingDir = CMDtoolsDir;
        }

        public FeatureEvidence Execute(string featureTitle, string solutionPath)
        {
            var proc = new ProcessDetails(processName, workingDir, solutionPath + outputArg + outputFile);
            var evidence = new FeatureRunner().Execute(proc);
            evidence.Output = File.ReadAllText(Path.Combine(workingDir, outputFile));
            return evidence;
        }
    }
}