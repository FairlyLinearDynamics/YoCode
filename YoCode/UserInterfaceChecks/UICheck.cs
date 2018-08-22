using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using Microsoft.Win32;

namespace YoCode
{
    class UICheck 
    {
        private static IWebDriver browser;
        private const string CHROME = "Google Chrome";
        private const string FIREFOX = "Firefox";

        public static bool Running { get; private set; }

        public UICheck(string port)
        {

            if (string.IsNullOrEmpty(port))
            {
                UIFeatureEvidences.ForEach(a => a.SetFailed("Could not retrieve the port number. Another program might be using it."));
                return;
            }

            Running = true;

            DriverService service;

            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet");
            var browsers = key.GetSubKeyNames();

            if (browsers.Any(a => a.Contains(FIREFOX)))
            {
                try
                {
                    service = FirefoxDriverService.CreateDefaultService(Directory.GetCurrentDirectory());
                    service.HideCommandPromptWindow = true;
                    var options = new FirefoxOptions();
                    options.AddArgument("--headless");
                    browser = new FirefoxDriver((FirefoxDriverService)service, options);
                }
                catch { };
            }
            else if (browsers.Any(a => a.Contains(CHROME)))
            {
                try
                {
                    service = ChromeDriverService.CreateDefaultService(Directory.GetCurrentDirectory());
                    service.HideCommandPromptWindow = true;
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("--headless");

                    browser = new ChromeDriver((ChromeDriverService)service, chromeOptions);
                }
                catch { };
            }
            else
            {
                UIFeatureEvidences.ForEach(a=>a.SetFailed($"Could not execute check: Did not find needed browser{Environment.NewLine}Please install Google Chrome or Mozilla Firefox internet browser"));
            }

            try
            {
                browser.Navigate().GoToUrl(port);

                var featureInUI = new UIFeatureImplemented(browser, UIKeywords.UNIT_KEYWORDS);
                UIFeatureEvidences[0] = featureInUI.UIFeatureImplementedEvidence;

                UIFeatureEvidences[1] = new UIBadInputChecker(browser, featureInUI.FeatureTagFound).UIBadInputCheckEvidence;

                UIFeatureEvidences[2] = new UIConversionCheck(browser, featureInUI.FeatureTagFound).UIConversionEvidence;

                CloseBrowser();
            }
            catch { return; }

        }

        public static bool CloseBrowser()
        {
            if (browser != null)
            {
                browser.Dispose();
                browser.Quit();
                Running = false;
                return true;
            }

            return !Running;
        }

        public List<FeatureEvidence> UIFeatureEvidences { get; } = new List<FeatureEvidence>()
        {
            new FeatureEvidence(){ FeatureTitle = "Found feature evidence in user interface", Feature = Feature.UIFeatureImplmeneted },
            new FeatureEvidence(){ FeatureTitle = "Bad input crashes have been fixed in the UI", Feature = Feature.UIBadInputCheck},
            new FeatureEvidence(){ FeatureTitle = "Units were converted successfully using UI", Feature = Feature.UIConversionCheck}
        };
    }
}
