using System;
using System.Collections.Generic;
using System.IO;

namespace YoCode
{
    public class DuplicationCheck
    {
        private readonly string CMDtoolsDir;
        private readonly string CMDtoolFileName = "dupfinder.exe";

        private readonly string fileNameChecked = "UnitConverterWebApp.sln";
        private readonly string outputFile = "report.xml";
        private readonly string outputArg = " -o=\"";

        private readonly string processName;
        private readonly string workingDir;
        private readonly string modiArguments;
        private readonly string origArguments;
        private readonly IFeatureRunner featureRunner;

        private int ModiCodeBaseCost { get; set; }
        private int ModiDuplicateCost { get; set; }

        private int OrigCodeBaseCost { get; set; }
        private int OrigDuplicateCost { get; set; }

        public DuplicationCheck(PathManager dir, string CMDtoolsDirConfig, IFeatureRunner featureRunner)
        {
            CMDtoolsDir = CMDtoolsDirConfig;
            DuplicationEvidence.FeatureTitle = "Code quality improvement";
            processName = Path.Combine(CMDtoolsDir, CMDtoolFileName);
            workingDir = CMDtoolsDir;
            this.featureRunner = featureRunner;

            modiArguments = Path.Combine(dir.modifiedTestDirPath, fileNameChecked) + outputArg + outputFile;
            origArguments = Path.Combine(dir.originalTestDirPath, fileNameChecked) + outputArg + outputFile;

            try
            {
                ExecuteTheCheck();
            }
            catch(Exception e)
            {
                DuplicationEvidence.FeatureImplemented = false;
                DuplicationEvidence.GiveEvidence(YoCode.messages.DupFinderHelp);
            }

        }

        private void ExecuteTheCheck() {
            (var origEvidence, var origCodeBaseCost, var origDuplicateCost) = RunAndGatherEvidence(origArguments,"Original");
            (var modEvidence, var modCodeBaseCost, var modDuplicateCost) = RunAndGatherEvidence(modiArguments,"Modified");

            if (origEvidence.FeatureFailed || modEvidence.FeatureFailed)
            {
                DuplicationEvidence.SetFailed($"Failed: Original={origEvidence.FeatureFailed}, Modified={modEvidence.FeatureFailed}");
                return;
            }

            OrigCodeBaseCost = origCodeBaseCost;
            OrigDuplicateCost = origDuplicateCost;

            ModiCodeBaseCost = modCodeBaseCost;
            ModiDuplicateCost = modDuplicateCost;

            DuplicationEvidence.FeatureImplemented = HasTheCodeImproved();
            DuplicationEvidence.GiveEvidence(origEvidence, modEvidence);
        }

        private (FeatureEvidence, int, int) RunAndGatherEvidence(string arguments, string whichDir)
        {
            var evidence = RunOneCheck(arguments);
            var codebaseCostText = evidence.Output.GetLineWithAllKeywords(GetCodeBaseCostKeyword());
            var duplicateCostText = evidence.Output.GetLineWithAllKeywords(GetTotalDuplicatesCostKeywords());
            var codebaseCost = codebaseCostText.GetNumbersInALine()[0];
            var duplicateCost = duplicateCostText.GetNumbersInALine()[0];

            evidence.GiveEvidence(whichDir + $"\nCodebase cost: {codebaseCost}\nDuplicate cost: {duplicateCost}");

            return (evidence, codebaseCost, duplicateCost);
        }

        private FeatureEvidence RunOneCheck(string args)
        {
            var proc = new ProcessDetails(processName, workingDir, args);
            var evidence = featureRunner.Execute(proc, "Check duplication");

            evidence.Output = GetResults(Path.Combine(workingDir,outputFile));
            return evidence;
        }

        public string GetResults(string path)
        {
            return File.ReadAllText(path);
        }

        public bool HasTheCodeImproved()
        {
            return OrigDuplicateCost > ModiDuplicateCost;
        }

        public List<String> GetCodeBaseCostKeyword()
        {
            return new List<string> { "<CodebaseCost>" };
        }

        public List<String> GetTotalDuplicatesCostKeywords()
        {
            return new List<string> { "<TotalDuplicatesCost>" };
        }

        public FeatureEvidence DuplicationEvidence { get; } = new FeatureEvidence();
    }
}
