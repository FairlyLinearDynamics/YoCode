using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System;

namespace YoCode
{
    internal static class Program
    {
        private static ProjectRunner pr;
        private static bool showLoadingAnim;
        private static bool isJunior;

        private static void Main(string[] args)
        {
            var outputs = new List<IPrint> { new WebWriter(), new ConsoleWriter() };

            var compositeOutput = new Output(new CompositeWriter(outputs), (IErrorReporter)outputs.Find(a => a is ConsoleWriter));

            var commandLinehandler = new CommandLineParser(args);
            var result = commandLinehandler.Parse();

            var parameters = new RunParameterChecker(compositeOutput, result, new AppSettingsBuilder());

            OpenHTMLOnFinish = !result.Silent;
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

            var modifiedTestDirPath = result.modifiedFilePath;

            var dir = new PathManager(modifiedTestDirPath);

            pr = new ProjectRunner(dir.ModifiedTestDirPath, new FeatureRunner());

            ConsoleCloseHandler.StartHandler(pr);

            showLoadingAnim = !result.NoLoadingScreen;
            var implementedFeatureList = PerformChecks(dir, parameters);
            compositeOutput.PrintFinalResults(implementedFeatureList.OrderBy(a => a.FeatureTitle), new Results(implementedFeatureList, TestType.Junior).FinalScore);
            pr.ReportLefOverProcess();
        }

        public static bool OpenHTMLOnFinish { get; set; }

        private static List<FeatureEvidence> PerformChecks(PathManager dir, RunParameterChecker p)
        {
            var checkList = new List<FeatureEvidence>();

            var fileCheck = new FileChangeFinder(dir.ModifiedTestDirPath);

            // Files changed check
            checkList.Add(fileCheck.FileChangeEvidence);

            if (fileCheck.FileChangeEvidence.Evidence.Contains("No Files Changed"))
            {
                return checkList;
            }
            var workThreads = new List<Thread>();

            if (showLoadingAnim)
            {
                Thread loadingThread = new Thread(LoadingAnimation.RunLoading)
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

            // Solution file exists
            checkList.Add(new FeatureEvidence()
            {
                FeatureTitle = "Solution File Exists",
                FeatureImplemented = true,
                FeatureRating = 1
            });

            // Git repo used
            checkList.Add(new GitCheck(dir.ModifiedTestDirPath).GitEvidence);

            // Project build
            checkList.Add(new ProjectBuilder(dir.ModifiedTestDirPath, new FeatureRunner()).ProjectBuilderEvidence);

            pr.Execute();
            // Project run test
            checkList.Add(pr.ProjectRunEvidence);

            // Unit test test
            checkList.Add(new TestCountCheck(dir.ModifiedTestDirPath, new FeatureRunner()).UnitTestEvidence);

            //Front End Check
            checkList.Add(new FrontEndCheck(pr.GetPort(), UIKeywords.UNIT_KEYWORDS).FrontEndEvidence);

            UnitConverterCheck ucc = new UnitConverterCheck(pr.GetPort());

            // Unit converter test
            checkList.Add(ucc.UnitConverterCheckEvidence);

            checkList.Add(ucc.BadInputCheckEvidence);

            LoadingAnimation.LoadingFinished = true;
            workThreads.ForEach(a => a.Join());
            pr.KillProject();

            return checkList;
        }
    }
}
