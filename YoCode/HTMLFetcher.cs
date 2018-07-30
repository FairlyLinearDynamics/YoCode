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

            for (var i = 0; i < texts.Count; i++)
            {
                UnitConverterResults tempActual = new UnitConverterResults();
                tempActual.input = Double.Parse(texts[i].ToString());

                for (var j = 0; j < actions.Count; j++)
                {
                    tempActual.action = actions[i];

                    var formContent = GetEncodedContent(texts[i].ToString(), actions[j]);

                    var bar = await client.PostAsync("/Home/Convert", formContent);

                    string tempOutput = await GetResponseAsTaskAsync(bar);
                    tempActual.output = Double.Parse(tempOutput);

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
