using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace YoCode
{
    public static partial class Program
    {
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(SetConsoleCtrlEventHandler handler, bool add);

        private delegate bool SetConsoleCtrlEventHandler(CtrlType sig);

        public static IConfiguration Configuration;

        private static string CMDToolsPath;
        private static string dotCoverDir;
        private static ProjectRunner pr;

        static void Main(string[] args)
        {
            var outputs = new List<IPrint> { new WebWriter(), new ConsoleWriter() };

            var compositeOutput = new Output(new CompositeWriter(outputs));

            try
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
                Configuration = builder.Build();
                CMDToolsPath = Configuration["duplicationCheckSetup:CMDtoolsDir"];
                dotCoverDir = Configuration["codeCoverageCheckSetup:dotCoverDir"];
            }
            catch (FileNotFoundException)
            {
                compositeOutput.ShowHelp();
                return;
            }

            compositeOutput.PrintIntroduction();

            var commandLinehandler = new CommandLineParser(args);
            var result = commandLinehandler.Parse();

            if (result.helpAsked)
            {
                compositeOutput.ShowHelp();
                return;
            }

            if (result.HasErrors)
            {
                compositeOutput.ShowInputErrors(result.errors);
                return;
            }

            var modifiedTestDirPath = result.modifiedFilePath;
            var originalTestDirPath = result.originalFilePath;

            var dir = new PathManager(originalTestDirPath, modifiedTestDirPath);

            if (dir.ModifiedPaths == null || dir.OriginalPaths == null)
            {
                compositeOutput.ShowDirEmptyMsg();
                return;
            }

            if (!dir.ModifiedPaths.Any())
            {
                compositeOutput.ShowLaziness();
                return;
            }

            pr = new ProjectRunner(dir.modifiedTestDirPath, new FeatureRunner());

            SetConsoleCtrlHandler(Handler, true);

            var implementedFeatureList = PerformChecks(dir);
            compositeOutput.PrintFinalResults(implementedFeatureList.OrderBy(a => a.FeatureTitle));
        }

        private static List<FeatureEvidence> PerformChecks(PathManager dir)
        {
            var checkList = new List<FeatureEvidence>();

            var fileCheck = new FileChangeChecker(dir);

            if (fileCheck.FileChangeEvidence.FeatureImplemented)
            {
                //Code Coverage
                var codeCoverageThread = new Thread(() =>
                {
                    checkList.Add(new CodeCoverageCheck(dotCoverDir, dir.modifiedTestDirPath, new FeatureRunner()).CodeCoverageEvidence);
                });
                codeCoverageThread.Start();

                // Duplication check
                var dupFinderThread = new Thread(() =>
                {
                    checkList.Add(new DuplicationCheck(dir, new DupFinder(CMDToolsPath)).DuplicationEvidence);
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

                checkList.Add(new FrontEndCheck(pr.GetPort(), UIKeywords.UNIT_KEYWORDS).FrontEndEvidence);

                // Project run test
                checkList.Add(pr.ProjectRunEvidence);

                // Unit test test
                checkList.Add(new TestCountCheck(dir.modifiedTestDirPath, new FeatureRunner()).UnitTestEvidence);

                UnitConverterCheck ucc = new UnitConverterCheck(pr.GetPort());

                // Unit converter test
                checkList.Add(ucc.UnitConverterCheckEvidence);

                checkList.Add(ucc.BadInputCheckEvidence);

                codeCoverageThread.Join();
                dupFinderThread.Join();
                pr.KillProject();
            }
            return checkList;
        }

        private static bool Handler(CtrlType signal)
        {
            switch (signal)
            {
                case CtrlType.CTRL_BREAK_EVENT:
                case CtrlType.CTRL_C_EVENT:
                case CtrlType.CTRL_LOGOFF_EVENT:
                case CtrlType.CTRL_SHUTDOWN_EVENT:
                case CtrlType.CTRL_CLOSE_EVENT:
                    Console.WriteLine("Closing");

                    try
                    {
                        pr.KillProject();

                        if (Array.Find(Process.GetProcesses(), x => x.ProcessName == "geckodriver") != null)
                        {
                            var process = Array.Find(Process.GetProcesses(), x => x.ProcessName == "geckodriver");
                            ProcessRunner.KillProcessWithChildren(process);
                        }
                    }
                    catch (NullReferenceException) { }

                    Environment.Exit(0);
                    return false;

                default:
                    return false;
            }
        }
    }
}
