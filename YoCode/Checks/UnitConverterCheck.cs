﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoCode
{
    internal class UnitConverterCheck
    {
        private Dictionary<List<string>, List<double>> KeywordMap;
        private Dictionary<string, string> badInputs;

        private readonly List<string> badInputResults;

        private List<UnitConverterResults> actual;
        private List<UnitConverterResults> expected;

        private List<double> texts;

        private List<string> actions;

        private List<double> InchesToCentimetres;
        private List<double> MilesToKilometres;
        private List<double> YardsToMeters;

        private const double InToCm = 2.54;
        private const double MiToKm = 1.60934;
        private const double YdToMe = 0.9144;

        public List<string> InToCmKeys { get; set; }
        public List<string> MiToKmKeys { get; set; }
        public List<string> YdToMeKeys { get; set; }

        List<bool> BadInputBoolResults;
        List<bool> UnitConverterBoolResults;
        
        string from = "value=\"";
        string to = "\"";

        private readonly string HTMLcode;

        private const int TitleColumnFormatter = -30;
        private const int ValueColumnFormatter = -15;

        private StringBuilder unitConverterResultsOutput = new StringBuilder();
        private StringBuilder badInputResultsOutput= new StringBuilder();

        public UnitConverterCheck(string port)
        {
            UnitConverterCheckEvidence.Feature = Feature.UnitConverterCheck;
            UnitConverterCheckEvidence.HelperMessage = messages.UnitConverterCheck;

            BadInputCheckEvidence.Feature = Feature.BadInputCheck;
            BadInputCheckEvidence.HelperMessage = messages.BadInputCheck;

            if (String.IsNullOrEmpty(port))
            {
                UnitConverterCheckEvidence.SetInconclusive(new SimpleEvidenceBuilder(messages.BadPort));
                BadInputCheckEvidence.SetInconclusive(new SimpleEvidenceBuilder(messages.BadPort));
            }
            else
            {
                try
                {
                    var fetcher = new HTMLFetcher(port);

                    HTMLcode = fetcher.GetHTMLCodeAsString();
                    InitializeDataStructures();
                    actual = fetcher.GetActualValues(texts, actions);

                    badInputResults = fetcher.GetBadInputs(badInputs, actions[0]);

                    if (OutputsAreEqual())
                    {
                        UnitConverterCheckEvidence.SetPassed(new SimpleEvidenceBuilder(unitConverterResultsOutput.ToString()));
                    }
                    else
                    {
                        UnitConverterCheckEvidence.SetFailed(new SimpleEvidenceBuilder(unitConverterResultsOutput.ToString()));
                    }
                    UnitConverterCheckEvidence.FeatureRating = GetUnitConverterCheckRating();

                    if (BadInputsAreFixed())
                    {
                        BadInputCheckEvidence.SetPassed(new SimpleEvidenceBuilder(badInputResultsOutput.ToString()));
                    }
                    else
                    {
                        BadInputCheckEvidence.SetFailed(new SimpleEvidenceBuilder(badInputResultsOutput.ToString()));
                    }
                    BadInputCheckEvidence.FeatureRating = GetBadInputCheckRating();
                }
                catch (Exception)
                {
                    UnitConverterCheckEvidence.SetInconclusive(new SimpleEvidenceBuilder("Could not check this feature"));
                    BadInputCheckEvidence.SetInconclusive(new SimpleEvidenceBuilder("Could not check this feature"));
                }
            }
        }

        private void InitializeDataStructures()
        {
            KeywordMap = new Dictionary<List<string>, List<double>>();

            actual = new List<UnitConverterResults>();
            expected = new List<UnitConverterResults>();

            texts = new List<double> { 5, 25, 125 };

            badInputs = new Dictionary<string, string>
            {
                { "Empty input", " " },
                { "Blank lines at the start", "\n10" },
                { "Blank lines at the middle", "10 \n\n 10" },
                { "Blank lines at the end", "10 \n\n" },
                { "Not numbers", "Y..@" }
            };

            InchesToCentimetres = MakeConversion(texts, InToCm);
            MilesToKilometres = MakeConversion(texts, MiToKm);
            YardsToMeters = MakeConversion(texts, YdToMe);

            InToCmKeys = new List<string> { "inc", "in", "inch", "inches", "cm", "centimetres", "centimetre" };
            MiToKmKeys = new List<string> { "miles", "mi", "mile", "kilo", "kilometres", "kilometre" };
            YdToMeKeys = new List<string> { "yards", "yard", "yardstometers", "tometers" };

            actions = GetListOfActions(HTMLcode);

            KeywordMap.Add(InToCmKeys, InchesToCentimetres);
            KeywordMap.Add(MiToKmKeys, MilesToKilometres);
            KeywordMap.Add(YdToMeKeys, YardsToMeters);

            InitializeExpectedValues();

            BadInputBoolResults = new List<bool>();
            UnitConverterBoolResults = new List<bool>();
        }

        private void InitializeExpectedValues()
        {
            var ToBeAdded = new UnitConverterResults();
            for (var x = 0; x < texts.Count; x++)
            {
                for (var y = 0; y < actions.Count; y++)
                {
                    var OutputsForThisAction = CheckActions(actions[y]);

                    ToBeAdded.input = texts[x];
                    ToBeAdded.action = actions[y];
                    ToBeAdded.output = OutputsForThisAction[x];

                    expected.Add(ToBeAdded);
                }
            }
        }

        public List<string> GetActionLines(string file)
        {
            return file.GetMultipleLinesWithAllKeywords(GetActionKeywords());
        }

        private List<string> GetListOfActions(string HTMLfile)
        {
            var actionlines = GetActionLines(HTMLfile);
            return ExtractActionsFromList(actionlines);
        }

        public List<string> ExtractActionsFromList(List<string> actionLines)
        {
            var list = new List<string>();

            foreach (var line in actionLines)
            {
                var res = line.GetStringBetweenStrings(from, to);

                list.Add(res);
            }
            return list;
        }

        public List<double> MakeConversion(List<double> inputs, double mult)
        {
            var list = new List<double>();
            foreach (var x in inputs)
            {
                list.Add(x * mult);
            }
            return list;
        }

        private List<string> GetActionKeywords()
        {
            return new List<string> { "action", "value" };
        }

        private List<double> CheckActions(string action)
        {
            foreach (var keywords in KeywordMap)
            {
                if (action.ToLower().ContainsAny(keywords.Key))
                {
                    return keywords.Value;
                }
            }
            return new List<double> { 0.1 };
        }

        private bool OutputsAreEqual()
        {
            var ret = true;
            try
            {
                unitConverterResultsOutput.AppendLine(Environment.NewLine + string.Format(
                    $"{"Action",TitleColumnFormatter} {"Input",ValueColumnFormatter} {"Expected",ValueColumnFormatter} {"Actual",ValueColumnFormatter} {"Are equal\n",ValueColumnFormatter}"));

                unitConverterResultsOutput.AppendLine(messages.ParagraphDivider);
                foreach (var expectation in expected)
                {
                    var expectedOutput = expectation.output;
                    var actualOutput = FindActualResultForExpectation(expectation, actual).output;

                    var x = string.Format($"{expectation.action,TitleColumnFormatter} {expectation.input,ValueColumnFormatter} {expectedOutput,ValueColumnFormatter} {actualOutput,ValueColumnFormatter} {actualOutput.ApproximatelyEquals(expectedOutput),ValueColumnFormatter} ");
                    unitConverterResultsOutput.AppendLine(x);

                    UnitConverterBoolResults.Add(actualOutput.ApproximatelyEquals(expectedOutput));

                    if (!actualOutput.ApproximatelyEquals(expectedOutput))
                    {
                        ret = false;
                    }
                }
            }
            catch (Exception)
            {
                UnitConverterCheckEvidence.SetInconclusive(new SimpleEvidenceBuilder("Unit converting has failed"));
                ret = false;
            }
            return ret;
        }

        public bool BadInputsAreFixed()
        {
            bool ret = true;

            badInputResultsOutput.AppendLine(string.Format($"\n{"Input name",TitleColumnFormatter} {"FIXED",ValueColumnFormatter}"));
            badInputResultsOutput.AppendLine(messages.ParagraphDivider);

            foreach (var a in badInputs)
            {
                bool isFixed = !badInputResults.Contains(a.Key);
                BadInputBoolResults.Add(isFixed);

                if (!isFixed)
                {
                    ret = false;
                }

                badInputResultsOutput.AppendLine(string.Format($"{a.Key,TitleColumnFormatter} {isFixed,ValueColumnFormatter}"));
            }
            return ret;
        }

        public double GetBadInputCheckRating()
        {
            return HelperMethods.GetRatingFromBoolList(BadInputBoolResults);
        }

        public double GetUnitConverterCheckRating()
        {
            return HelperMethods.GetRatingFromBoolList(UnitConverterBoolResults);
        }

        public static UnitConverterResults FindActualResultForExpectation(UnitConverterResults expectation, List<UnitConverterResults> listOfActualResults)
        {
            return listOfActualResults.Single(result => result.action == expectation.action && result.input.ApproximatelyEquals(expectation.input));
        }

        public FeatureEvidence UnitConverterCheckEvidence { get; } = new FeatureEvidence();
        public FeatureEvidence BadInputCheckEvidence { get; } = new FeatureEvidence();
    }
}