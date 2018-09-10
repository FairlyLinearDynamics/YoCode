using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoCode
{
    internal class CheckManager
    {
        private readonly CheckConfig checkConfig;

        public CheckManager(CheckConfig checkConfig)
        {
            this.checkConfig = checkConfig;
        }

        public async Task<List<FeatureEvidence>> PerformChecks()
        {
            var projectBuilder = new ProjectBuilder(checkConfig.PathManager.ModifiedTestDirPath, new FeatureRunner());
            var projectBuilderTask = projectBuilder.Execute();

            var projectRunner = new ProjectRunner(checkConfig.PathManager.ModifiedTestDirPath, new FeatureRunner(), projectBuilderTask);
            var projectRunnerTask = projectRunner.Execute();
            var portTask = projectRunner.GetPort();

            var checks = new ICheck[]
            {
                new CodeCoverageCheck(checkConfig, projectBuilderTask),
                new DuplicationCheckRunner(checkConfig),
                new FileChangeFinder(checkConfig),
                new UICodeCheck(UIKeywords.MILE_KEYWORDS, checkConfig),
                new GitCheck(checkConfig),
                new TestCountCheck(checkConfig, projectBuilderTask),
                new UICheck(portTask, projectRunnerTask),
                new UnitConverterCheck(portTask, projectRunnerTask),
                new BadInputCheck(portTask, projectRunnerTask)
            };

            var featureTasks = checks.Select(c => c.Execute()).ToArray();
            var featureEvidences = await Task.WhenAll(featureTasks);

            projectRunner.KillProject();

            projectRunner.ReportLefOverProcess();
            return featureEvidences.SelectMany(x => x).ToList();
        }
    }
}
