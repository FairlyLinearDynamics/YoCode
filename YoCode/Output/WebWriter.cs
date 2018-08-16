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

        public WebWriter()
        {
            features = new StringBuilder();
            errors = new StringBuilder();
            msg = new StringBuilder();
        }

        public void AddErrs(IEnumerable<string> errs)
        {
            /* HTML does not support error output */
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

            var featureTitle = WebElementBuilder.FormaFeatureTitle(data.title,data.featurePass);

            features.Append(WebElementBuilder.FormatAccordionElement(featureTitle, featureResults.ToString()));
        }

        public void AddBanner()
        {
            msg.Append(messages.HtmlFireplaceBanner);
        }

        private string BuildReport()
        {
            if (features.Length == 0 && errors.Length == 0)
            {
                return messages.HtmlTemplate_HelpPage.Replace(FEATURE_TAG, msg.ToString());
            }

            else
            {
                return messages.HtmlTemplate.Replace(FEATURE_TAG, features.Append(msg).ToString());
            }
        }

        public void WriteReport()
        {
            File.WriteAllText(OUTPUT_PATH, BuildReport());
            if (Program.OpenHTMLOnFinish)
            {
                HtmlReportLauncher.LaunchReport(OUTPUT_PATH);
            }
        }
    }
}
