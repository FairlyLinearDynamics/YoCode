using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace YoCode
{
    internal class CheckManager
    {
        private readonly List<Thread> workThreads;
        private readonly CheckConfig checkConfig;

        public CheckManager(List<Thread> workThreads, CheckConfig checkConfig)
        {
            this.workThreads = workThreads;
            this.checkConfig = checkConfig;
        }

        public ProjectRunner PassGatewayChecks(List<FeatureEvidence> evidenceList)
        {
            var fileCheck = new FileChangeFinder(checkConfig);
            var fileChangeEvidence = fileCheck.Execute().ToArray();
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

        public List<FeatureEvidence> PerformChecks(ProjectRunner projectRunner)
        {
            var checkList = new List<FeatureEvidence>();

            var codeCoverageCheck = new CodeCoverageCheck(checkConfig);
            var dupcheck = new DuplicationCheckRunner(checkConfig);
            var fileCheck = new FileChangeFinder(checkConfig);
            var uiCodeCheck = new UICodeCheck(UIKeywords.MILE_KEYWORDS, checkConfig);
            var gitCheck = new GitCheck(checkConfig);
            var testCountCheck = new TestCountCheck(checkConfig);
            var uiCheck = new UICheck(projectRunner.GetPort());
            var ucc = new UnitConverterCheck(projectRunner.GetPort());

            checkList.AddRange(codeCoverageCheck.Execute());
            checkList.AddRange(dupcheck.Execute());
            checkList.AddRange(fileCheck.Execute());
            checkList.AddRange(uiCodeCheck.Execute());
            checkList.AddRange(gitCheck.Execute());
            checkList.AddRange(testCountCheck.Execute());
            checkList.AddRange(uiCheck.Execute());
            checkList.AddRange(ucc.Execute());

            workThreads.Where(a=>a.Name!="loadingThread").ToList().ForEach(a => a.Join());
            projectRunner.KillProject();

            LoadingAnimation.LoadingFinished = true;
            workThreads.ForEach(a => a.Join());

            projectRunner.ReportLefOverProcess();
            return checkList;
        }
    }
}
