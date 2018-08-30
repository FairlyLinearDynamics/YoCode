using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace YoCode
{
    internal class WebWriter : IPrint
    {
        private const string OUTPUT_PATH = @"YoCodeReport.html";
        private const string FEATURE_TAG = "{FEATURE}";
        private const string SCORE_TAG = "{SCORE}";

        private readonly StringBuilder features;
        private readonly StringBuilder errors;
        private readonly StringBuilder msg;

        private double score;
        private readonly bool generateHtml;
        private readonly bool openHtmlOnFinish;
        private readonly string outputTo;

        public WebWriter(bool generateHtml, bool openHtmlOnFinish, string outputTo)
        {
            features = new StringBuilder();
            errors = new StringBuilder();
            msg = new StringBuilder();
            this.generateHtml = generateHtml;
            this.openHtmlOnFinish = openHtmlOnFinish;
            this.outputTo = outputTo;
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
            const char dash = (char)0x2013;
            var featureTitle = WebElementBuilder.FormatFeatureTitle(data.title, data.featurePass, !data.featurePass.HasValue ? dash.ToString() : data.score.ToString() + "%");

            features.Append(WebElementBuilder.FormatAccordionElement(featureTitle, featureResults.ToString()));
        }

        public void AddBanner()
        {
            msg.Append(messages.HtmlFireplaceBanner);
        }

        public void AddFinalScore(double score)
        {
            this.score = score;
        }

        private string BuildReport()
        {
            if (features.Length == 0 && errors.Length == 0)
            {
                return messages.HtmlTemplate_HelpPage.Replace(FEATURE_TAG, msg.ToString());
            }

            return messages.HtmlTemplate.Replace(FEATURE_TAG, features.Append(msg).ToString()).Replace(SCORE_TAG, score + "%");
        }

        public void WriteReport()
        {
            var writeTo = (outputTo != null) ? Path.Combine(outputTo, OUTPUT_PATH) : OUTPUT_PATH;

            var consoleWriter = new ConsoleWriter();
            try
            {
                if (generateHtml)
                {
                    File.WriteAllText(writeTo, BuildReport());
                    if (openHtmlOnFinish)
                    {
                        HtmlReportLauncher.LaunchReport(writeTo);
                    }
                    consoleWriter.AddMessage(String.Format(messages.SuccessfullyWroteReport, Environment.NewLine, Path.GetFullPath(writeTo)));
                    consoleWriter.WriteReport();
                }
            }
            catch
            {
                consoleWriter.PrintErrors(new List<string>() { String.Format(messages.WrongWritePermission, Path.GetFullPath(writeTo), Environment.NewLine, Environment.NewLine) });
            }
        }
    }
}
