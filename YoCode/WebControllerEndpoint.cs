using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace YoCode
{
    //Part 1. Getting "action" names
    //Part 2. Testing "actions" with values and expected answers
    // If all of them are correct, the test has passed

    class WebControllerEndpoint
    {
        List<string> texts;
        List<string> actions;


        public WebControllerEndpoint(string port)
        {
            HttpClient client = new HttpClient { BaseAddress = new Uri("http://localhost:57009") };



            InitializeLists();
            ExecuteTheCheck();
        }

        public async string GetHTMLFile()
        {

        }


        public void readActions()
        {

        }

        public void InitializeLists()
        {
            texts = new List<string> { "5", "25", "125" };
            actions = new List<string>{"Yards to meters", "Inches to centimeters", "Miles to kilometers"};
        }

        public async void ExecuteTheCheck()
        {
            for(int i = 0; i < texts.Count; i++)
            {
                for (int j = 0; j < actions.Count; j++)
                {
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string,string>("text",texts[i]),
                        new KeyValuePair<string, string>("action",actions[j])
                    });

                    //Post request with above vlaues
                    var bar = await client.PostAsync("/Home/Convert", formContent);
                    //Console.WriteLine(bar);

                    var baz = await bar.Content.ReadAsStringAsync();
                    Console.WriteLine(i + " " + j + " " + texts[i] + actions[j]);
                    Console.WriteLine(baz);
                }
            } 
        }
    }
}
