﻿using System;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace YoCode
{
    public static class Program
    {
        public static IConfiguration Configuration;

        private static string CMDToolsPath;
        private static string dotCoverDir;

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

            var implementedFeatureList = PerformChecks(dir);
            compositeOutput.PrintFinalResults(implementedFeatureList.OrderBy(a=>a.FeatureTitle));
            Process.Start(@"C:\Program Files\Mozilla Firefox\firefox.exe", @"C:\Users\ukekar\source\repos\YoCode\YoCode\bin\Debug\netcoreapp2.1\YoCodeReport.html");
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

                var pr = new ProjectRunner(dir.modifiedTestDirPath, new FeatureRunner());
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
    }
}
