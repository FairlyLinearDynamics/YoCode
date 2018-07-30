using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace YoCode
{
    class HTMLFetcher
    {
        private HttpClient client;       

        public HTMLFetcher(string port)
        {
            client = new HttpClient { BaseAddress = new Uri(port) };
            GetHTMLCodeAsString();
        }

        public Task<string> GetHTMLCodeAsTask()
        {
            return client.GetStringAsync("/");
        }

        public string GetHTMLCodeAsString()
        {
            return GetHTMLCodeAsTask().Result;
        }

        public async Task<List<double>> GetActionNamesAndOutputsViaHTTP(List<double> texts, List<string> actions)
        {
            var actualOutputs = new List<double>();

            for (var i = 0; i < texts.Count; i++)
            {
                for (var j = 0; j < actions.Count; j++)
                {
                    var formContent = GetEncodedContent(texts[i].ToString(), actions[j]);

                    var bar = await client.PostAsync("/Home/Convert", formContent);

                    string tempOutput = await GetResponseAsTaskAsync(bar);
                    actualOutputs.Add(Double.Parse(tempOutput));
                }
            }
            return actualOutputs;
        }

        public List<double> GetActualOutputs(List<double> texts, List<string> actions)
        {
            var task = GetActionNamesAndOutputsViaHTTP(texts, actions);
            task.Wait();
            return task.Result;
        }
        
        private FormUrlEncodedContent GetEncodedContent(string i, string j)
        {
            return new FormUrlEncodedContent(new[]
            {
                        new KeyValuePair<string,string>("text",i),
                        new KeyValuePair<string, string>("action",j)
            });
        }

        private static Task<string> GetResponseAsTaskAsync(HttpResponseMessage bar)
        {
            return bar.Content.ReadAsStringAsync();
        }

    }
}
