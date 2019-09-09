using System;
using System.Collections.Generic;
using System.IO;

namespace YoCode
{
    internal class RunParameterChecker : IRunParameterChecker
    {
        private readonly Output compositeOutput;
        private readonly IInputResult result;
        private readonly IAppSettingsBuilder appsettingsBuilder;

        public List<string> Errs = new List<string>();
        public ToolPath DupFinderPath { get; set; }
        public ToolPath DotCoverPath { get; set; }

        public string TestCodeBaseCost { get; set; }
        public string TestDuplicationCost { get; set; }

        public string AppCodeBaseCost { get; set; }
        public string AppDuplicationCost { get; set; }

        public RunParameterChecker(Output compositeOutput, IInputResult result, IAppSettingsBuilder appsettingsBuilder)
        {
            this.compositeOutput = compositeOutput;
            this.result = result;
            this.appsettingsBuilder = appsettingsBuilder;
        }

        public bool ParametersAreValid()
        {
            if (result.HelpAsked)
            {
                return false;
            }
            if (result.HasErrors)
            {
                Errs.AddRange(result.Errors);
                return false;
            }

            try
            {
                ReadAppsettings();
            }
            catch (FileNotFoundException)
            {
                return SetError("Did not find appsettings file");
            }
            catch (FormatException)
            {
                return SetError("Error reading JSON file");
            }

            bool anyFilesMissing = CheckAppsettingPathsAreValid();

            if (anyFilesMissing)
            {
                return false;
            }

            return CheckIfToolExecutablesExist();
        }

        private bool CheckAppsettingPathsAreValid()
        {
            var CMDPathExists = CheckToolDirectory(DupFinderPath, "CMDtoolsDir");
            var dotCoverPathExists = CheckToolDirectory(DotCoverPath, "dotCoverDir");
            var costValuesProvided = CheckIfCostsProvided(TestCodeBaseCost, TestDuplicationCost, "Test cost values") && CheckIfCostsProvided(AppCodeBaseCost, AppDuplicationCost, "App cost values");

            var juniorFileExists = FileExists("JuniorWeightings.json");
            var originalFileExists = FileExists("OriginalWeightings.json");

            return !CMDPathExists || !dotCoverPathExists || !juniorFileExists || !originalFileExists || !costValuesProvided;
        }

        private void ReadAppsettings()
        {
            appsettingsBuilder.ReadJSONFile();

            DupFinderPath = appsettingsBuilder.GetDupFinderPath();
            DotCoverPath = appsettingsBuilder.GetDotCoverPath();

            (AppCodeBaseCost, AppDuplicationCost) = appsettingsBuilder.GetWebAppCosts();
            (TestCodeBaseCost, TestDuplicationCost) = appsettingsBuilder.GetTestsCosts();
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

        private bool CheckIfToolExecutablesExist()
        {
            if (!File.Exists(DupFinderPath.FullPath))
            {
                return SetError("dupfinder.exe not found in specified directory");
            }
            if (!File.Exists(DotCoverPath.FullPath))
            {
                return SetError("dotCover.exe not found in specified directory");
            }
            return true;
        }

        private bool CheckToolDirectory(ToolPath toolPath, string checkName)
        {
            if (String.IsNullOrEmpty(toolPath.Dir))
            {
                return SetError($"{checkName} cannot be empty");
            }

            return toolPath.Exists() || SetError($"invalid directory provided for {checkName}");
        }

        private bool CheckIfCostsProvided(string cost1, string cost2, string checkName)
        {
            if (String.IsNullOrEmpty(cost1) || String.IsNullOrEmpty(cost2))
            {
                return SetError($"{checkName} cannot be empty");
            }
            return true;
        }
    }
}