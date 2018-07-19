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
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            CMDToolsPath = Configuration["duplicationCheckSetup:CMDtoolsDir"];

            var consoleOutput = new Output(new PrintToConsole());
            consoleOutput.PrintIntroduction();

            var commandLinehandler = new CommandLineParser(args);
            var result = commandLinehandler.Parse();

            if (result.helpAsked)
            {
                consoleOutput.PrintHelp();
                return;
            }

            if (result.HasErrors)
            {
                consoleOutput.PrintError(result.errors);
                return;
            }

            var modifiedTestDirPath = result.modifiedFilePath;
            var originalTestDirPath = result.originalFilePath;

            var dir = new PathManager(originalTestDirPath, modifiedTestDirPath);

            if (dir.ModifiedPaths == null || dir.OriginalPaths == null)
            {
                consoleOutput.NothingInDirectory();
                return;
            }

            if (!dir.ModifiedPaths.Any())
            {
                consoleOutput.LazinessEvidence();
                return;
            }

            consoleOutput.PrintFinalResults(PerformChecks(dir));
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

                var featureRunner = new FeatureRunner();

                // Git repo used
                checkList.Add(new GitCheck(dir.modifiedTestDirPath, featureRunner).GitEvidence);

                // Project build
                checkList.Add(new ProjectBuilder(dir.modifiedTestDirPath).ProjectBuilderEvidence);

                // Duplication check
                checkList.Add(new DuplicationCheck(dir,CMDToolsPath, featureRunner).DuplicationEvidence);

                // Project run test
                checkList.Add(new ProjectRunner(dir.modifiedTestDirPath).ProjectRunEvidence);

                // Unit test test
                checkList.Add(new TestCountCheck(dir.modifiedTestDirPath,featureRunner).UnitTestEvidence);

                
            }

            return checkList;
        }
    }
}
