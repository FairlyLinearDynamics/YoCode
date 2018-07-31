using System;
using System.Collections.Generic;
using System.Net;
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

                    var response = SubmitForm(input.ToString(), action);
                    response.Wait();

                    var tempOutput = await GetResponseAsTaskAsync(response.Result);
                    tempActual.output = double.Parse(tempOutput);
                   
                    actual.Add(tempActual);
                }
            }
            return actual;
        }

        public  Task<HttpResponseMessage> SubmitForm(string inputForm,string action)
        {
            var formContent = GetEncodedContent(inputForm, action);

            return client.PostAsync("/Home/Convert", formContent);
        }


        public bool FixedBadInputs(List<string> inputs, List<string> actions)
        {
            foreach (var input in inputs)
            {
                foreach (var action in actions)
                {
                    var x = SubmitForm(input, action);
                    x.Wait();

                    if(x.Result.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public List<UnitConverterResults> GetActualValues(List<double> texts, List<string> actions)
        {
            var task = GetActionNamesAndOutputsViaHTTP(texts, actions);
            task.Wait();
            return task.Result;
        }
        
        public List<string> GetInternalServerErrorKeywords()
        {
            return new List<String> { "Internal "};
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
