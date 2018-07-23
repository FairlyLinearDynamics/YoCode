using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace YoCode
{
    public class WebWriter : IPrint
    {
        const string TEMPLATE_FILE = @"C:\Users\ukekar\source\repos\YoCode\YoCode\HTMLTemplate.html";
        const string OUTPUT_FILE = @"C:\Users\ukekar\source\repos\YoCode\output.html";

        const string FEATURE_TAG = "{FEATURE}";

        string template;
        StringBuilder msg;

        public WebWriter()
        {
            template = File.ReadAllText(TEMPLATE_FILE);
            msg = new StringBuilder();
        }

        public void AddDiv()
        {
            msg.Append("<br>");
        }

        public void AddNewLine(string text)
        {
            msg.Append(text);
        }

        public void WriteAndFlush()
        {
            template = template.Replace(FEATURE_TAG, msg.ToString() + FEATURE_TAG);
            File.WriteAllText(OUTPUT_FILE, template);
        }
    }
}
