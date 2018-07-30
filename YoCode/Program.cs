using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace YoCode
{
    public static class Program
    {
        public static IConfiguration Configuration;

        private static string CMDToolsPath;

        static void Main(string[] args)
        {
            var outputs = new List<IPrint> { new WebWriter(), new ConsoleWriter() };

            var compositeOutput = new Output(new CompositeWriter(outputs));

            try
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
                Configuration = builder.Build();
                CMDToolsPath = Configuration["duplicationCheckSetup:CMDtoolsDir"];
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

            ProjectBuilder.CleanBuildOutput(originalTestDirPath);
            ProjectBuilder.CleanBuildOutput(modifiedTestDirPath);

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
            compositeOutput.PrintFinalResults(implementedFeatureList);
        }

        private static List<FeatureEvidence> PerformChecks(PathManager dir)
        {
            var checkList = new List<FeatureEvidence>();

            var fileCheck = new FileChangeChecker(dir);
            

            if (fileCheck.FileChangeEvidence.FeatureImplemented)
            {
                // Duplication check
                var dupFinderThread = new Thread(() =>
                {
                    checkList.Add(new DuplicationCheck(dir, new DupFinder(CMDToolsPath)).DuplicationEvidence);
                });
                dupFinderThread.Start();

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

                // Project run test
                var pr = new ProjectRunner(dir.modifiedTestDirPath, new FeatureRunner());
                checkList.Add(pr.ProjectRunEvidence);

                // Unit test test
                checkList.Add(new TestCountCheck(dir.modifiedTestDirPath, new FeatureRunner()).UnitTestEvidence);

                // Unit converter test
                checkList.Add(new UnitConverterCheck(pr.GetPort()).UnitConverterCheckEvidence);

                pr.KillProject();

                dupFinderThread.Join();
            }
            return checkList;
        }
    }
}
