using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace YoCode
{
    internal class CheckManager
    {
        private readonly IPathManager dir;
        private readonly List<Thread> workThreads;
        private readonly bool isJunior;

        public CheckManager(IPathManager dir, List<Thread> workThreads, bool isJunior)
        {
            this.dir = dir;
            this.workThreads = workThreads;
            this.isJunior = isJunior;
        }

        public ProjectRunner PassGatewayChecks(ICollection<FeatureEvidence> evidenceList)
        {
            var fileCheck = new FileChangeFinder(dir.ModifiedTestDirPath);
            if (fileCheck.FileChangeEvidence.Failed)
            {
                evidenceList.Add(fileCheck.FileChangeEvidence);
                return null;
            }

            var projectBuilder = new ProjectBuilder(dir.ModifiedTestDirPath, new FeatureRunner());
            if (projectBuilder.ProjectBuilderEvidence.Failed)
            {
                evidenceList.Add(projectBuilder.ProjectBuilderEvidence);
                return null;
            }

            var projectRunner = new ProjectRunner(dir.ModifiedTestDirPath, new FeatureRunner());
            ConsoleCloseHandler.StartHandler(projectRunner);
            projectRunner.Execute();
            if(projectRunner.ProjectRunEvidence.Failed)
            {
                evidenceList.Add(projectRunner.ProjectRunEvidence);
                return null;
            }
            return projectRunner;
        }

        public List<FeatureEvidence> PerformChecks(RunParameterChecker p, ProjectRunner projectRunner)
        {
            var checkList = new List<FeatureEvidence>();

            // CodeCoverage check
            var codeCoverage = new Thread(() =>
            {
                checkList.Add(new CodeCoverageCheck(p.DotCoverDir, dir.ModifiedTestDirPath, new FeatureRunner()).CodeCoverageEvidence);
            });
            workThreads.Add(codeCoverage);
            codeCoverage.Start();

            // Duplication check
            var dupFinderThread = new Thread(() =>
            {
                checkList.Add(new DuplicationCheck(dir, new DupFinder(p.CMDToolsPath), p).DuplicationEvidence);
            });
            workThreads.Add(dupFinderThread);
            dupFinderThread.Start();

            //File Change
            var fileCheck = new FileChangeFinder(dir.ModifiedTestDirPath);
            checkList.Add(fileCheck.FileChangeEvidence);

            // UI test
            var modifiedHtmlFiles = dir.GetFilesInDirectory(dir.ModifiedTestDirPath, FileTypes.html).ToList();

            checkList.Add(new UICheck(modifiedHtmlFiles, UIKeywords.UNIT_KEYWORDS).UIEvidence);

            // Git repo used
            checkList.Add(new GitCheck(dir.ModifiedTestDirPath).GitEvidence);

            // Unit test test
            checkList.Add(new TestCountCheck(dir.ModifiedTestDirPath, new FeatureRunner()).UnitTestEvidence);

            //Front End Check
            checkList.Add(new FrontEndCheck(projectRunner.GetPort(), UIKeywords.UNIT_KEYWORDS).FrontEndEvidence);

            var ucc = new UnitConverterCheck(projectRunner.GetPort());

            // Unit converter test
            checkList.Add(ucc.UnitConverterCheckEvidence);

            checkList.Add(ucc.BadInputCheckEvidence);

            LoadingAnimation.LoadingFinished = true;
            workThreads.ForEach(a => a.Join());
            projectRunner.KillProject();

            projectRunner.ReportLefOverProcess();
            return checkList;
        }
    }
}
