using System;
using System.Collections.Generic;
using System.IO;

namespace YoCode
{
    public class DuplicationCheck
    {
        private readonly string fileNameChecked = "UnitConverterWebApp.sln";

        private readonly string modifiedSolutionPath;
        private readonly string originalSolutionPath;
        private readonly IDupFinder dupFinder;

        private int ModiCodeBaseCost { get; set; }
        private int ModiDuplicateCost { get; set; }

        private int OrigCodeBaseCost { get; set; }
        private int OrigDuplicateCost { get; set; }

        public DuplicationCheck(IPathManager dir, IDupFinder dupFinder)
        {

            DuplicationEvidence.FeatureTitle = "Code quality improvement";
            this.dupFinder = dupFinder;

            modifiedSolutionPath = Path.Combine(dir.modifiedTestDirPath, fileNameChecked);
            originalSolutionPath = Path.Combine(dir.originalTestDirPath, fileNameChecked);

            try
            {
                ExecuteTheCheck();
            }
            catch(Exception e)
            {
                DuplicationEvidence.FeatureImplemented = false;
                DuplicationEvidence.GiveEvidence(messages.DupFinderHelp + "\n" +e);
            }

        }

        private void ExecuteTheCheck() {
            (var origEvidence, var origCodeBaseCost, var origDuplicateCost) = RunAndGatherEvidence(originalSolutionPath,"Original");
            (var modEvidence, var modCodeBaseCost, var modDuplicateCost) = RunAndGatherEvidence(modifiedSolutionPath,"Modified");

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

        private (FeatureEvidence, int, int) RunAndGatherEvidence(string solutionPath, string whichDir)
        {
            var evidence = RunOneCheck(solutionPath);
            var codebaseCostText = evidence.Output.GetLineWithAllKeywords(GetCodeBaseCostKeyword());
            var duplicateCostText = evidence.Output.GetLineWithAllKeywords(GetTotalDuplicatesCostKeywords());
            var codebaseCost = codebaseCostText.GetNumbersInALine()[0];
            var duplicateCost = duplicateCostText.GetNumbersInALine()[0];

            evidence.GiveEvidence(whichDir + $"\nCodebase cost: {codebaseCost}\nDuplicate cost: {duplicateCost}");

            return (evidence, codebaseCost, duplicateCost);
        }

        private FeatureEvidence RunOneCheck(string solutionPath)
        {
            return dupFinder.Execute("Dup check",solutionPath);
        }

        private bool HasTheCodeImproved()
        {
            return OrigDuplicateCost > ModiDuplicateCost;
        }

        private List<String> GetCodeBaseCostKeyword()
        {
            return new List<string> { "<CodebaseCost>" };
        }

        private List<String> GetTotalDuplicatesCostKeywords()
        {
            return new List<string> { "<TotalDuplicatesCost>" };
        }

        public FeatureEvidence DuplicationEvidence { get; } = new FeatureEvidence();
    }
}
