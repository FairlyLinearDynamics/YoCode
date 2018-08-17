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

        private static void Main(string[] args)
        {
            var outputs = new List<IPrint> { new WebWriter(), new ConsoleWriter() };

            var compositeOutput = new Output(new CompositeWriter(outputs));

            var commandLinehandler = new CommandLineParser(args);
            var result = commandLinehandler.Parse();

            var parameters = new RunParameterChecker(compositeOutput, result, new AppSettingsBuilder());
            if(!parameters.ParametersAreValid())
            {
                if(!result.HelpAsked)
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
            var originalTestDirPath = result.originalFilePath;

            var dir = new PathManager(originalTestDirPath, modifiedTestDirPath);

            if (!parameters.FilesReadCorrectly(dir))
            {
                return;
            }

            pr = new ProjectRunner(dir.ModifiedTestDirPath, new FeatureRunner());

            ConsoleCloseHandler.StartHandler(pr);

            showLoadingAnim = !result.NoLoadingScreen;
            OpenHTMLOnFinish = !result.Silent;
            var implementedFeatureList = PerformChecks(dir, parameters);
            compositeOutput.PrintFinalResults(implementedFeatureList.OrderBy(a=>a.FeatureTitle));

        }

        public static bool OpenHTMLOnFinish { get; set; }

        private static List<FeatureEvidence> PerformChecks(PathManager dir, RunParameterChecker p)
        {
            var checkList = new List<FeatureEvidence>();

            var fileCheck = new FileChangeChecker(dir);

            var workThreads = new List<Thread>();

            if (fileCheck.FileChangeEvidence.FeatureImplemented)
            {
                if (showLoadingAnim)
                {
                    Thread loadingThread = new Thread(LoadingAnimation.RunLoading)
                    {
                        IsBackground = true
                    };
                    workThreads.Add(loadingThread);
                    loadingThread.Start();
                }

                //Code Coverage
                var codeCoverageThread = new Thread(() =>
                {
                    checkList.Add(new CodeCoverageCheck(p.DotCoverDir, dir.ModifiedTestDirPath, new FeatureRunner()).CodeCoverageEvidence);
                });
                workThreads.Add(codeCoverageThread);
                codeCoverageThread.Start();

                // Duplication check
                var dupFinderThread = new Thread(() =>
                {
                    checkList.Add(new DuplicationCheck(dir, new DupFinder(p.CMDToolsPath)).DuplicationEvidence);
                });
                workThreads.Add(dupFinderThread);
                dupFinderThread.Start();

                // Files changed check
                checkList.Add(fileCheck.FileChangeEvidence);

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
                workThreads.ForEach(a=> a.Join());
                pr.KillProject();

                Console.WriteLine("The final score is " + new Results(checkList).FinalScore + "\n");
            }
            return checkList;
        }
    }
}
