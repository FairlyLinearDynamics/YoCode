using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace YoCode
{
    public class WebWriter : IPrint
    {
        const string TEMPLATE_FILE = @"C:\Users\ukekar\source\repos\YoCode\webReport\HTMLTemplate.html";
        const string OUTPUT_FILE = @"C:\Users\ukekar\source\repos\YoCode\webReport\output.html";

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
            introduction = WebTemplateBuilder.FormatParagraph(text).ToString();
        }

        public void AddErrs(IEnumerable<string> err)
        {
            errors.Append(WebTemplateBuilder.FormatAccordionElement("Errors present",
                WebTemplateBuilder.FormatListOfStrings(err).ToString()));
        }

        public void AddMessage(string message)
        {
            msg.Append(WebTemplateBuilder.FormatParagraph(message));
        }

        public void AddFeature(FeatureData data)
        {
            var feature = new StringBuilder();
            feature.Append(WebTemplateBuilder.FormatParagraph(data.featureResult));
            feature.Append(WebTemplateBuilder.FormatListOfStrings(data.evidence));
            features.Append(WebTemplateBuilder.FormatAccordionElement(data.title, feature.ToString()));
        }

        private string BuildReport()
        {
            var report = new StringBuilder();
            if (features.Length != 0)
            {
                report.Append(introduction);
                report.Append(features.ToString());
            }
            if (errors.Length != 0)
            {
                report.Append(errors.ToString());
            }
            if (msg.Length != 0)
            {
                report.Append(msg);
            }

            return template.Replace(FEATURE_TAG,report.ToString());
        }

        public void WriteReport()
        {
            File.WriteAllText(OUTPUT_FILE, BuildReport());
        }
    }
}
