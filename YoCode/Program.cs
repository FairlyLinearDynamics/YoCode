using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace YoCode
{
    public static class Program
    {
        private static ProjectRunner pr;

        static void Main(string[] args)
        {
            var outputs = new List<IPrint> { new WebWriter(), new ConsoleWriter() };

            var compositeOutput = new Output(new CompositeWriter(outputs));

            var commandLinehandler = new CommandLineParser(args);
            var result = commandLinehandler.Parse();

            var parameters = new RunParameterChecker(compositeOutput, result);
            if(parameters.NeedToReturn)
            {
                if(!result.helpAsked)
                {
                    compositeOutput.ShowInputErrors(parameters.errs);
                }
                return;
            }

            var modifiedTestDirPath = result.modifiedFilePath;
            var originalTestDirPath = result.originalFilePath;

            var dir = new PathManager(originalTestDirPath, modifiedTestDirPath);

            var failedToReadFiles = parameters.FilesReadCorrectly(dir);
            if (failedToReadFiles)
            {
                return;
            }

            compositeOutput.PrintIntroduction();

            pr = new ProjectRunner(dir.modifiedTestDirPath, new FeatureRunner());

            ConsoleCloseHandler.StartHandler(pr);

            var implementedFeatureList = PerformChecks(dir, parameters);
            compositeOutput.PrintFinalResults(implementedFeatureList.OrderBy(a => a.FeatureTitle));
        }

        private static List<FeatureEvidence> PerformChecks(PathManager dir, RunParameterChecker p)
        {
            var checkList = new List<FeatureEvidence>();

            var fileCheck = new FileChangeChecker(dir);

            if (fileCheck.FileChangeEvidence.FeatureImplemented)
            {
                //Code Coverage
                var codeCoverageThread = new Thread(() =>
                {
                    checkList.Add(new CodeCoverageCheck(p.DotCoverDir, dir.modifiedTestDirPath, new FeatureRunner()).CodeCoverageEvidence);
                });
                codeCoverageThread.Start();

                // Duplication check
                var dupFinderThread = new Thread(() =>
                {
                    checkList.Add(new DuplicationCheck(dir, new DupFinder(p.CMDToolsPath)).DuplicationEvidence);
                });
                dupFinderThread.Start();

                // Files changed check
                checkList.Add(fileCheck.FileChangeEvidence);

                // UI test

                var modifiedHtmlFiles = dir.GetFilesInDirectory(dir.modifiedTestDirPath, FileTypes.html).ToList();

                checkList.Add(new UICheck(modifiedHtmlFiles, UIKeywords.UNIT_KEYWORDS).UIEvidence);

                // Solution file exists
                checkList.Add(new FeatureEvidence()
                {
                    FeatureTitle = "Solution File Exists",
                    FeatureImplemented = true,
                });

                // Git repo used
                checkList.Add(new GitCheck(dir.modifiedTestDirPath).GitEvidence);

                // Project build
                checkList.Add(new ProjectBuilder(dir.modifiedTestDirPath, new FeatureRunner()).ProjectBuilderEvidence);

                pr.Execute();
                // Project run test
                checkList.Add(pr.ProjectRunEvidence);

                // Unit test test
                checkList.Add(new TestCountCheck(dir.modifiedTestDirPath, new FeatureRunner()).UnitTestEvidence);

                //Front End Check
                checkList.Add(new FrontEndCheck(pr.GetPort(), UIKeywords.UNIT_KEYWORDS).FrontEndEvidence);

                UnitConverterCheck ucc = new UnitConverterCheck(pr.GetPort());

                // Unit converter test
                checkList.Add(ucc.UnitConverterCheckEvidence);

                checkList.Add(ucc.BadInputCheckEvidence);

                pr.KillProject();

                codeCoverageThread.Join();
                dupFinderThread.Join();
            }
            return checkList;
        }
    }
}
