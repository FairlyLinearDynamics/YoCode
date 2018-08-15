using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.IO;

namespace YoCode
{
    internal class FrontEndCheck
    {
        private readonly IWebDriver browser;
        private readonly string port;

        public FrontEndCheck(string applicantsWebPort, string[] keyWords)
        {
            FrontEndEvidence.FeatureTitle = "New feature found in front-end implementation";

            if (String.IsNullOrEmpty(applicantsWebPort))
            {
                FrontEndEvidence.SetFailed("Could not retrieve the port number. Another program might be using it.");
                return;
            }

            try
            {
                try
                {
                    var foxService = FirefoxDriverService.CreateDefaultService(Directory.GetCurrentDirectory());
                    foxService.HideCommandPromptWindow = true;
                    port = applicantsWebPort;
                    var options = new FirefoxOptions();
                    options.AddArgument("--headless");

                    browser = new FirefoxDriver(foxService, options);
                }
                catch (Exception)
                {
                    var chromeService = ChromeDriverService.CreateDefaultService(Directory.GetCurrentDirectory());
                    chromeService.HideCommandPromptWindow = true;
                    port = applicantsWebPort;
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("--headless");

                    browser = new ChromeDriver(chromeService, chromeOptions);
                }

                browser.Navigate().GoToUrl(port);

                FrontEndEvidence.FeatureImplemented = CheckIfUIContainsFeature(keyWords);

                UIKeywords.GARBAGE_INPUT.ToList().ForEach(InputData);

                if (!FrontEndEvidence.Evidence.Any())
                {
                    FrontEndEvidence.GiveEvidence("Could not input any data");
                }

                browser.Dispose();
            }
            catch (WebDriverException e)
            {
                browser.Dispose();

                FrontEndEvidence.SetFailed($"Check could not be executed due to exception: \"{e.Message}\"");
            }
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
                FrontEndEvidence.SetFailed($"Exception with \"{testData.Replace(Environment.NewLine, "(New line here)")}\" input not handled");
            }
            else
            {
                FrontEndEvidence.GiveEvidence($"No exceptions found with \"{testData.Replace(Environment.NewLine, "(New line here)")}\" input");
            }

            browser.Navigate().GoToUrl(port);
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