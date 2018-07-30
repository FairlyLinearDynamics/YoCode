using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.IO;

namespace YoCode
{
    class FrontEndCheck
    {
        IWebDriver browser;
        string port;

        public FrontEndCheck(string applicantsWebPort, string[] keyWords)
        {
            FrontEndEvidence.FeatureTitle = "New feature found in front-end implementation";
            var foxService = FirefoxDriverService.CreateDefaultService(Directory.GetCurrentDirectory());
            foxService.HideCommandPromptWindow = true;
            port = applicantsWebPort;

            try
            {
                browser = new FirefoxDriver(foxService, new FirefoxOptions());
                browser.Navigate().GoToUrl(port);

                FrontEndEvidence.FeatureImplemented = CheckIfUIContainsFeature(keyWords);

                var testInput = new List<string>()
                {
                    "",
                    Environment.NewLine,
                    $"{Environment.NewLine}5",
                    $"5{Environment.NewLine}{Environment.NewLine}5",
                    $"5{Environment.NewLine}{Environment.NewLine}",
                    "a b c"
                };

                testInput.ForEach(a => InputData(a));

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
            var tags = browser.FindElements(By.CssSelector(htmlTag.ToString()));
            foreach (var tag in tags)
            {
                if (keyWords.Any(a => tag.Text.ToLower().Equals(a.ToLower())))
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
                            clicker.SelectByText((clicker.Options.Where(a=>!a.Text.Equals(selectedElem))).Last().Text);
                            selectedElem = clicker.SelectedOption.Text;
                        }

                        var textFields = form.FindElements(By.CssSelector("textarea"));
                        foreach (var textField in textFields)
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

                        var textFields = form.FindElements(By.CssSelector("textarea"));
                        foreach (var textField in textFields)
                        {
                            textField.SendKeys(applicantTestInput);
                        }

                        form.FindElement(By.CssSelector("input")).Click();
                        OutputCheck(applicantTestInput);
                    }
                    else
                    {
                        var textFields = form.FindElements(By.CssSelector("textarea"));
                        foreach (var textField in textFields)
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

        public FeatureEvidence FrontEndEvidence { get; private set; } = new FeatureEvidence();

    }
}