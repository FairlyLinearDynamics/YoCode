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

        public async Task<ProjectRunner> PassGatewayChecksAsync(List<FeatureEvidence> evidenceList)
        {
            var fileCheck = new FileChangeFinder(checkConfig);
            var fileChangeEvidence = (await fileCheck.Execute()).ToArray();
            if (fileChangeEvidence.Any(e => e.Failed))
            {
                evidenceList.AddRange(fileChangeEvidence);
                return null;
            }

            var projectBuilder = new ProjectBuilder(checkConfig.PathManager.ModifiedTestDirPath, new FeatureRunner());
            if (projectBuilder.ProjectBuilderEvidence.Failed)
            {
                evidenceList.Add(projectBuilder.ProjectBuilderEvidence);
                return null;
            }

            var projectRunner = new ProjectRunner(checkConfig.PathManager.ModifiedTestDirPath, new FeatureRunner());
            ConsoleCloseHandler.StartHandler(projectRunner);
            projectRunner.Execute();
            if(projectRunner.ProjectRunEvidence.Failed)
            {
                evidenceList.Add(projectRunner.ProjectRunEvidence);
                return null;
            }
            return projectRunner;
        }

        public async Task<List<FeatureEvidence>> PerformChecks(ProjectRunner projectRunner)
        {
            var checks = new ICheck[]
            {
                new CodeCoverageCheck(checkConfig),
                new DuplicationCheckRunner(checkConfig),
                new FileChangeFinder(checkConfig),
                new UICodeCheck(UIKeywords.MILE_KEYWORDS, checkConfig),
                new GitCheck(checkConfig),
                new TestCountCheck(checkConfig),
                new UICheck(projectRunner.GetPort()),
                new UnitConverterCheck(projectRunner.GetPort())
            };

            var featureTasks = checks.Select(c => c.Execute()).ToArray();
            var featureEvidences = await Task.WhenAll(featureTasks);

            projectRunner.KillProject();

            projectRunner.ReportLefOverProcess();
            return featureEvidences.SelectMany(x => x).ToList();
        }
    }
}
