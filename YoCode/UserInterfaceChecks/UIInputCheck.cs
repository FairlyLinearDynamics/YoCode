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
        private const int TitleColumnFormatter = -40;
        private const int ValueColumnFormatter = -10;

        private static double unitConvertValue = 1.60934;

        public UIInputCheck(IWebDriver browser, string foundKeyWord)
        {
            UIInputEvidence.FeatureTitle = "Correct convertion using user interface";

            this.browser = browser;

            var uiInputhandler = new InputingToUI(browser, foundKeyWord);
            foreach(var key in UIKeywords.PROPER_INPUT)
            {
                uiInputhandler.InputData(key);
                try
                {
                    browser.FindElement(By.XPath($"//*[contains(text(),{Double.Parse(key) * unitConvertValue})]"));
                }
                catch (NoSuchElementException)
                {
                    UIInputEvidence.SetFailed("Values were converted incorrectly");
                    browser.Navigate().Back();
                    return;
                }
                browser.Navigate().Back();
            }

            UIInputEvidence.FeatureImplemented = true;
            UIInputEvidence.GiveEvidence("Successfully converted from miles to kilometres");
        }

        public FeatureEvidence UIInputEvidence { get; set; } = new FeatureEvidence();
    }
}
