using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace YoCode
{
    public class UnitConverterCheck
    {
        Dictionary<List<string>,List<double>> KeywordMap;

        List<UnitConverterResults> actual;
        List<UnitConverterResults> expected;

        List<double> texts;
        List<string> actions;
           
        List<double> InchesToCentimetres;
        List<double> MilesToKilometres;
        List<double> YardsToMeters;

        const double InToCm = 2.54;
        const double MiToKm = 1.60934;
        const double YdToMe = 0.9144;

        public List<string> InToCmKeys { get; set; }
        public List<string> MiToKmKeys { get; set; }
        public List<string> YdToMeKeys { get; set; }        
        
        string from = "value=\"";
        string to = "\"";

        private string HTMLcode;

        public UnitConverterCheck(string port)
        {
            if (String.IsNullOrEmpty(port))
            {
                UnitConverterCheckEvidence.SetFailed("The unit converter check was not implemented: could not retrieve the port number\nAnother program might be using it.");
            }
            else
            {
                try
                {
                    var fetcher = new HTMLFetcher(port);
                    UnitConverterCheckEvidence.FeatureTitle = "Units were converted successfully";

                    HTMLcode = fetcher.GetHTMLCodeAsString();
                    InitializeDataStructures();
                    actual = fetcher.GetActualValues(texts, actions);
                    UnitConverterCheckEvidence.FeatureImplemented = OutputsAreEqual();    
                }
                catch (Exception)
                {
                    UnitConverterCheckEvidence.SetFailed("The program could not check this feature");
                }

            }
        }

        private void InitializeDataStructures()
        {
            KeywordMap = new Dictionary<List<string>, List<double>>();

            actual = new List<UnitConverterResults>();
            expected = new List<UnitConverterResults>();

            texts = new List<double> { 5, 25, 125};

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
            return new List<double>{0.1};
        }

        private bool OutputsAreEqual()
        {
            var ret = true;
            try
            {
                UnitConverterCheckEvidence.GiveEvidence(string.Format("{0,-9} {1,10}", "Expected", "Actual"));

                foreach (var expectation in expected)
                {
                    var expectedOutput = expectation.output;
                    var actualOutput = FindActualResultForExpectation(expectation, actual).output;

                    var x = string.Format("{0,-9} and {1,-9} Are equal: {2,-4} ", expectedOutput, actualOutput, actualOutput.ApproximatelyEquals(expectedOutput));
                    UnitConverterCheckEvidence.GiveEvidence(x);

                    if (!actualOutput.ApproximatelyEquals(expectedOutput))
                    {
                        ret = false;
                    }
                }
            }
            catch (Exception)
            {
                UnitConverterCheckEvidence.SetFailed("Unit converting has failed");
                ret = false;
            }
            return ret;
        }

        public static UnitConverterResults FindActualResultForExpectation(UnitConverterResults expectation, List<UnitConverterResults> listOfActualResults)
        {
            return listOfActualResults.Single(result => result.action == expectation.action && result.input.ApproximatelyEquals(expectation.input));
        }
        public FeatureEvidence UnitConverterCheckEvidence { get; } = new FeatureEvidence();
    }
}
