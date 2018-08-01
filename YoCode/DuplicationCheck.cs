using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace YoCode
{
    public class DuplicationCheck
    {
        private readonly string fileNameChecked = "UnitConverterWebApp.sln";

        private readonly string modifiedSolutionPath;
        private readonly string originalSolutionPath;
        private readonly IDupFinder dupFinder;
        private readonly IPathManager dir;

        private int ModiCodeBaseCost { get; set; }
        private int ModiDuplicateCost { get; set; }

        private int OrigCodeBaseCost { get; set; }
        private int OrigDuplicateCost { get; set; }

        public DuplicationCheck(IPathManager dir, IDupFinder dupFinder)
        {
            this.dir = dir;
            DuplicationEvidence.FeatureTitle = "Code quality improvement";
            this.dupFinder = dupFinder;

            modifiedSolutionPath = Path.Combine(dir.modifiedTestDirPath, fileNameChecked);
            originalSolutionPath = Path.Combine(dir.originalTestDirPath, fileNameChecked);

            try
            {
                ExecuteTheCheck();
                SpecialDuplicatuib();
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

        private void SpecialDuplicatuib()
        {
            // TODO: get a list of cs files excluding ones from Unit Test
            var csUris = dir.GetFilesInDirectory(dir.modifiedTestDirPath,FileTypes.cs);

            var csUrisWithoutUnitTests = csUris.Where(a => !a.Contains("UnitConverterTests"));

            csUrisWithoutUnitTests.ToList().ForEach(Console.WriteLine);

            string regexForNumbers = "[0-9]+\\.?[0-9]*";
            string regexForText = "Yards to meters";

            var yardCount = 0;
            var inchCount = 0;
            var mileCount = 0;
            var stringCount = 0;

            foreach (var csFile in csUrisWithoutUnitTests)
            {
                var file = File.ReadAllText(csFile);
                yardCount += CountRepetition(regexForNumbers, "0.914", file);
                inchCount += CountRepetition(regexForNumbers, "2.54", file);
                mileCount += CountRepetition(regexForNumbers, "1.609", file);
                stringCount += CountRepetition(regexForText, regexForText, file);

            }

            DuplicationEvidence.GiveEvidence($"Number 0.914 repeated: {yardCount}");
            DuplicationEvidence.GiveEvidence($"Number 2.54 repeated: {yardCount}");
            DuplicationEvidence.GiveEvidence($"Number 1.609 repeated: {yardCount}");
            DuplicationEvidence.GiveEvidence($"Number string repeated: {stringCount}");

            // TODO: Check for duplication of "Yards to meters"
        }

        private int CountRepetition(string regexPattern, string valueToCheckAgainst ,string fileToReadFrom)
        {
            var elements = Regex.Matches(fileToReadFrom, regexPattern);
            return elements.Where(element => element.Value.Contains(valueToCheckAgainst)).Count();
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
