using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace YoCode
{
    //Part 1. Getting "action" names
    //Part 2. Testing "actions" with values and expected answers
    // If all of them are correct, the test has passed

    class WebControllerEndpoint
    {
        List<string> texts;
        List<string> actions;


        string from = "value=\"";
        string to = "\"";

        HttpClient client;

        private string HTMLcode;

        public WebControllerEndpoint(string port)
        {
            client = new HttpClient { BaseAddress = new Uri(port) };
            GetHTMLCodeAsString();
            InitializeLists();
            ExecuteTheCheck();
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

        public async void ExecuteTheCheck()
        {
            for (int i = 0; i < texts.Count; i++)
            {
                for (int j = 0; j < actions.Count; j++)
                {
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string,string>("text",texts[i]),
                        new KeyValuePair<string, string>("action",actions[j])
                    });

                    var bar = await client.PostAsync("/Home/Convert", formContent);

                    var baz = await bar.Content.ReadAsStringAsync();
                    Console.WriteLine(i + " " + j + " " + texts[i] + actions[j]);
                    Console.WriteLine(baz);
                }
            }
        }

        public List<string> GetActionKeywords()
        {
            return new List<string> { "action", "value" };
        }

    }
}
