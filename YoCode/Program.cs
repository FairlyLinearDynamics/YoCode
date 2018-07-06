﻿using System;
using System.Linq;
using System.IO;

namespace YoCode
{
    public class Program
    {

        static void Main(string[] args)
        {
            var modifiedTestDirPath = args[0];
            var originalTestDirPath = args[1];

            var consoleOutput = new PrintToConsole();
            var testResults = new TestResults();
            var fileReader = new FileImport();

            var modifiedTest = fileReader.GetAllFilesInDirectory(modifiedTestDirPath);
            var originalTest = fileReader.GetAllFilesInDirectory(originalTestDirPath);

            var dir = new Directory(originalTest, modifiedTest);

            if (FileChangeChecker.ProjectIsModified(dir))
            {
                testResults.AnyFileChanged = true;
                // UI test
                var keyWords = new string[] { "miles", "kilometers", "km" };
                var modifiedHtmlFiles = dir.GetFilesInDirectory(modifiedTestDirPath, FileTypes.html).ToList();

                UICheck uiChecker = new UICheck(modifiedHtmlFiles, keyWords);

                testResults.Lines = uiChecker.ListOfMatches;

                // Solution file exists
                testResults.SolutionFileExist = dir.GetFilesInDirectory(modifiedTestDirPath, FileTypes.sln).Count() != 0;

                // Git repo used
                GitCheck gitChecker = new GitCheck();
                gitChecker.ExecuteTheCheck();
                testResults.GitUsed = gitChecker.GitUsed;
                // Will not work on different systems due to issues in GitUsed method
            }
            else
                testResults.AnyFileChanged = false;


            // Printing calls
            consoleOutput.PrintIntroduction();
            consoleOutput.PrintFinalResults(testResults);
        }
    }
}
