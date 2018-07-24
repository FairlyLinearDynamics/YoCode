using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace YoCode
{
    class UnitConverterCheck
    {
        List<string> texts;
        List<string> actions;
       
        List<string> expectedOutputs;
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
                UnitConverterCheckEvidence.SetFailed("The unit converter check was not implemented: could not retrieve the port number\nAnother program might be using it.")
            }
            else
            {
                UnitConverterCheckEvidence.FeatureTitle = "Units were converted successfully";
                client = new HttpClient { BaseAddress = new Uri(port) };
                Setup();
                RunTheCheck();
                UnitConverterCheckEvidence.FeatureImplemented = OutputsAreEqual();
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
            actual = new List<UnitConverterResults>();
            texts = new List<string> { "5", "25", "125" };
            expectedOutputs = GetExpectedOutputs();              
            actions = GetActions(HTMLcode);
        }

        public async Task<List<UnitConverterResults>> ExecuteTheCheck()
        {
            for (int i = 0; i < texts.Count; i++)
            {
                UnitConverterResults tempActual = new UnitConverterResults();
                tempActual.input = texts[i];

                for (int j = 0; j < actions.Count; j++)
                {
                    tempActual.action = actions[j];
                    var formContent = GetEncodedContent(i, j);

                    var bar = await client.PostAsync("/Home/Convert", formContent);

                    tempActual.output = await GetResponseAsTaskAsync(bar);
                    actual.Add(tempActual);
                }
            }
            return actual;
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

        public bool OutputsAreEqual()
        {
            bool ret = true;

            UnitConverterCheckEvidence.GiveEvidence(String.Format("{0,-9} {1,10}","Expected","Actual"));

            for(int i = 0; i < actual.Count; i++)
            {
                (var A, var B) = ToDouble(expectedOutputs[i], actual[i].output);

                var x = String.Format("{0,-9} and {1,-9} Are equal: {2,-4} ", Math.Round(A,6), Math.Round(B,6), A.ApproximatelyEquals(B));
                UnitConverterCheckEvidence.GiveEvidence(x);

                if (!A.ApproximatelyEquals(B))
                {
                    ret = false;
                }
            }
            return ret;
        }

        public (double,double) ToDouble(string a, string b)
        {
            return (Double.Parse(a), Double.Parse(b));
        }

        public List<string> GetExpectedOutputs()
        {
            return new List<string> { "4.572", "12.7", "8.04672", "22.86", "63.5", "40.2336", "114.3", "317.5", "201.168"};
        }

        public FeatureEvidence UnitConverterCheckEvidence { get; } = new FeatureEvidence();
    }
}
