using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace YoCode
{
    class UICheck 
    {
        private readonly IWebDriver browser;

        public UICheck(string port)
        {
            try
            {
                try
                {
                    var foxService = FirefoxDriverService.CreateDefaultService(Directory.GetCurrentDirectory());
                    foxService.HideCommandPromptWindow = true;
                    var options = new FirefoxOptions();
                    options.AddArgument("--headless");

                    browser = new FirefoxDriver(foxService, options);
                }
                catch (Exception)
                {
                    var chromeService = ChromeDriverService.CreateDefaultService(Directory.GetCurrentDirectory());
                    chromeService.HideCommandPromptWindow = true;
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("--headless");

                    browser = new ChromeDriver(chromeService, chromeOptions);
                }

                browser.Navigate().GoToUrl(port);
            }
            finally
            {
                //Console.WriteLine(browser.FindElement(By.CssSelector("input")).GetAttribute("value"));
                UIFeatureEvidences.Add(new FeaturePresentInUI(browser, UIKeywords.UNIT_KEYWORDS).FeatureInUIEvidence);
                UIFeatureEvidences.Add(new UIBadInputChecker(browser).UIBadInputEvidence);
                UIFeatureEvidences.Add(new UIInputCheck(browser).UIInputEvidence);
            }
        }

        public List<FeatureEvidence> UIFeatureEvidences { get; } = new List<FeatureEvidence>();
    }
}
