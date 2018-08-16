using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace YoCode
{
    internal class DuplicationCheck
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

        private const int VARIABLE_REPETITION_TRESHOLD = 1;

        private const string yardsToMeters = "0.914";
        private const string inchToCentimeter = "2.54";
        private const string mileToKilometer = "1.60934";
        private const string stringCheck = "Yards to meters";

        public DuplicationCheck(IPathManager dir, IDupFinder dupFinder)
        {
            this.dir = dir;
            DuplicationEvidence.FeatureTitle = "Code quality improvement";
            DuplicationEvidence.Feature = Feature.DuplicationCheck;
            this.dupFinder = dupFinder;

            modifiedSolutionPath = Path.Combine(dir.ModifiedTestDirPath, fileNameChecked);
            originalSolutionPath = Path.Combine(dir.OriginalTestDirPath, fileNameChecked);

            try
            {
                ExecuteTheCheck();
                CheckForSpecialRepetition();
            }
            catch (FileNotFoundException) { }
            catch (Exception e)
            {
                DuplicationEvidence.FeatureImplemented = false;
                DuplicationEvidence.GiveEvidence(messages.DupFinderHelp + "\n" + e);
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
            DuplicationEvidence.FeatureRating = GetDuplicationCheckRating();

            DuplicationEvidence.GiveEvidence(origEvidence, modEvidence);
        }

        private void CheckForSpecialRepetition()
        {
            var csUris = dir.GetFilesInDirectory(dir.ModifiedTestDirPath,FileTypes.cs);

            var csUrisWithoutUnitTests = csUris.Where(a => !a.Contains("UnitConverterTests")).ToList();

            var htmlUris = dir.GetFilesInDirectory(dir.ModifiedTestDirPath, FileTypes.html).ToList();

            var combinedList = csUrisWithoutUnitTests.Concat(htmlUris);

            var stringRep = 0;
            var yardRepetition = 0;
            var inchRepetition = 0;
            var mileRepetition = 0;

            string regexPatternForInts = "[0-9]+\\.?[0-9]*";

            foreach (var csFile in combinedList)
            {
                var file = File.ReadAllText(csFile);

                yardRepetition += CountRepetition(yardsToMeters, file, regexPatternForInts);
                inchRepetition += CountRepetition(inchToCentimeter, file, regexPatternForInts);
                mileRepetition += CountRepetition(mileToKilometer, file, regexPatternForInts);

                stringRep += CountRepetition(stringCheck, file, stringCheck);
            }

            if (yardRepetition > VARIABLE_REPETITION_TRESHOLD)
            {
                DuplicationEvidence.GiveEvidence($"Number {yardsToMeters} duplicated {yardRepetition} times");
            }
            if (inchRepetition > VARIABLE_REPETITION_TRESHOLD)
            {
                DuplicationEvidence.GiveEvidence($"Number {inchToCentimeter} duplicated {inchRepetition} times");
            }
            if (mileRepetition > VARIABLE_REPETITION_TRESHOLD)
            {
                DuplicationEvidence.GiveEvidence($"Number {mileToKilometer} duplicated {mileRepetition} times");
            }
            if (stringRep > VARIABLE_REPETITION_TRESHOLD)
            {
                DuplicationEvidence.GiveEvidence($"String \"Yards to meters\" duplicated {stringRep}");
            }
        }

        public double GetDuplicationCheckRating()
        {
            double UpperBound = 628;
            double LowerBound = 174;
            double range = UpperBound - LowerBound;

            return ModiDuplicateCost >= UpperBound ? 0 : 1-Math.Round((ModiDuplicateCost - LowerBound) / range,2);
        }


        private int CountRepetition(string valueToCheckAgainst ,string fileToReadFrom, string regexPattern)
        {
            var elements = Regex.Matches(fileToReadFrom, regexPattern);
            return elements.Count(element => element.Value.Contains(valueToCheckAgainst));
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
