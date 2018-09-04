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

            // CodeCoverage check
            var codeCoverage = new Thread(() =>
            {
                var codeCoverageCheck = new CodeCoverageCheck(checkConfig);
                var codeCoverageEvidence = codeCoverageCheck.Execute();
                checkList.AddRange(codeCoverageEvidence);
            });
            workThreads.Add(codeCoverage);
            codeCoverage.Start();

            // Duplication check
            var dupcheck = new DuplicationCheckRunner(checkConfig);

            var dupFinderThread = new Thread(() =>
            {
                checkList.AddRange(dupcheck.Execute());
            });
            workThreads.Add(dupFinderThread);
            dupFinderThread.Start();

            //File Change
            var fileCheck = new FileChangeFinder(checkConfig);
            checkList.AddRange(fileCheck.Execute());

            // UI test
            checkList.AddRange(new UICodeCheck(UIKeywords.MILE_KEYWORDS, checkConfig).Execute());

            // Git repo used
            checkList.AddRange(new GitCheck(checkConfig).Execute());

            // Unit test test
            checkList.AddRange(new TestCountCheck(checkConfig).Execute());

            //Front End Check
            checkList.AddRange(new UICheck(projectRunner.GetPort()).Execute());

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
