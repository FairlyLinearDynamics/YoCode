using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoCode
{
    internal class UnitConverterCheck : ICheck
    {
        private readonly string port;
        private Dictionary<List<string>, List<double>> KeywordMap;

        private List<UnitConverterResults> actual;
        private List<UnitConverterResults> expected;

        private List<double> texts;

        public List<string> actions { get; set; }

        private List<double> InchesToCentimetres;
        private List<double> MilesToKilometres;
        private List<double> YardsToMeters;

        private const double InToCm = 2.54;
        private const double MiToKm = 1.60934;
        private const double YdToMe = 0.9144;

        private List<string> InToCmKeys { get; set; }
        private List<string> MiToKmKeys { get; set; }
        private List<string> YdToMeKeys { get; set; }

        List<bool> UnitConverterBoolResults;
        
        private const int TitleColumnFormatter = -30;
        private const int ValueColumnFormatter = -15;

        private StringBuilder unitConverterResultsOutput = new StringBuilder();

        BackEndStringHandling handler;

        private string from = "value=\"";
        private string to = "\"";

        public UnitConverterCheck(string port)
        {
            this.port = port;
        }

        private void InitializeDataStructures(string htmlCode)
        {
            KeywordMap = new Dictionary<List<string>, List<double>>();

            actual = new List<UnitConverterResults>();
            expected = new List<UnitConverterResults>();

            texts = new List<double> { 5, 25, 125 };

            InchesToCentimetres = handler.MakeConversion(texts, InToCm);
            MilesToKilometres = handler.MakeConversion(texts, MiToKm);
            YardsToMeters = handler.MakeConversion(texts, YdToMe);

            InToCmKeys = new List<string> { "inc", "in", "inch", "inches", "cm", "centimetres", "centimetre" };
            MiToKmKeys = new List<string> { "miles", "mi", "mile", "kilo", "kilometres", "kilometre" };
            YdToMeKeys = new List<string> { "yards", "yard", "yardstometers", "tometers" };

            actions = handler.GetListOfActions(htmlCode,from,to);

            KeywordMap.Add(InToCmKeys, InchesToCentimetres);
            KeywordMap.Add(MiToKmKeys, MilesToKilometres);
            KeywordMap.Add(YdToMeKeys, YardsToMeters);

            InitializeExpectedValues();

            UnitConverterBoolResults = new List<bool>();
        }

        private void InitializeExpectedValues()
        {
            var ToBeAdded = new UnitConverterResults();
            for (var x = 0; x < texts.Count; x++)
            {
                for (var y = 0; y < actions.Count; y++)
                {
                    var OutputsForThisAction = handler.CheckActions(actions[y],KeywordMap);

                    ToBeAdded.input = texts[x];
                    ToBeAdded.action = actions[y];
                    ToBeAdded.output = OutputsForThisAction[x];

                    expected.Add(ToBeAdded);
                }
            }
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

        private double GetUnitConverterCheckRating()
        {
            return HelperMethods.GetRatingFromBoolList(UnitConverterBoolResults);
        }

        public static UnitConverterResults FindActualResultForExpectation(UnitConverterResults expectation, List<UnitConverterResults> listOfActualResults)
        {
            return listOfActualResults.Single(result => result.action == expectation.action && result.input.ApproximatelyEquals(expectation.input));
        }

        public Task<List<FeatureEvidence>> Execute()
        {
            return Task.Run(() =>
            {
                UnitConverterCheckEvidence.Feature = Feature.UnitConverterCheck;
                UnitConverterCheckEvidence.HelperMessage = messages.UnitConverterCheck;


                if (string.IsNullOrEmpty(port))
                {
                    UnitConverterCheckEvidence.SetInconclusive(new SimpleEvidenceBuilder(messages.BadPort));
                    return new List<FeatureEvidence> { UnitConverterCheckEvidence};
                }

                try
                {
                    var fetcher = new HTMLFetcher(port);
                    handler = new BackEndStringHandling();

                    var htmlCode = fetcher.GetHTMLCodeAsString();
                    InitializeDataStructures(htmlCode);
                    actual = fetcher.GetActualValues(texts, actions);

                    if (OutputsAreEqual())
                    {
                        UnitConverterCheckEvidence.SetPassed(new SimpleEvidenceBuilder(unitConverterResultsOutput.ToString()));
                    }
                    else
                    {
                        UnitConverterCheckEvidence.SetFailed(new SimpleEvidenceBuilder(unitConverterResultsOutput.ToString()));
                    }

                    UnitConverterCheckEvidence.FeatureRating = GetUnitConverterCheckRating();
                
                }
                catch (Exception)
                {
                    UnitConverterCheckEvidence.SetInconclusive(new SimpleEvidenceBuilder("Could not check this feature"));
                }

                return new List<FeatureEvidence> {UnitConverterCheckEvidence};
            });
        }

        private FeatureEvidence UnitConverterCheckEvidence { get; } = new FeatureEvidence();
    }
}