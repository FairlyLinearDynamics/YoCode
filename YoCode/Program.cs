using System;
using System.Linq;
using System.IO;

namespace YoCode
{
    public static class Program
    {

        static void Main(string[] args)
        {
            var modifiedTestDirPath = args[0];
            var originalTestDirPath = args[1];

            var consoleOutput = new PrintToConsole();

            TestResults testResults;

            // TODO: Create new class to handle input and check correctness of input
            if (Directory.Exists(modifiedTestDirPath) && Directory.Exists(originalTestDirPath))
            {
                var fileReader = new FileImport();

                var modifiedTest = FileImport.GetAllFilesInDirectory(modifiedTestDirPath);
                var originalTest = FileImport.GetAllFilesInDirectory(originalTestDirPath);

                var dir = new PathManager(originalTest, modifiedTest);

                testResults = PerformChecks(modifiedTestDirPath, dir);
            }
            else
            {
                testResults = new TestResults();
                if (Directory.Exists(modifiedTestDirPath))
                {
                    //testResults.WrongDirectory.ProvideEvidence($"{modifiedTestDirPath} does not exist.");
                }
            }

            // Printing calls
            consoleOutput.PrintIntroduction();
            consoleOutput.PrintFinalResults(testResults);
        }

        private static TestResults PerformChecks(string modifiedTestDirPath, PathManager dir)
        {
            var testResults = new TestResults();
            if (FileChangeChecker.ProjectIsModified(dir))
            {
                // TODO: Add evidence to file changed/not changed
                testResults.AnyFileChanged = true;

                // UI test
                var keyWords = new[] { "miles", "kilometers", "km" };
                var modifiedHtmlFiles = dir.GetFilesInDirectory(modifiedTestDirPath, FileTypes.html).ToList();

                var uiChecker = new UICheck(modifiedHtmlFiles, keyWords);

                testResults.UICheckExists = uiChecker.UIExists;
                testResults.UICheckExistsEvidence = uiChecker.EvidenceList;

                // Solution file exists
                //testResults.SolutionFileExist = dir.GetFilesInDirectory(modifiedTestDirPath, FileTypes.sln).Count() != 0;

                // Git repo used
                //var gitChecker = new GitCheck(modifiedTestDirPath);
                //testResults.GitUsed = gitChecker.GitUsed;
            }

            return testResults;
        }
    }
}
