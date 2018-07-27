﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace YoCode
{
    class HTMLFetcher
    {
        HttpClient client;
        List<UnitConverterResults> actual;
        public string HTMLcode { get; set; }
       


        public HTMLFetcher(string port)
        {
            client = new HttpClient { BaseAddress = new Uri(port) };
            GetHTMLCodeAsString();
        }

        public Task<string> GetHTMLCodeAsTask()
        {
            return client.GetStringAsync("/");
        }

        public async void GetHTMLCodeAsString()
        {
            HTMLcode = GetHTMLCodeAsTask().Result;
        }


        public async Task<List<UnitConverterResults>> GetActionNamesAndOutputsViaHTTP(List<string> texts, List<string> actions)
        {
            for (int i = 0; i < texts.Count; i++)
            {
                UnitConverterResults tempActual = new UnitConverterResults();
                tempActual.input = texts[i];

                for (int j = 0; j < actions.Count; j++)
                {
                    tempActual.action = actions[i];
                    var formContent = GetEncodedContent(texts[i], actions[j]);

                    var bar = await client.PostAsync("/Home/Convert", formContent);

                    tempActual.output = await GetResponseAsTaskAsync(bar);
                    actual.Add(tempActual);
                }
            }
            return actual;
        }

        public List<UnitConverterResults> GetActualValues(List<string> texts, List<string> actions)
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
