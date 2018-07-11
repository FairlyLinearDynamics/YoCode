﻿using System;
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
                var fileReader = new FileImport();

                var modifiedTest = FileImport.GetAllFilesInDirectory(modifiedTestDirPath);
                var originalTest = FileImport.GetAllFilesInDirectory(originalTestDirPath);

                var dir = new PathManager(originalTest, modifiedTest);

                var checkList = PerformChecks(modifiedTestDirPath, dir);

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

        private static List<FeatureEvidence> PerformChecks(string modifiedTestDirPath, PathManager dir)
        {
            var checkList = new List<FeatureEvidence>();

            var filesChangedEvidence = new FeatureEvidence()
            {
                FeatureTitle = "Any files changed",
            };

            if (FileChangeChecker.ProjectIsModified(dir,filesChangedEvidence))
            {
                checkList.Add(filesChangedEvidence);

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
