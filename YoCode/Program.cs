﻿using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace YoCode
{
    public static class Program
    {
        public static IConfiguration Configuration;
        
        static void Main(string[] args)
        {
            //This needs to be implemented in order for DuplicationCheck to work
            var builder = new ConfigurationBuilder().SetBasePath(Path.GetFullPath(@"..\..\..\Properties")).AddJsonFile("launchsettings.json");
            Configuration = builder.Build();
            var CMDToolsPath = Configuration["profiles:YoCode:duplicationCheckSetup:CMDtoolsDir"];

            var consoleOutput = new PrintToConsole();
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

            var modifiedTest = FileImport.GetAllFilesInDirectory(modifiedTestDirPath);
            var originalTest = FileImport.GetAllFilesInDirectory(originalTestDirPath);

            if (modifiedTest == null || originalTest == null)
            {
                consoleOutput.NothingInDirectory();
                return;
            }

            var dir = new PathManager(originalTestDirPath, modifiedTestDirPath);

            var checkList = PerformChecks(dir);

            if (checkList.Count() != 0)
            {
                consoleOutput.PrintFinalResults(checkList);
            }
            else
            {
                consoleOutput.LazinessEvidence();
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
