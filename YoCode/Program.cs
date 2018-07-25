using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using LibGit2Sharp;
using System.Resources;

namespace YoCode
{
    public static class Program
    {
        public static IConfiguration Configuration;

        private static string CMDToolsPath;

        static void Main(string[] args)
        {

            var consoleOutput = new Output(new ConsoleWriter());
            var webReport = new Output(new WebWriter());

            try
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
                Configuration = builder.Build();
                CMDToolsPath = Configuration["duplicationCheckSetup:CMDtoolsDir"];
            }
            catch (FileNotFoundException)
            {
                consoleOutput.ShowHelp();
                webReport.ShowHelp();
                return;
            }

            //var consoleOutput = new Output(new WebWriter());
            consoleOutput.PrintIntroduction();
            webReport.PrintIntroduction();

            var commandLinehandler = new CommandLineParser(args);
            var result = commandLinehandler.Parse();

            if (result.helpAsked)
            {
                consoleOutput.ShowHelp();
                webReport.ShowHelp();
                return;
            }

            if (result.HasErrors)
            {
                consoleOutput.ShowInputErrors(result.errors);
                webReport.ShowInputErrors(result.errors);
                return;
            }

            var modifiedTestDirPath = result.modifiedFilePath;
            var originalTestDirPath = result.originalFilePath;

            var dir = new PathManager(originalTestDirPath, modifiedTestDirPath);

            if (dir.ModifiedPaths == null || dir.OriginalPaths == null)
            {
                consoleOutput.ShowDirEmptyMsg();
                webReport.ShowDirEmptyMsg();
                return;
            }

            if (!dir.ModifiedPaths.Any())
            {
                consoleOutput.ShowLaziness();
                webReport.ShowLaziness();
                return;
            }

            var implementedFeatureList = PerformChecks(dir);
            consoleOutput.PrintFinalResults(implementedFeatureList);
            webReport.PrintFinalResults(implementedFeatureList);
        }

        private static List<FeatureEvidence> PerformChecks(PathManager dir)
        {
            var checkList = new List<FeatureEvidence>();

            var fileCheck = new FileChangeChecker(dir);

            if (fileCheck.FileChangeEvidence.FeatureImplemented)
            {
                checkList.Add(fileCheck.FileChangeEvidence);

                // UI test
                var keyWords = new[] { "miles", "kilometers", "km" };
                var modifiedHtmlFiles = dir.GetFilesInDirectory(dir.modifiedTestDirPath, FileTypes.html).ToList();

                checkList.Add(new UICheck(modifiedHtmlFiles, keyWords).UIEvidence);

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

                // Duplication check
                checkList.Add(new DuplicationCheck(dir, new DupFinder(CMDToolsPath)).DuplicationEvidence);


                var pr = new ProjectRunner(dir.modifiedTestDirPath, new FeatureRunner());
                // Project run test
                checkList.Add(pr.ProjectRunEvidence);

                // Unit test test
                checkList.Add(new TestCountCheck(dir.modifiedTestDirPath, new FeatureRunner()).UnitTestEvidence);

                checkList.Add(new UnitConverterCheck(pr.GetPort()).UnitConverterCheckEvidence);

                pr.KillProject();         
            }
            return checkList;
        }
    }
}
