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
        const string SCORE_TAG = "{SCORE}";

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
            errors.Append(WebElementBuilder.FormatAccordionElement(WebElementBuilder.FormaFeatureTitle("Errors present"),
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

            var featureTitle = WebElementBuilder.FormaFeatureTitle(data.title,data.featurePass);

            features.Append(WebElementBuilder.FormatAccordionElement(featureTitle, featureResults.ToString()));
        }

        public void AddBanner()
        {
            msg.Append(messages.HtmlFireplaceBanner);
        }

        private string BuildReport()
        {
            var report = new StringBuilder();
            if (features.Length == 0)
            {
                report.Append(errors.ToString());
                report.Append(msg);
                return messages.HtmlTemplate_WithoutFeatures.Replace(FEATURE_TAG, report.ToString());
            }
            report.Append(features.ToString());
            report.Append(errors.ToString());
            report.Append(msg);

            var score = 0.0;
            return messages.HtmlTemplate.Replace(FEATURE_TAG, report.ToString()).Replace(SCORE_TAG, score.ToString());
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
