using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace YoCode
{
    class FrontEndCheck
    {
        IWebDriver browser;
        string port;

        public FrontEndCheck(string port)
        {
            FrontEndEvidence.FeatureTitle = "New feature found in front-end implementation";
            var foxService = FirefoxDriverService.CreateDefaultService(@"C:\Users\ukekar\source\repos\YoCode\YoCode\bin\Debug\netcoreapp2.1\");
            foxService.HideCommandPromptWindow = true;
            this.port = port;
            try
            {
                browser = new FirefoxDriver(foxService, new FirefoxOptions());
                browser.Navigate().GoToUrl("");

                FrontEndEvidence.FeatureImplemented = CheckIfUIContainsFeature();

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

                browser.Dispose();
            }
            catch (WebDriverException)
            {
                browser.Dispose();

                FrontEndEvidence.SetFailed("Check could not be executed");
            }
        }

        private bool CheckIfUIContainsFeature()
        {
            foreach(HtmlTags tag in Enum.GetValues(typeof(HtmlTags)))
            {
                if (SearchForElement(tag, "Yard"))
                {
                    return true;
                }
            }
            return false;
        }

        private bool SearchForElement(HtmlTags htmlTag, string keyWord)
        {
            var tags = browser.FindElements(By.CssSelector(htmlTag.ToString()));
            foreach (var tag in tags)
            {
                var tagText = tag.Text;
                if (tagText.Equals(keyWord))
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
                FrontEndEvidence.SetFailed($"Exception with \"{testData}\" input not handled");
            }
            else
            {
                FrontEndEvidence.GiveEvidence($"No exceptions found with \"{testData}\" input");

            }

            browser.Navigate().GoToUrl(port);
            // TODO: Check if output is number
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