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

        public async Task<List<UnitConverterResults>> GetActionNamesAndOutputsViaHTTP(List<double> texts, List<string> actions)
        {
            var actual = new List<UnitConverterResults>();

            foreach (var input in texts)
            {
                var tempActual = new UnitConverterResults {input = double.Parse(input.ToString())};

                foreach (var action in actions)
                {
                    tempActual.action = action;

                    var formContent = GetEncodedContent(input.ToString(), action);

                    var response = await client.PostAsync("/Home/Convert", formContent);

                    var tempOutput = await GetResponseAsTaskAsync(response);
                    tempActual.output = double.Parse(tempOutput);

                    actual.Add(tempActual);
                }
            }
            return actual;
        }

        public List<UnitConverterResults> GetActualValues(List<double> texts, List<string> actions)
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
