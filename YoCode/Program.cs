using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace YoCode
{
    public static class Program
    {

        static void Main(string[] args)
        {
            var consoleOutput = new PrintToConsole();
            var commandLinehandler = new CommandLineParser(args);
            var result = commandLinehandler.Parse();

            if (result.HasErrors)
            {
                consoleOutput.PrintError(result.errors);
                return;
            }

            var modifiedTestDirPath = result.modifiedFilePath;
            var originalTestDirPath = result.originalFilePath;

            var fileReader = new FileImport();

            var modifiedTest = FileImport.GetAllFilesInDirectory(modifiedTestDirPath);
            var originalTest = FileImport.GetAllFilesInDirectory(originalTestDirPath);

            if (modifiedTest == null || originalTest == null)
            {
                consoleOutput.NothingInDirectory();
                return;
            }

            var dir = new PathManager(originalTest, modifiedTest);

            var checkList = PerformChecks(modifiedTestDirPath, dir);

            if (checkList.Count() != 0)
            {
                consoleOutput.PrintFinalResults(checkList);
            }
            else
            {
                consoleOutput.LazinessEvidence();
            }
        }

        private static List<FeatureEvidence> PerformChecks(string modifiedTestDirPath, PathManager dir)
        {
            var checkList = new List<FeatureEvidence>();

            var fileCheck = new FileChangeChecker(dir);

            if (fileCheck.FileChangeEvidence.FeatureImplemented)
            {
                checkList.Add(fileCheck.FileChangeEvidence);

                // UI test
                var keyWords = new[] { "miles", "kilometers", "km" };
                var modifiedHtmlFiles = dir.GetFilesInDirectory(modifiedTestDirPath, FileTypes.html).ToList();

                checkList.Add(new UICheck(modifiedHtmlFiles, keyWords).UIEvidence);

                // Solution file exists
                checkList.Add(new FeatureEvidence()
                {
                    FeatureTitle = "Solution File Exists",
                    FeatureImplemented = true,
                });

                // Git repo used
                checkList.Add(new GitCheck(modifiedTestDirPath).GitEvidence);
            }

            return checkList;
        }
    }
}
