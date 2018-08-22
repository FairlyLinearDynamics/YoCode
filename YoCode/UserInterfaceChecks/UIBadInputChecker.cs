using System;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using OpenQA.Selenium.Support.UI;

namespace YoCode
{
    internal class UIBadInputChecker
    {
        private readonly IWebDriver browser;
        private const int TitleColumnFormatter = -40;
        private const int ValueColumnFormatter = -10;
        private List<bool> ratingsList = new List<bool>();

        public UIBadInputChecker(IWebDriver browser, string foundKeyWord)
        {
            UIBadInputCheckEvidence.Feature = Feature.BadInputCheck;

            this.browser = browser;

            UIKeywords.GARBAGE_INPUT.ToList().ForEach(a => InputCheckResult.Add(a,false));

            var uiInputhandler = new InputingToUI(browser, foundKeyWord);
            UIKeywords.GARBAGE_INPUT.ToList().ForEach(a=> {
                uiInputhandler.InputData(a);
                OutputCheck(a);
            });

            UIBadInputCheckEvidence.FeatureRating = GetOutputCheckRating();
            UIBadInputCheckEvidence.FeatureImplemented = !ratingsList.Contains(false) && ratingsList.Any();
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
                UIBadInputCheckEvidence.SetFailed(string.Format($"{x,TitleColumnFormatter} {false,ValueColumnFormatter}"));
                ratingsList.Add(false);
            }
            else
            {
                UIBadInputCheckEvidence.GiveEvidence(string.Format($"{x,TitleColumnFormatter} {true,ValueColumnFormatter}"));
                ratingsList.Add(true);
            }

            browser.Navigate().Back();
        }

        public FeatureEvidence UIBadInputCheckEvidence { get; set; } = new FeatureEvidence();
        public Dictionary<string, bool> InputCheckResult { get; set; } = new Dictionary<string, bool>();
    }
}
