﻿using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.IO;
using Microsoft.Win32;
using System.Collections.Generic;

namespace YoCode
{
    internal class FrontEndCheck
    {
        private static IWebDriver browser;
        private readonly string port;
        private const string CHROME = "Google Chrome";
        private const string FIREFOX = "Firefox";
        private List<bool> ratingsList = new List<bool>();

        public static bool running;

        public FrontEndCheck(string applicantsWebPort, string[] keyWords)
        {
            if (string.IsNullOrEmpty(applicantsWebPort))
            {
                FrontEndEvidence.SetFailed("Could not retrieve the port number. Another program might be using it.");
                return;
            }
            running = true;
            FrontEndEvidence.FeatureTitle = "New feature found in front-end implementation";
            FrontEndEvidence.Feature = Feature.FrontEndCheck;
            DriverService service;

            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet");
            var browsers = key.GetSubKeyNames();
            
            if (browsers.Any(a=>a.Contains(FIREFOX)))
            {
                try
                {
                    FrontEndEvidence.GiveEvidence(string.Format("\n{0,-40} {1,-10}", "Input name", "FIXED"));
                    FrontEndEvidence.GiveEvidence(messages.ParagraphDivider);
                    service = FirefoxDriverService.CreateDefaultService(Directory.GetCurrentDirectory());
                    service.HideCommandPromptWindow = true;
                    port = applicantsWebPort;
                    var options = new FirefoxOptions();
                    options.AddArgument("--headless");
                    browser = new FirefoxDriver((FirefoxDriverService)service, options);
                }
                catch { };
            }
            else if (browsers.Any(a=>a.Contains(CHROME)))
            {
                try
                {
                    FrontEndEvidence.GiveEvidence(string.Format("\n{0,-40} {1,-10}", "Input name", "FIXED"));
                    FrontEndEvidence.GiveEvidence(messages.ParagraphDivider);
                    service = ChromeDriverService.CreateDefaultService(Directory.GetCurrentDirectory());
                    service.HideCommandPromptWindow = true;
                    port = applicantsWebPort;
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("--headless");

                    browser = new ChromeDriver((ChromeDriverService)service, chromeOptions);
                }
                catch { };
            }
            else
            {
                FrontEndEvidence.SetFailed($"Could not execute check: Did not find needed browser{Environment.NewLine}Please install Google Chrome or Mozilla Firefox internet browser");
                return;
            }

            try
            {
                browser.Navigate().GoToUrl(port);

                UIKeywords.GARBAGE_INPUT.ToList().ForEach(InputData);
                FrontEndEvidence.FeatureRating = GetOutputCheckRating();
                FrontEndEvidence.FeatureImplemented = !ratingsList.Contains(false);
            }
            catch { return; }

            if (!FrontEndEvidence.Evidence.Any())
            {
                FrontEndEvidence.GiveEvidence("Could not input any data");
            }


            CloseBrowser();
        }

        public static bool CloseBrowser()
        {
            if (browser != null)
            {
                browser.Dispose();
                browser.Quit();
                running = false;
                return true;
            }
            if(running)
            {
                return false;
            }

            return true;
        }

        private bool CheckIfUIContainsFeature(string[] keyWords)
        {
            foreach(HtmlTags tag in Enum.GetValues(typeof(HtmlTags)))
            {
                if (SearchForElement(tag, keyWords))
                {
                    return true;
                }
            }
            return false;
        }

        private bool SearchForElement(HtmlTags htmlTag, string[] keyWords)
        {
            foreach (var tag in browser.FindElements(By.CssSelector(htmlTag.ToString())))
            {
                if (keyWords.Any(a => tag.Text.Equals(a, StringComparison.OrdinalIgnoreCase)))
                {
                    return true;
                }
            }
            return false;
        }

        private void OutputCheck(string testData)
        {
            var exception = browser.FindElements(By.XPath("//*[contains(text(), 'An unhandled exception occurred')]"));
            if (exception.Any())
            {
                var x = $"\"{testData.Replace(Environment.NewLine, "(New line here)")}\"";
                FrontEndEvidence.SetFailed(string.Format("{0,-40} {1,-10}", x, false));
                ratingsList.Add(false);
            }
            else
            {
                var x = $"\"{testData.Replace(Environment.NewLine, "(New line here)")}\"";
                FrontEndEvidence.GiveEvidence(string.Format("{0,-40} {1,-10}", x, true));
                ratingsList.Add(true);
            }

            browser.Navigate().GoToUrl(port);
        }

        public double GetOutputCheckRating()
        {
            double sum = 0;

            foreach(var elem in ratingsList)
            {
                if(elem)
                {
                    var temp = 1 / Convert.ToDouble(ratingsList.Count);
                    sum += temp;
                }
            }
            return sum;
        }

        private void InputData(string applicantTestInput)
        {
            var forms = browser.FindElements(By.CssSelector("form"));

            if (!forms.Any())
            {
                return;
            }
            foreach(var form in forms)
            {
                try
                {
                    var selectors = form.FindElements(By.CssSelector("select"));
                    if (selectors.Count > 1)
                    {
                        string selectedElem = null;

                        foreach (var select in selectors)
                        {
                            SelectElement clicker = new SelectElement(select);
                            clicker.SelectByText(clicker.Options.Last(a=>!a.Text.Equals(selectedElem)).Text);
                            selectedElem = clicker.SelectedOption.Text;
                        }

                        foreach (var textField in form.FindElements(By.CssSelector("textarea")))
                        {
                            textField.SendKeys(applicantTestInput);
                        }

                        form.FindElement(By.CssSelector("input")).Click();
                        OutputCheck(applicantTestInput);
                    }
                    else if (selectors.Count == 1)
                    {
                        SelectElement selectFromDropDown = new SelectElement(selectors.First());
                        selectFromDropDown.SelectByIndex(1);

                        foreach (var textField in form.FindElements(By.CssSelector("textarea")))
                        {
                            textField.SendKeys(applicantTestInput);
                        }

                        form.FindElement(By.CssSelector("input")).Click();
                        OutputCheck(applicantTestInput);
                    }
                    else
                    {
                        foreach (var textField in form.FindElements(By.CssSelector("textarea")))
                        {
                            textField.SendKeys(applicantTestInput);
                        }

                        form.FindElement(By.CssSelector("input")).Click();
                        OutputCheck(applicantTestInput);
                    }
                }
                catch (Exception) { }
            }
        }

        public FeatureEvidence FrontEndEvidence { get; } = new FeatureEvidence();
    }
}