using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace YoCode
{
    internal static class Program
    {
        private static ProjectRunner pr;
        private static bool htmlReportLaunched;

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
                return;
            }

            var modifiedTestDirPath = result.modifiedFilePath;
            var originalTestDirPath = result.originalFilePath;

            var dir = new PathManager(originalTestDirPath, modifiedTestDirPath);

            if (!parameters.FilesReadCorrectly(dir))
            {
                return;
            }

            compositeOutput.PrintIntroduction();

            pr = new ProjectRunner(dir.ModifiedTestDirPath, new FeatureRunner());

            ConsoleCloseHandler.StartHandler(pr);

            var implementedFeatureList = PerformChecks(dir, parameters);
            compositeOutput.PrintFinalResults(implementedFeatureList.OrderBy(a=>a.FeatureTitle));
            htmlReportLaunched = HtmlReportLauncher.LaunchReport("YoCodeReport.html");
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
                    checkList.Add(new CodeCoverageCheck(p.DotCoverDir, dir.ModifiedTestDirPath, new FeatureRunner()).CodeCoverageEvidence);
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

                var modifiedHtmlFiles = dir.GetFilesInDirectory(dir.ModifiedTestDirPath, FileTypes.html).ToList();

                checkList.Add(new UICheck(modifiedHtmlFiles, UIKeywords.UNIT_KEYWORDS).UIEvidence);

                // Solution file exists
                checkList.Add(new FeatureEvidence()
                {
                    FeatureTitle = "Solution File Exists",
                    FeatureImplemented = true,
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

                pr.KillProject();

                codeCoverageThread.Join();
                dupFinderThread.Join();
            }
            return checkList;
        }
    }
}
