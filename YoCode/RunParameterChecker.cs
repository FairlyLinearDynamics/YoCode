using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace YoCode
{
    internal class RunParameterChecker
    {
        private static IConfiguration Configuration;
        private readonly Output compositeOutput;

        public List<string> errs = new List<string>();
        public string CMDToolsPath { get; }
        public string DotCoverDir { get; }
        public bool NeedToReturn { get; set; }

        public RunParameterChecker(Output compositeOutput, InputResult result)
        {
            this.compositeOutput = compositeOutput;

            if (result.helpAsked)
            {
                compositeOutput.PrintIntroduction();
                compositeOutput.ShowHelp();
                NeedToReturn = true;
                return;
            }
            if (result.HasErrors || errs.Any())
            {
                errs.AddRange(result.errors);
                NeedToReturn = true;
                return;
            }

            try
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
                Configuration = builder.Build();
            }
            catch (FileNotFoundException)
            {
                errs.Add("Did not find appsettings file");
                NeedToReturn = true;
                return;
            }
            catch (FormatException)
            {
                errs.Add("Error reading JSON file");
                NeedToReturn = true;
                return;
            }

            CMDToolsPath = Configuration["duplicationCheckSetup:CMDtoolsDir"];
            DotCoverDir = Configuration["codeCoverageCheckSetup:dotCoverDir"];

            CheckToolDirectory(errs, CMDToolsPath, "CMDtoolsDir");
            CheckToolDirectory(errs, DotCoverDir, "dotCoverDir");

            if(NeedToReturn)
            {
                return;
            }

            CheckIfToolExecutablesExist();
        }

        private void CheckIfToolExecutablesExist()
        {
            if (!File.Exists(Path.Combine(CMDToolsPath, "dupfinder.exe")))
            {
                errs.Add("dupfinder.exe not found in specified directory");
                NeedToReturn = true;
            }
            if (!File.Exists(Path.Combine(DotCoverDir, "dotCover.exe")))
            {
                errs.Add("dotCover.exe not found in specified directory");
                NeedToReturn = true;
            }
        }

        private void CheckToolDirectory(List<string> errs, string path, string checkName)
        {
            if (String.IsNullOrEmpty(path))
            {
                errs.Add($"{checkName} cannot be empty");
                NeedToReturn = true;
            }
            else if (!Directory.Exists(path))
            {
                errs.Add($"invalid directory provided for {checkName}");
                NeedToReturn = true;
            }
        }

        public bool FilesReadCorrectly(PathManager dir)
        {
            if (dir.ModifiedPaths == null || dir.OriginalPaths == null)
            {
                compositeOutput.ShowDirEmptyMsg();
                return true;
            }

            if (!dir.ModifiedPaths.Any())
            {
                compositeOutput.ShowLaziness();
                return true;
            }
            return false;
        }
    }
}
