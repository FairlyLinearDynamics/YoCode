using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace YoCode
{
    public class WebWriter : IPrint
    {
        const string TEMPLATE_FILE = @"C:\Users\ukekar\source\repos\YoCode\YoCode\Output\HTMLTemplate.html";
        const string OUTPUT_FILE = @"C:\Users\ukekar\source\repos\YoCode\output.html";

        const string FEATURE_TAG = "{FEATURE}";


        string template;

        StringBuilder features;
        StringBuilder errors;
        StringBuilder msg;
        string introduction;

        public WebWriter()
        {
            template = File.ReadAllText(TEMPLATE_FILE);
            features = new StringBuilder();
            errors = new StringBuilder();
            msg = new StringBuilder();
        }

        public void AddIntro(string text)
        {
            introduction = "<p1>" + text + "</p1>";
        }

        public void AddErr(string err)
        {
            errors.Append($"<li>{err}</li>");
        }

        public void AddMessage(string message)
        {
            msg.Append($"<p1>{message}<br /></p1>");
        }

        public void AddFeature(FeatureData data)
        {
            features.Append("<li>");
            features.Append($"<h1>{data.title}</h1>");
            features.Append($"<p1>{data.featureResult}<br /></p1>");
            if (data.evidence.Any())
            {
                features.Append("<p1>Evidence: <br /></p1>");
                features.Append("<ul>");
                foreach (var feat in data.evidence)
                {
                    features.Append($"<li>{feat}</li>");
                }
                features.Append("</ul>");
            }
            features.Append("</li>");
        }

        private string BuildReport()
        {
            var report = new StringBuilder();
            if (features.Length != 0)
            {
                report.Append(introduction);
                report.Append("<ul>");
                report.Append(features.ToString());
                report.Append("</ul>");
            }
            if (errors.Length != 0)
            {
                report.Append("<h1>Input errors present: </h1>");
                report.Append($"<ul>{errors.ToString()}</ul>");
            }
            if (msg.Length != 0)
            {
                report.Append($"{msg.ToString()}");
            }

            return template.Replace(FEATURE_TAG,report.ToString());
        }

        public void WriteReport()
        {
            File.WriteAllText(OUTPUT_FILE, BuildReport());
        }
    }
}
