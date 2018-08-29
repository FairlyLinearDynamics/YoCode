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
            UIFeatureEvidences = ExecuteChecks(port);
        }

        private static List<FeatureEvidence> ExecuteChecks(string port)
        {
            if (string.IsNullOrEmpty(port))
            {
                return CreateFailureEvidence("Could not retrieve the port number. Another program might be using it.");
            }

            if (!OpenBrowser())
            {
                return CreateFailureEvidence($"Could not execute check: Did not find needed browser{Environment.NewLine}" +
                    $"Please install Google Chrome or Mozilla Firefox internet browser");
            }

            try
            {
                browser.Navigate().GoToUrl(port);

                var featureInUI = new UIFeatureImplemented(browser);

                return new List<FeatureEvidence>
                {
                    featureInUI.UIFeatureImplementedEvidence,
                    new UIBadInputChecker(browser, featureInUI.FoundTagsInfo).UIBadInputCheckEvidence,
                    new UIConversionCheck(browser, featureInUI.FoundTagsInfo).UIConversionEvidence
                };
            }
            catch
            {
                return CreateFailureEvidence("Unexpected closure of application, could not interact with browser.");
            }
            finally
            {
                CloseBrowser();
            }
        }

        private static bool OpenBrowser()
        {
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
                    return true;
                }
                catch
                {
                    CloseBrowser();
                    return false;
                }
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
                    return true;
                }
                catch
                {
                    CloseBrowser();
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private static List<FeatureEvidence> CreateDefaultFeatureEvidence()
        {
            return new List<FeatureEvidence> {
                new FeatureEvidence(){ FeatureTitle = "Found feature evidence in user interface", Feature = Feature.UIFeatureImplemeneted },
                new FeatureEvidence(){ FeatureTitle = "Bad input crashes have been fixed in the UI", Feature = Feature.UIBadInputCheck},
                new FeatureEvidence(){ FeatureTitle = "Units were converted successfully using UI", Feature = Feature.UIConversionCheck}
            };
        }

        private static List<FeatureEvidence> CreateFailureEvidence(string failureReason)
        {
            var evidence = CreateDefaultFeatureEvidence();
            evidence.ForEach(a => a.SetInconclusive(failureReason));
            return evidence;
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

        public List<FeatureEvidence> UIFeatureEvidences { get; }
    }
}
