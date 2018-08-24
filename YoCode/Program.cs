using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System;

namespace YoCode
{
    internal static class Program
    {
        private static bool showLoadingAnim;
        private static bool isJunior;

        public static bool OpenHTMLOnFinish { get; set; }
        public static string OutputTo { get; set; }
        public static bool GenerateHtml { get; set; }

        private static void Main(string[] args)
        {
            var outputs = new List<IPrint> { new WebWriter(), new ConsoleWriter() };

            var compositeOutput = new Output(new CompositeWriter(outputs), (IErrorReporter)outputs.Find(a => a is ConsoleWriter));

            var commandLinehandler = new CommandLineParser(args);
            var result = commandLinehandler.Parse();

            var parameters = new RunParameterChecker(compositeOutput, result, new AppSettingsBuilder());

            OpenHTMLOnFinish = !result.Silent;
            OutputTo = result.OutputFilePath;
            GenerateHtml = !result.NoHtml;
            isJunior = result.JuniorTest;

            if (!parameters.ParametersAreValid())
            {
                if (!result.HelpAsked)
                {
                    compositeOutput.ShowInputErrors(parameters.Errs);
                }
                else
                {
                    compositeOutput.ShowHelp();
                }
                return;
            }

            var modifiedTestDirPath = result.InputFilePath;

            var dir = new PathManager(modifiedTestDirPath);

            showLoadingAnim = !result.NoLoadingScreen;

            var evidenceList = new List<FeatureEvidence>();

            var projectRunner = PassGatewayChecks(dir, evidenceList);
            if (projectRunner == null)
            {
                compositeOutput.PrintFinalResults(evidenceList, 0);
                return;
            }

            evidenceList = PerformChecks(dir, parameters, projectRunner);
            compositeOutput.PrintFinalResults(evidenceList.OrderBy(a => a.FeatureTitle),
                new Results(evidenceList, isJunior ? TestType.Junior : TestType.Original).FinalScore);
        }

        private static ProjectRunner PassGatewayChecks(IPathManager dir, ICollection<FeatureEvidence> evidenceList)
        {
            var fileCheck = new FileChangeFinder(dir.ModifiedTestDirPath);
            evidenceList.Add(fileCheck.FileChangeEvidence);
            if (fileCheck.FileChangeEvidence.FeatureFailed)
            {
                return null;
            }

            var projectBuilder = new ProjectBuilder(dir.ModifiedTestDirPath, new FeatureRunner());
            evidenceList.Add(projectBuilder.ProjectBuilderEvidence);
            if (projectBuilder.ProjectBuilderEvidence.FeatureFailed)
            {
                return null;
            }

            var projectRunner = new ProjectRunner(dir.ModifiedTestDirPath, new FeatureRunner());
            ConsoleCloseHandler.StartHandler(projectRunner);
            projectRunner.Execute();
            evidenceList.Add(projectRunner.ProjectRunEvidence);
            return projectRunner.ProjectRunEvidence.FeatureFailed ? null : projectRunner;
        }

        private static List<FeatureEvidence> PerformChecks(IPathManager dir, RunParameterChecker p, ProjectRunner projectRunner)
        {
            var checkList = new List<FeatureEvidence>();

            var workThreads = new List<Thread>();

            if (showLoadingAnim)
            {
                var loadingThread = new Thread(LoadingAnimation.RunLoading)
                {
                    IsBackground = true
                };
                workThreads.Add(loadingThread);
                loadingThread.Start();
            }

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
                checkList.Add(new DuplicationCheck(dir, new DupFinder(p.CMDToolsPath), isJunior).DuplicationEvidence);
            });
            workThreads.Add(dupFinderThread);
            dupFinderThread.Start();

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
