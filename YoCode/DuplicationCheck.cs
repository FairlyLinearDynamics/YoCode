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
        private readonly IDupFinder dupFinder;
        private readonly IPathManager dir;

        private int ModiCodeBaseCost { get; set; }
        private int ModiDuplicateCost { get; set; }

        private int OrigCodeBaseCost { get; }
        private int OrigDuplicateCost { get; }

        private const int VARIABLE_REPETITION_TRESHOLD = 1;

        private const string yardsToMeters = "0.914";
        private const string inchToCentimeter = "2.54";
        private const string mileToKilometer = "1.60934";
        private const string stringCheck = "Yards to meters";

        private double FeatureImplementedTreshold = 0.5;

        public DuplicationCheck(IPathManager dir, IDupFinder dupFinder, bool isJunior)
        {
            if (isJunior)
            {
                OrigCodeBaseCost = 2381;
                OrigDuplicateCost = 628;
            }
            else
            {
                OrigCodeBaseCost = 2305;
                OrigDuplicateCost = 611;
            }
            DuplicationEvidence.FeatureTitle = "Code quality improvement";
            DuplicationEvidence.Feature = Feature.DuplicationCheck;


            this.dir = dir;
            this.dupFinder = dupFinder;

            modifiedSolutionPath = Path.Combine(dir.ModifiedTestDirPath, fileNameChecked);

            try
            {
                ExecuteTheCheck();
                StructuredOutput();
                CheckForSpecialRepetition();

            }
            catch (FileNotFoundException) { }
            catch (Exception e)
            {
                DuplicationEvidence.SetFailed(messages.DupFinderHelp + "\n" + e);
            }
        }

        private void ExecuteTheCheck()
        {
            (var modEvidence, var modCodeBaseCost, var modDuplicateCost) = RunAndGatherEvidence(modifiedSolutionPath, "Modified");

            if (modEvidence.FeatureFailed)
            {
                DuplicationEvidence.SetFailed($"Failed: {modEvidence.FeatureFailed}");
                return;
            }

            ModiCodeBaseCost = modCodeBaseCost;
            ModiDuplicateCost = modDuplicateCost;

            DuplicationEvidence.FeatureRating = GetDuplicationCheckRating();
            DuplicationEvidence.FeatureImplemented = GetDuplicationCheckRating() >= FeatureImplementedTreshold ? true : false;

        }

        private void CheckForSpecialRepetition()
        {
            var csUris = dir.GetFilesInDirectory(dir.ModifiedTestDirPath, FileTypes.cs);

            var csUrisWithoutUnitTests = csUris.Where(a => !a.Contains("UnitConverterTests")).ToList();

            var htmlUris = dir.GetFilesInDirectory(dir.ModifiedTestDirPath, FileTypes.html).ToList();

            var combinedList = csUrisWithoutUnitTests.Concat(htmlUris);

            var stringRep = 0;
            var yardRepetition = 0;
            var inchRepetition = 0;
            var mileRepetition = 0;

            const string regexPatternForInts = "[0-9]+\\.?[0-9]*";

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
                DuplicationEvidence.GiveEvidence($"String \"Yards to meters\" duplicated {stringRep} times");
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

            evidence.GiveEvidence(BuildEvidenceString(whichDir, codebaseCost, duplicateCost));
            return (evidence, codebaseCost, duplicateCost);
        }


        public void StructuredOutput()
        {
            DuplicationEvidence.GiveEvidence(String.Format("{0,-15}{1}{2,20}", "Version", "Codebase Cost", "Duplicate Cost"));
            DuplicationEvidence.GiveEvidence(messages.ParagraphDivider);
            DuplicationEvidence.GiveEvidence(String.Format("{0,-15}{1,8}{2,18}", "Original", OrigCodeBaseCost, OrigDuplicateCost));
            DuplicationEvidence.GiveEvidence(String.Format("{0,-15}{1,8}{2,18}", "Modified", ModiCodeBaseCost, ModiDuplicateCost));
            DuplicationEvidence.GiveEvidence(Environment.NewLine);
        }

        private string BuildEvidenceString(string whichDir, int codebaseCost, int duplicateCost)
        {
            return "Original" + Environment.NewLine + "Codebase cost: " + OrigCodeBaseCost + Environment.NewLine +
                   "Duplicate cost: " + OrigDuplicateCost + Environment.NewLine + whichDir + Environment.NewLine +
                   $"Codebase cost: {codebaseCost} {Environment.NewLine}Duplicate cost: {duplicateCost}";
        }

        private FeatureEvidence RunOneCheck(string solutionPath)
        {
            return dupFinder.Execute("Dup check", solutionPath);
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
