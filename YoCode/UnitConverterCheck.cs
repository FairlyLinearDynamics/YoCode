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
        Dictionary<List<string>,List<double>> map;

        List<string> texts;
        List<string> actions;
           
        List<double> InchesToCentimetres;
        List<double> MilesToKilometres;
        List<double> YardsToMeters;

        double InToCm = 2.54;
        double MiToKm = 1.60934;
        double YdToMe = 0.9144;

        List<string> InToCmKeys;
        List<string> MiToKmKeys;
        List<string> YdToMeKeys;
       
        List<UnitConverterResults> actual;
        
        string from = "value=\"";
        string to = "\"";

        HttpClient client;

        private string HTMLcode;
        string tempResultStorage;

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
                Setup();
                RunTheCheck();
                UnitConverterCheckEvidence.FeatureImplemented = OutputsAreEqual();
                PrintProgress();

            }
        }

        private void Setup()
        {
            GetHTMLCodeAsString();
            InitializeLists();
        }

        private void RunTheCheck()
        {
            var task = ExecuteTheCheck();
            task.Wait();
            actual = task.Result;
        }

        public List<string> GetActions(string HTMLfile)
        {
            var actionlines = GetActionLines(HTMLfile);
            return ExtractActions(actionlines);
        }

        public async Task<string> GetHTMLCodeAsTask()
        { 
            return await client.GetStringAsync("/");
        }
       
        public async void GetHTMLCodeAsString()
        {
            HTMLcode = GetHTMLCodeAsTask().Result;
        }
        
        public List<string> GetActionLines(string file)
        {
            return file.GetMultipleLinesWithAllKeywords(GetActionKeywords());
        }

        public List<string> ExtractActions(List<string> actionLines)
        {
            var list = new List<string>();

            foreach (string line in actionLines)
            {
                string res = line.GetStringBetweenStrings(from, to);

                list.Add(res);
            }
            return list;
        }

        public void InitializeLists()
        {
            map = new Dictionary<List<string>, List<double>>();
            actual = new List<UnitConverterResults>();
            texts = new List<string> { "5", "25", "125" };

            InchesToCentimetres = MakeConversion(texts, InToCm);
            MilesToKilometres = MakeConversion(texts, MiToKm);
            YardsToMeters = MakeConversion(texts, YdToMe);

            InToCmKeys = new List<string> {"inc","in","inch","inches","cm","centimetres","centimetre" };
            MiToKmKeys = new List<string> {"miles","mi","mile","kilo","kilometres","kilometre"};
            YdToMeKeys = new List<string> {"yards","yard","yardstometers","tometers"  };
                
            actions = GetActions(HTMLcode);

            FillMap();
            //CheckActionNames();
        }

        public void FillMap()
        {
            map.Add(InToCmKeys, InchesToCentimetres);
            map.Add(MiToKmKeys, MilesToKilometres);
            map.Add(YdToMeKeys, YardsToMeters);
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

        public async Task<List<UnitConverterResults>> ExecuteTheCheck()
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

        public int CheckActionNames(List<string> keywords)
        {
            for(int i=0;i<actions.Count;i++)
            {
                foreach(string y in keywords)
                {
                    if (actions[i].ContainsAny(keywords)){

                        return i;

                    }
                }                                   
            }
            return -1;
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

        public async void GetResponseAsString(Task<string> bar)
        {
            tempResultStorage = bar.Result;
        }

        public List<double> ConvertAnswersToDouble(List<String> strings)
        {
            return strings.Select(s => double.Parse(s)).ToList();
        }

        public List<string> GetActionKeywords()
        {
            return new List<string> { "action", "value" };
        }

        public List<double> CheckActions(string action)
        {
                foreach (var keywords in map)
                {
                    if (action.ContainsAny(keywords.Key))
                    {
                        return keywords.Value;
                    }
                }
            return new List<double>{0.0};
        }

        public bool OutputsAreEqual()
        {
            bool ret = true;
            UnitConverterCheckEvidence.GiveEvidence(String.Format("{0,-9} {1,10}","Expected","Actual"));

            int cur = 0;

            for(int i=0; i<actual.Count;i++)
            {
                var toCheck = CheckActions(actual[i].action);

                foreach(var x in toCheck)
                {
                    if( x.ApproximatelyEquals(Double.Parse(actual[i].output)))
                    {
                        Console.WriteLine(x + " " + actual[i].output);
                    }
                }


            }

            return true;
        }

        //public bool OutputsAreEqual()
        //{
        //    bool ret = true;

        //    UnitConverterCheckEvidence.GiveEvidence(String.Format("{0,-9} {1,10}","Expected","Actual"));

        //    for(int i = 0; i < actual.Count; i++)
        //    {
        //        (var A, var B) = ToDouble(expectedOutputs[i], actual[i].output);

        //        var x = String.Format("{0,-9} and {1,-9} Are equal: {2,-4} ", Math.Round(A,6), Math.Round(B,6), A.ApproximatelyEquals(B));
        //        UnitConverterCheckEvidence.GiveEvidence(x);

        //        if (!A.ApproximatelyEquals(B))
        //        {
        //            ret = false;
        //        }
        //    }
        //    return ret;
        //}

        
        public void PrintProgress()
        {
            //foreach(string x in actions)
            //{
            //    Console.WriteLine(x);

            //    foreach(double y in CheckActions(x.ToLower()))
            //    {
            //        Console.WriteLine(y);
            //    }
            //    Console.WriteLine("-------------");
            //}

            foreach(var y in actual)
            {
                Console.WriteLine(y.action);
            }

        }



        public (double,double) ToDouble(string a, string b)
        {
            return (Double.Parse(a), Double.Parse(b));
        }

        public FeatureEvidence UnitConverterCheckEvidence { get; } = new FeatureEvidence();
    }
}
