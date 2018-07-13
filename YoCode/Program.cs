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
            var modifiedTestDirPath = args[0];
            var originalTestDirPath = args[1];

            var consoleOutput = new PrintToConsole();

            // TODO: Create new class to handle input and check correctness of input
            if (Directory.Exists(modifiedTestDirPath) && Directory.Exists(originalTestDirPath))
            {

                var modifiedTest = FileImport.GetAllFilesInDirectory(modifiedTestDirPath);
                var originalTest = FileImport.GetAllFilesInDirectory(originalTestDirPath);

                var dir = new PathManager(originalTestDirPath, modifiedTestDirPath);

                var checkList = PerformChecks(dir);

                if(checkList.Count() != 0)
                {
                    consoleOutput.PrintFinalResults(checkList);
                }
                else
                {
                    consoleOutput.LazinessEvidence();
                }        
            }
            else
            {
                consoleOutput.PrintWrongDirectory();
                //if (Directory.Exists(modifiedTestDirPath))
                //{
                //    // TODO: Add evidence for wrong Directory 
                //    testResults.WrongDirectory = true;
                //}
            }
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

                // Code score test
                checkList.Add(new DuplicationCheck(dir).DuplicationEvidence);
            }

            return checkList;
        }
    }
}
