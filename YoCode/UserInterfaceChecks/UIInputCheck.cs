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
            var uiInputhandler = new InputingToUI(browser, browser.Url);
            UIKeywords.PROPER_INPUT.ToList().ForEach(a => {
                uiInputhandler.InputData(a);
            });
        }

        public FeatureEvidence UIInputEvidence { get; set; } = new FeatureEvidence();
    }
}
