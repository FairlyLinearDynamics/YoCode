using System;
using System.Collections.Generic;
using System.IO;

namespace YoCode
{
    internal class RunParameterChecker : IRunParameterChecker
    {
        private readonly Output compositeOutput;
        private readonly IAppSettingsBuilder appsettingsBuilder;

        public List<string> Errs = new List<string>();
        public string CMDToolsPath { get; set; }
        public string DotCoverDir { get; set; }

        public string TestCodeBaseCost { get; set; }
        public string TestDuplicationCost { get; set; }

        public string AppCodeBaseCost { get; set; }
        public string AppDuplicationCost { get; set; }

        public RunParameterChecker(Output compositeOutput, IAppSettingsBuilder appsettingsBuilder)
        {
            this.compositeOutput = compositeOutput;
            this.appsettingsBuilder = appsettingsBuilder;
        }

        public bool ParametersAreValid(string outputPath, IInputResult result)
        {
            CheckInput(outputPath, result);

            ReadAppsettings();

            CheckIfAppsettingsValuesAreValid();

            return CheckIfToolExecutablesExist();
        }

        private void CheckIfAppsettingsValuesAreValid()
        {
            var CMDPathExists = CheckToolDirectory(CMDToolsPath, "CMDtoolsDir");
            var dotCoverPathExists = CheckToolDirectory(DotCoverDir, "dotCoverDir");
            var costValuesProvided = CheckIfCostsProvided(TestCodeBaseCost, TestDuplicationCost, "Test cost values")
                && CheckIfCostsProvided(AppCodeBaseCost, AppDuplicationCost, "App cost values");

            var juniorFileExists = FileExists("JuniorWeightings.json");
            var originalFileExists = FileExists("OriginalWeightings.json");

            bool anyFilesMissing = !CMDPathExists || !dotCoverPathExists || !juniorFileExists || !originalFileExists || !costValuesProvided;

            if (anyFilesMissing)
            {
                compositeOutput.ShowInputErrors(Errs);
                Environment.Exit(1);
            }
        }

        private void CheckInput(string outputPath, IInputResult result)
        {
            if (result.HelpAsked)
            {
                compositeOutput.ShowHelp();
                if (result.CreateHtmlReport && result.OpenHtmlReport)
                {
                    WebWriter.LaunchReport(result, outputPath);
                }
                Environment.Exit(0);
            }
            if (result.HasErrors)
            {
                Errs.AddRange(result.Errors);
                compositeOutput.ShowInputErrors(Errs);
                Environment.Exit(1);
            }
        }

        private void ReadAppsettings()
        {
            try
            {
                appsettingsBuilder.ReadJSONFile();

                CMDToolsPath = appsettingsBuilder.GetCMDToolsPath();
                DotCoverDir = appsettingsBuilder.GetDotCoverDir();

                (AppCodeBaseCost, AppDuplicationCost) = appsettingsBuilder.GetWebAppCosts();
                (TestCodeBaseCost, TestDuplicationCost) = appsettingsBuilder.GetTestsCosts();
            }
            catch (FileNotFoundException)
            {
                ExitWithErrorMessage("Did not find appsettings file");
            }
            catch (FormatException)
            {
                ExitWithErrorMessage("Error reading JSON file");
            }
        }

        private bool CheckIfToolExecutablesExist()
        {
            if (!File.Exists(Path.Combine(CMDToolsPath, "dupfinder.exe")))
            {
                ExitWithErrorMessage("dupfinder.exe not found in specified directory");
            }
            if (!File.Exists(Path.Combine(DotCoverDir, "dotCover.exe")))
            {
                ExitWithErrorMessage("dotCover.exe not found in specified directory");
            }
            return true;
        }

        private void ExitWithErrorMessage(string msg)
        {
            Errs.Add(msg);
            compositeOutput.ShowInputErrors(Errs);
            Environment.Exit(1);
        }

        private bool FileExists(string fileName)
        {
            if (!File.Exists(appsettingsBuilder.GetWeightingsPath()))
            {
                return SetError($"{fileName} not found");
            }
            return true;
        }

        private bool SetError(string errorMessage)
        {
            Errs.Add(errorMessage);
            return false;
        }

        private bool CheckToolDirectory(string path, string checkName)
        {
            if (String.IsNullOrEmpty(path))
            {
                return SetError($"{checkName} cannot be empty");
            }
            else if (!Directory.Exists(path))
            {
                return SetError($"invalid directory provided for {checkName}");
            }
            return true;
        }

        private bool CheckIfCostsProvided(string cost1, string cost2, string checkName)
        {
            if (String.IsNullOrEmpty(cost1) || String.IsNullOrEmpty(cost2))
            {
                return SetError($"{checkName} missing");
            }
            return true;
        }
    }
}
