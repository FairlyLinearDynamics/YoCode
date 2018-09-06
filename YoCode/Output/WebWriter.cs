using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace YoCode
{
    internal class WebWriter : IPrint
    {
        private const string FEATURE_TAG = "{FEATURE}";
        private const string SCORE_TAG = "{SCORE}";
        private const string TEST_TYPE = "{TEST-TYPE}";

        private const string FILECHANGESPAN_OPEN = "<span class=\"changedFileText\">";
        private const string FILECHANGESPAN_CLOSE = "</span>";

        private readonly StringBuilder features;
        private readonly StringBuilder errors;
        private readonly StringBuilder msg;

        private double totalScore;
        private bool isJunior;
        private readonly string nameOfReportFile;

        public WebWriter(string outputPath)
        {
            features = new StringBuilder();
            errors = new StringBuilder();
            msg = new StringBuilder();
            nameOfReportFile = outputPath;
        }

        public void AddMessage(string message)
        {
            msg.Append(WebElementBuilder.FormatAndEncapsulateParagraph(message));
        }

        public void AddFeature(FeatureData data)
        {
            var featureResults = new StringBuilder();
            featureResults.Append(WebElementBuilder.FormatParagraph(data.featureResult));
            featureResults.Append(data.featureEvidence.BuildEvidenceForHTML());
            string featureTitle;
            switch (data.featurePass)
            {
                case true:
                    featureTitle = WebElementBuilder.FormatPassedFeatureTitle(new WebTitleElements()
                    {
                        score = data.score + "%",
                        rawScore = data.rawScore + "%",
                        weighting = data.weighting.ToString(),
                        title = data.title,
                    });
                    break;

                case false:
                    featureTitle = WebElementBuilder.FormatFailedFeatureTitle(new WebTitleElements()
                    {
                        score = data.score + "%",
                        rawScore = data.rawScore + "%",
                        weighting = data.weighting.ToString(),
                        title = data.title,
                    });
                    break;

                default:
                    const char dash = (char)0x2013;
                    featureTitle = WebElementBuilder.FormatInconclusiveFeatureTitle(new WebTitleElements()
                    {
                        score = dash.ToString(),
                        rawScore = dash.ToString(),
                        weighting = dash.ToString(),
                        title = data.title,
                    });
                    break;
            }

            features.Append(WebElementBuilder.FormatAccordionElement(new WebAccordionData()
            {
                featureTitle = featureTitle,
                content = featureResults.ToString(),
                helperMessage = data.featureHelperMessage,
            }));
        }

        public void AddBanner()
        {
            msg.Append(messages.HtmlFireplaceBanner);
        }

        public void AddFinalScore(double score)
        {
            totalScore = score;
        }

        private string BuildReport()
        {
            if (features.Length == 0 && errors.Length == 0)
            {
                return messages.HtmlTemplate_HelpPage.Replace(FEATURE_TAG, msg.ToString());
            }

            return messages.HtmlTemplate
                .Replace(FEATURE_TAG, features.Append(msg).ToString())
                .Replace(SCORE_TAG, totalScore + "%")
                .Replace(TEST_TYPE, isJunior?"Junior":"Original");
        }

        public void WriteReport()
        {
            var consoleWriter = new ConsoleWriter();
            try
            {
                File.WriteAllText(nameOfReportFile, BuildReport());
                consoleWriter.AddMessage(String.Format(messages.SuccessfullyWroteReport, Environment.NewLine, Path.GetFullPath(nameOfReportFile)));
                consoleWriter.WriteReport();
            }
            catch
            {
                consoleWriter.PrintErrors(new List<string>() { String.Format(messages.WrongWritePermission, Path.GetFullPath(nameOfReportFile), Environment.NewLine, Environment.NewLine) });
            }
        }

        public void AddTestType(bool isJunior)
        {
            this.isJunior = isJunior;
        }
    }
}
