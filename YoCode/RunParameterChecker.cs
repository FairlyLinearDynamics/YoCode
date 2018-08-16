using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace YoCode
{
    internal class RunParameterChecker
    {
        private readonly Output compositeOutput;
        private readonly IInputResult result;
        private readonly IAppSettingsBuilder appsettingsBuilder;

        public List<string> Errs= new List<string>();
        public string CMDToolsPath { get; set; }
        public string DotCoverDir { get; set; }

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
            if (!CMDPathExists || !dotCoverPathExists)
            {
                return false;
            }

            return CheckIfToolExecutablesExist();
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

        public bool FilesReadCorrectly(IPathManager dir)
        {
            if (dir.ModifiedPaths == null || dir.OriginalPaths == null)
            {
                compositeOutput.ShowDirEmptyMsg();
                return false;
            }

            if (!dir.ModifiedPaths.Any())
            {
                compositeOutput.ShowLaziness();
                return false;
            }
            return true;
        }
    }
}
