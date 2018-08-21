using System;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using OpenQA.Selenium.Support.UI;

namespace YoCode
{
    class UIBadInputChecker
    {
        IWebDriver browser;
        List<bool> inputCheckResult = new List<bool>();

        public UIBadInputChecker(IWebDriver browser)
        {
            UIBadInputEvidence.FeatureTitle = "Applicant handled bad input cases in user interface";
            this.browser = browser;
            var uiInputhandler = new InputingToUI(browser, browser.Url);
            UIKeywords.GARBAGE_INPUT.ToList().ForEach(a=> {
                uiInputhandler.InputData(a);
                OutputCheck(a);
            });
        }

        private void OutputCheck(string testData)
        {
            var exception = browser.FindElements(By.XPath("//*[contains(text(), 'An unhandled exception occurred')]"));
            if (exception.Any())
            {
                UIBadInputEvidence.SetFailed($"Exception with \"{testData.Replace(Environment.NewLine, "(New line here)")}\" input not handled");
                inputCheckResult.Add(false);
            }
            else
            {
                UIBadInputEvidence.GiveEvidence($"No exceptions found with \"{testData.Replace(Environment.NewLine, "(New line here)")}\" input");
                inputCheckResult.Add(true);
            }

            browser.Navigate().Back();
        }



        public FeatureEvidence UIBadInputEvidence { get; set; } = new FeatureEvidence();
    }
}
