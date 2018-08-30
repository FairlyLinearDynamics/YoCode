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

        public List<string> Errs= new List<string>();
        public string CMDToolsPath { get; set; }
        public string DotCoverDir { get; set; }

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
                appsettingsBuilder.ReadJSONFile();

                CMDToolsPath = appsettingsBuilder.GetCMDToolsPath();
                DotCoverDir = appsettingsBuilder.GetDotCoverDir();

                (AppCodeBaseCost, AppDuplicationCost) = appsettingsBuilder.GetWebAppCosts();
                (TestCodeBaseCost, TestDuplicationCost) = appsettingsBuilder.GetTestsCosts();

            }
            catch (FileNotFoundException)
            {
                return SetError("Did not find appsettings file");
            }
            catch (FormatException)
            {
                return SetError("Error reading JSON file");
            }

            var CMDPathExists = CheckToolDirectory(CMDToolsPath, "CMDtoolsDir");
            var dotCoverPathExists = CheckToolDirectory(DotCoverDir, "dotCoverDir");
            var costValuesProvided = CheckIfCostsProvided(TestCodeBaseCost, TestDuplicationCost, "Test cost values") && CheckIfCostsProvided(AppCodeBaseCost, AppDuplicationCost, "App cost values");

            var juniorFileExists = FileExists(TestType.Junior, "JuniorWeightings.json");
            var originalFileExists = FileExists(TestType.Original, "OriginalWeightings.json");

            bool anyFilesMissing = !CMDPathExists || !dotCoverPathExists || !juniorFileExists || !originalFileExists || !costValuesProvided;

            if (anyFilesMissing)
            {
                return false;
            }

            return CheckIfToolExecutablesExist();
        }

        private bool FileExists(TestType type, string fileName)
        {
            if(!File.Exists(appsettingsBuilder.GetWeightingsPath()))
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
            if (!File.Exists(Path.Combine(CMDToolsPath, "dupfinder.exe")))
            {
                return SetError("dupfinder.exe not found in specified directory");
            }
            if (!File.Exists(Path.Combine(DotCoverDir, "dotCover.exe")))
            {
                return SetError("dotCover.exe not found in specified directory");
            }
            return true;
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
                return SetError($"{checkName} cannot be empty");
            }
            return true;
        }
    }
}
