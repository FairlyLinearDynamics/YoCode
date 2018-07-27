using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace YoCode
{
    public class WebWriter : IPrint
    {
        const string OUTPUT_PATH = @"YoCodeReport.html";
        const string FEATURE_TAG = "{FEATURE}";

        StringBuilder features;
        StringBuilder errors;
        StringBuilder msg;
        string introduction;

        public WebWriter()
        {
            features = new StringBuilder();
            errors = new StringBuilder();
            msg = new StringBuilder();
        }

        public void AddIntro(string text)
        {
            introduction = WebElementBuilder.FormatParagraph(text);
        }

        public void AddErrs(IEnumerable<string> errs)
        {
            errors.Append(WebElementBuilder.FormatAccordionElement("Errors present",
                WebElementBuilder.FormatListOfStrings(errs)));
        }

        public void AddMessage(string message)
        {
            msg.Append(WebElementBuilder.FormatParagraph(message));
        }

        public void AddFeature(FeatureData data)
        {
            var featureResults = new StringBuilder();
            featureResults.Append(WebElementBuilder.FormatParagraph(data.featureResult));
            featureResults.Append(WebElementBuilder.FormatListOfStrings(data.evidence));

            var featureTitle = data.title + WebElementBuilder.FormatCheckIcont(data.featurePass);

            features.Append(WebElementBuilder.FormatAccordionElement(featureTitle, featureResults.ToString()));
        }

        private string BuildReport()
        {
            var report = new StringBuilder();
            report.Append(introduction);
            report.Append(features.ToString());
            report.Append(errors.ToString());
            report.Append(msg);

            return messages.HtmlTemplate.Replace(FEATURE_TAG,report.ToString());
        }

        public void WriteReport()
        {
            File.WriteAllText(OUTPUT_PATH, BuildReport());
        }
    }
}
