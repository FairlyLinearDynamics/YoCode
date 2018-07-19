using System.IO;

namespace YoCode
{
    public class DupFinder : IDupFinder
    {
        private FeatureRunner featureRunner;
        private readonly string CMDtoolsDir;
        private readonly string CMDtoolFileName = "dupfinder.exe";
        private readonly string outputFile = "report.xml";
        private readonly string outputArg = " -o=\"";
        private readonly string processName;
        private readonly string workingDir;

        public DupFinder(FeatureRunner featureRunner, string CMDtoolsDirConfig)
        {
            CMDtoolsDir = CMDtoolsDirConfig;
            processName = Path.Combine(CMDtoolsDir, CMDtoolFileName);
            workingDir = CMDtoolsDir;
            this.featureRunner = featureRunner;
        }

        public FeatureEvidence Execute(string featureTitle, string solutionPath)
        {
            var proc = new ProcessDetails(processName, workingDir, solutionPath + outputArg + outputFile);
            var evidence = featureRunner.Execute(proc, featureTitle);
            evidence.Output = File.ReadAllText(Path.Combine(workingDir, outputFile));
            return evidence;
        }
    }
}