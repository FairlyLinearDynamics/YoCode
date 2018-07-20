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
            client = new HttpClient { BaseAddress = new Uri(port) };
            Setup();
            RunTheCheck();
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
            expectedOutputs = new List<string> { "4.572", "12.7", "8.0467", "22.86", "63.5", "40.2335", "114.3", "317.5", "201.1675" };               
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
                    //string baz = await GetResponseAsTaskAsync(bar);

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

        public void PrintResults(List<UnitConverterResults> results)
        {
            foreach(UnitConverterResults x in results)
            {
                Console.WriteLine("Input: " + x.input);
                Console.WriteLine("Action: " + x.action);
                Console.WriteLine("Output: " + x.output);
                Console.WriteLine("-------------------");
            }
        }

        public bool OutputsAreEqual()
        {
            for(int i = 0; i < actual.Count; i++)
            {
                if (!expectedOutputs[i].Equals(actual[i].output))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
