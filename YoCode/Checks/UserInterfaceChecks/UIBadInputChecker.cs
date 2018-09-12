using System;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoCode
{
    internal class UIBadInputChecker
    {
        private readonly IWebDriver browser;
        private const int TitleColumnFormatter = -40;
        private const int ValueColumnFormatter = -10;
        private List<bool> ratingsList = new List<bool>();
        private StringBuilder resultsOutput = new StringBuilder();

        public UIBadInputChecker(IWebDriver browser, UIFoundTags foundKeyWord)
        {
            UIBadInputCheckEvidence.Feature = Feature.UIBadInputCheck;
            UIBadInputCheckEvidence.HelperMessage = messages.UIBadInputCheck;

            this.browser = browser;

            UIKeywords.GARBAGE_INPUT.ToList().ForEach(a => InputCheckResult.Add(a, false));

            var uiInputhandler = new InputingToUI(browser, foundKeyWord);

            resultsOutput.AppendLine(string.Format($"\n{"Input name",TitleColumnFormatter} {"FIXED",ValueColumnFormatter}"));
            resultsOutput.AppendLine(messages.ParagraphDivider);

            foreach (var key in UIKeywords.GARBAGE_INPUT)
            {
                var errs = uiInputhandler.InputData(key);
                if (errs.Any())
                {
                    SetCheckUndefined(errs);
                    return;
                }

                OutputCheck(key);
            }
            UIBadInputCheckEvidence.FeatureRating = GetOutputCheckRating();

            var featureImplemented = !ratingsList.Contains(false) && ratingsList.Any();
            if (featureImplemented)
            {
                UIBadInputCheckEvidence.SetPassed(new SimpleEvidenceBuilder(resultsOutput.ToString()));
            }
            else
            {
                UIBadInputCheckEvidence.SetFailed(new SimpleEvidenceBuilder(resultsOutput.ToString()));
            }
        }

        private void SetCheckUndefined(List<UICheckErrEnum> errs)
        {
            UIBadInputCheckEvidence.SetInconclusive(new SimpleEvidenceBuilder(UIEnumErrFormat.ConvertEnum(errs).ToList()));
        }

        public double GetOutputCheckRating()
        {
            return HelperMethods.GetRatingFromBoolList(ratingsList);
        }

        private void OutputCheck(string testData)
        {
            var exception = browser.FindElements(By.XPath("//*[contains(text(), 'An unhandled exception occurred')]"));
            var x = $"\"{testData.Replace(Environment.NewLine, "(New line here)")}\"";

            if (exception.Any())
            {
                resultsOutput.AppendLine(string.Format($"{x,TitleColumnFormatter} {false,ValueColumnFormatter}"));
                ratingsList.Add(false);
            }
            else
            {
                resultsOutput.AppendLine(string.Format($"{x,TitleColumnFormatter} {true,ValueColumnFormatter}"));
                ratingsList.Add(true);
            }

            browser.Navigate().Back();
        }

        public FeatureEvidence UIBadInputCheckEvidence { get; set; } = new FeatureEvidence();
        public Dictionary<string, bool> InputCheckResult { get; set; } = new Dictionary<string, bool>();
    }
}
