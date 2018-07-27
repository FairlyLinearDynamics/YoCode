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

        IWebElement tagOfInterest;

        public FrontEndCheck()
        {
            FrontEndEvidence.FeatureTitle = "Front End Contains new feature and works correctly";

            var foxService = FirefoxDriverService.CreateDefaultService(@"C:\Users\ukekar\source\repos\YoCode\YoCode\bin\Debug\netcoreapp2.1\");
            foxService.HideCommandPromptWindow = true;
            browser = new FirefoxDriver(foxService,new FirefoxOptions());
            browser.Navigate().GoToUrl("http://localhost:5000/");
            FrontEndEvidence.FeatureImplemented = CheckIfUIContainsFeature();
            InputData();
            OutputCheck();
            
            //FrontEndEvidence.GiveEvidence(InputData());
            browser.Close();
        }

        private bool CheckIfUIContainsFeature()
        {
            //var 
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
                tagOfInterest = tag;

                var tagText = tag.Text;
                if (tagText.Equals(keyWord))
                {
                    return true;
                }
            }
            return false;

        }

        private void OutputCheck()
        { 
            // TODO: Check for thrown exceptions
            var exception = browser.FindElements(By.XPath("//*[contains(text(), 'An unhandled exception occurred ')]"));
            if (exception.Any())
            {
                FrontEndEvidence.SetFailed("Exception not handled");
            }
            else
            {
                FrontEndEvidence.GiveEvidence("No exceptions found");
            }

            // TODO: Check if output is number
        }

        private void InputData()
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
                        SelectElement clicker;
                        foreach (var select in selectors)
                        {
                            clicker = new SelectElement(select);
                            clicker.SelectByText(clicker.SelectedOption.Text.Contains("Yard") ? "Meter" : "Yard");
                        }

                        // TODO: Find and enter something in textfield
                        var textFields = form.FindElements(By.CssSelector("textarea"));
                        foreach (var textField in textFields)
                        {
                            textField.SendKeys("");
                        }

                        // TODO: Submit form
                        form.FindElement(By.CssSelector("input")).Click();
                    }
                    else if (selectors.Count == 1)
                    {
                        // Only one drop down, check if it has Yards And Meters
                        // TODO: Select proper option
                        SelectElement selectFromDropDown = new SelectElement(selectors.First());
                        selectFromDropDown.SelectByIndex(1);

                        // TODO: Find and enter something in textfield
                        var textFields = form.FindElements(By.CssSelector("textarea"));
                        foreach (var textField in textFields)
                        {
                            textField.SendKeys("");
                        }

                        // TODO: Submit form
                        form.FindElement(By.CssSelector("input")).Click();
                    }
                    else
                    {
                        // No dropdown, check for other type of input
                        var textFields = form.FindElements(By.CssSelector("textarea"));
                        foreach (var textField in textFields)
                        {
                            textField.SendKeys("");
                        }

                        form.FindElement(By.CssSelector("input")).Click();
                    }
                }
                catch (Exception) { }
            }

        }

        public FeatureEvidence FrontEndEvidence { get; private set; } = new FeatureEvidence();

    }
}