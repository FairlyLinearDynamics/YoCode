using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace YoCode
{
    class WebControllerEndpoint
    {
        public WebControllerEndpoint(string port)
        {
            ExecuteTheCheck();
        }

        public async void ExecuteTheCheck()
        {
            HttpClient client = new HttpClient { BaseAddress = new Uri("http://localhost:57009") };
            var foo = await client.GetStringAsync("/");
            Console.WriteLine(foo);

            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("text","4"),
                new KeyValuePair<string, string>("action","Yards To Meters")
            });

            var bar = await client.PostAsync("/Home/Convert", formContent);
            Console.WriteLine(bar);

            var baz = await bar.Content.ReadAsStringAsync();
            Console.WriteLine(baz);




        }
    }
}
