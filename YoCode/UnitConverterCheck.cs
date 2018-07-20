using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace YoCode
{
    //Part 1. Getting "action" names
    //Part 2. Testing "actions" with values and expected answers
    // If all of them are correct, the test has passed

    class UnitConverterCheck
    {
        List<string> texts;
        List<string> actions;
        List<string> answers;
        List<double> answers_numbers;

        List<UnitConverterResults> actual;
        List<UnitConverterResults> expected;
       
        string from = "value=\"";
        string to = "\"";

        HttpClient client;

        private string HTMLcode;
        string tempResultStorage;

        public UnitConverterCheck(string port)
        {
            actual = new List<UnitConverterResults>();
            expected = new List<UnitConverterResults>();

            client = new HttpClient { BaseAddress = new Uri(port) };
            GetHTMLCodeAsString();
            InitializeLists();
            var task = ExecuteTheCheck();
            task.Wait();
            answers = task.Result;
            //answers_numbers = ConvertAnswersToDouble(answers);
            PrintResults(actual);
            PrintResults(expected);
         
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
            texts = new List<string> { "5", "25", "125" };
            actions = GetActions(HTMLcode);
        }

        public async Task<List<string>> ExecuteTheCheck()
        {
            var answers = new List<string>();

            for (int i = 0; i < texts.Count; i++)
            {
                UnitConverterResults tempActual = new UnitConverterResults();
                UnitConverterResults tempExpected = new UnitConverterResults();

                tempActual.input = texts[i];
                tempExpected.input = texts[i];

                for (int j = 0; j < actions.Count; j++)
                {
                    tempActual.action = actions[j];
                    tempExpected.action = actions[j];
                        
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string,string>("text",texts[i]),
                        new KeyValuePair<string, string>("action",actions[j])
                    });

                    var bar = await client.PostAsync("/Home/Convert", formContent);
                    string baz = await GetResponseAsTaskAsync(bar);

                    tempActual.output = baz;
                    actual.Add(tempActual);
                    expected.Add(tempExpected);
                }
            }
            return answers;
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

        public void PrintResults(List<UnitConverterResults> results){

            foreach(UnitConverterResults x in results)
            {
                Console.WriteLine("Input: " + x.input);
                Console.WriteLine("Action: " + x.action);
                Console.WriteLine("Output: " + x.output);
                Console.WriteLine("-------------------");
            }

                Console.WriteLine("END----------------");

        }


    }
}
