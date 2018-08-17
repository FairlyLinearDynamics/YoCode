using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoCode
{
    class UIInputCheck
    {
        IWebDriver browser;
        List<bool> inputCheckResult = new List<bool>();

        public UIInputCheck(IWebDriver browser)
        {
            UIInputEvidence.FeatureTitle = "Correct convertion using user interface";
            this.browser = browser;
            var uiInputhandler = new InputingToUI(browser);
            UIKeywords.PROPER_INPUT.ToList().ForEach(a => {
                uiInputhandler.InputData(a);
                OutputCheck(a);
            });
        }

        private void OutputCheck(string testData)
        {
            var exception = browser.FindElements(By.XPath($"//*[contains(text(), '{Int32.Parse(testData)*1.609}')]"));
            var value = browser.FindElements(By.XPath("//*"));
            value.ToList().ForEach(Console.WriteLine);
            if (exception.Any())
            {
                UIInputEvidence.SetFailed($"Number \"{testData}\" converted successfully");
                inputCheckResult.Add(true);
            }
            else
            {
                UIInputEvidence.GiveEvidence($"Failed to convert \"{testData}\". Applicant's program returned: ");
                inputCheckResult.Add(true);
            }

            browser.Navigate().Back();
        }

        public FeatureEvidence UIInputEvidence { get; set; } = new FeatureEvidence();
    }
}
