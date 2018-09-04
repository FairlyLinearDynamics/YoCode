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

        public ProjectRunner PassGatewayChecks(ICollection<FeatureEvidence> evidenceList)
        {
            var fileCheck = new FileChangeFinder(checkConfig);
            if (fileCheck.FileChangeEvidence.Failed)
            {
                evidenceList.Add(fileCheck.FileChangeEvidence);
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

            // CodeCoverage check
            var codeCoverage = new Thread(() =>
            {
                checkList.Add(new CodeCoverageCheck(checkConfig).CodeCoverageEvidence);
            });
            workThreads.Add(codeCoverage);
            codeCoverage.Start();

            // Duplication check
            var dupcheck = new DuplicationCheckRunner(checkConfig);

            var dupFinderThread = new Thread(() =>
            {
                checkList.Add(dupcheck.AppDuplicationEvidence);
                checkList.Add(dupcheck.TestDuplicationEvidence);
            });
            workThreads.Add(dupFinderThread);
            dupFinderThread.Start();

            //File Change
            var fileCheck = new FileChangeFinder(checkConfig);
            checkList.Add(fileCheck.FileChangeEvidence);

            // UI test
            checkList.Add(new UICodeCheck(UIKeywords.MILE_KEYWORDS, checkConfig).UIEvidence);

            // Git repo used
            checkList.Add(new GitCheck(checkConfig).GitEvidence);

            // Unit test test
            checkList.Add(new TestCountCheck(checkConfig).UnitTestEvidence);

            //Front End Check
            checkList.AddRange(new UICheck(projectRunner.GetPort()).UIFeatureEvidences);

            var ucc = new UnitConverterCheck(projectRunner.GetPort());

            // Unit converter test
            checkList.Add(ucc.UnitConverterCheckEvidence);

            checkList.Add(ucc.BadInputCheckEvidence);

            workThreads.Where(a=>a.Name!="loadingThread").ToList().ForEach(a => a.Join());
            projectRunner.KillProject();

            LoadingAnimation.LoadingFinished = true;
            workThreads.ForEach(a => a.Join());

            projectRunner.ReportLefOverProcess();
            return checkList;
        }
    }
}
