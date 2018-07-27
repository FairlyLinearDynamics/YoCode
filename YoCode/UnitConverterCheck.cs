using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace YoCode
{
    class UnitConverterCheck
    {
        Dictionary<List<string>,List<double>> KeywordMap;
        List<string> texts;
        List<string> actions;
           
        List<double> InchesToCentimetres;
        List<double> MilesToKilometres;
        List<double> YardsToMeters;

        const double InToCm = 2.54;
        const double MiToKm = 1.60934;
        const double YdToMe = 0.9144;

        List<string> InToCmKeys;
        List<string> MiToKmKeys;
        List<string> YdToMeKeys;
       
        List<UnitConverterResults> actual;
        List<UnitConverterResults> expected;
        
        string from = "value=\"";
        string to = "\"";

        HttpClient client;

        private string HTMLcode;

        public UnitConverterCheck(string port)
        {
            if (String.IsNullOrEmpty(port))
            {
                UnitConverterCheckEvidence.FeatureImplemented = false;
                UnitConverterCheckEvidence.GiveEvidence("The unit converter check was not implemented: could not retrieve the port number\nAnother program might be using it.");
            }
            else
            {
                UnitConverterCheckEvidence.FeatureTitle = "Units were converted successfully";
                client = new HttpClient { BaseAddress = new Uri(port) };
                GetHTMLCodeAsString();
                
                KeywordMap = new Dictionary<List<string>, List<double>>();

                actual = new List<UnitConverterResults>();
                expected = new List<UnitConverterResults>();

                texts = new List<string> { "5", "25", "125" };

                InchesToCentimetres = MakeConversion(texts, InToCm);
                MilesToKilometres = MakeConversion(texts, MiToKm);
                YardsToMeters = MakeConversion(texts, YdToMe);

                InToCmKeys = new List<string> {"inc","in","inch","inches","cm","centimetres","centimetre" };
                MiToKmKeys = new List<string> {"miles","mi","mile","kilo","kilometres","kilometre"};
                YdToMeKeys = new List<string> {"yards","yard","yardstometers","tometers"  };
                
                actions = GetListOfActions(HTMLcode);

                KeywordMap.Add(InToCmKeys, InchesToCentimetres);
                KeywordMap.Add(MiToKmKeys, MilesToKilometres);
                KeywordMap.Add(YdToMeKeys, YardsToMeters);

                UnitConverterResults ToBeAdded = new UnitConverterResults();
                for(int x=0;x<texts.Count;x++)
                {
                    for(int y=0;y<actions.Count;y++)
                    {
                    List<double> outputsForThisAction = CheckActions(actions[y]);

                    ToBeAdded.input = texts[x];
                    ToBeAdded.action = actions[y];
                    ToBeAdded.output = outputsForThisAction[x].ToString();

                    expected.Add(ToBeAdded);
                    }
                }
           
                var task = GetActionNamesViaHTTP();
                task.Wait();
                actual = task.Result;
                UnitConverterCheckEvidence.FeatureImplemented = OutputsAreEqual();
            }
        }        

        public List<string> GetActionLines(string file)
        {
            return file.GetMultipleLinesWithAllKeywords(GetActionKeywords());
        }

        public List<string> GetListOfActions(string HTMLfile)
        {
            var actionlines = GetActionLines(HTMLfile);
            return ExtractActionsFromList(actionlines);
        }

        public List<string> ExtractActionsFromList(List<string> actionLines)
        {
            var list = new List<string>();

            foreach (string line in actionLines)
            {
                string res = line.GetStringBetweenStrings(from, to);

                list.Add(res);
            }
            return list;
        }

        public List<double> MakeConversion(List<string> inputs, double mult)
        {
            var list = new List<double>();
            foreach (string x in inputs)
            {
                list.Add(Double.Parse(x) * mult);
            }
            return list;
        }

        public List<string> GetActionKeywords()
        {
            return new List<string> { "action", "value" };
        }

        public List<double> CheckActions(string action)
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

        public (double,double) StringToDouble(string a, string b)
        {
            return (Double.Parse(a), Double.Parse(b));
        }

        public bool OutputsAreEqual()
        {
            bool ret = false;
            try
            {
                UnitConverterCheckEvidence.GiveEvidence(String.Format("{0,-9} {1,10}", "Expected", "Actual"));

                for (int i = 0; i < actual.Count; i++)
                {
                    (double a,double  b) = StringToDouble(actual[i].output, expected[i].output);                    

                    var x = String.Format("{0,-9} and {1,-9} Are equal: {2,-4} ", b, a, a.ApproximatelyEquals(b));
                    UnitConverterCheckEvidence.GiveEvidence(x);

                    if (!a.ApproximatelyEquals(b))
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

        // HTML stuff

        public async Task<List<UnitConverterResults>> GetActionNamesViaHTTP()
        {
            for (int i = 0; i < texts.Count; i++)
            {
                UnitConverterResults tempActual = new UnitConverterResults();
                tempActual.input = texts[i];

                for (int j = 0; j < actions.Count; j++)
                {
                    tempActual.action = actions[i];
                    var formContent = GetEncodedContent(i, j);

                    var bar = await client.PostAsync("/Home/Convert", formContent);

                    tempActual.output = await GetResponseAsTaskAsync(bar);
                    actual.Add(tempActual);
                }
            }
            return actual;
        }

        public Task<string> GetHTMLCodeAsTask()
        { 
            return client.GetStringAsync("/");
        }
       
        public async void GetHTMLCodeAsString()
        {
            HTMLcode = GetHTMLCodeAsTask().Result;
        }
        
        private FormUrlEncodedContent GetEncodedContent(int i, int j)
        {
            return new FormUrlEncodedContent(new[]
            {
                        new KeyValuePair<string,string>("text",texts[i]),
                        new KeyValuePair<string, string>("action",actions[j])
                    });
        }

        private static Task<string> GetResponseAsTaskAsync(HttpResponseMessage bar)
        {
            return bar.Content.ReadAsStringAsync();
        }

        public FeatureEvidence UnitConverterCheckEvidence { get; } = new FeatureEvidence();
    }
}
