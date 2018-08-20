using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoCode
{
    class InputingToUI
    {
        IWebDriver browser;
        string startPage;

        public InputingToUI(IWebDriver browser, string startPage)
        {
            this.browser = browser;
            this.startPage = startPage;
        }

        public void InputData(string applicantTestInput)
        {
            var forms = browser.FindElements(By.CssSelector("form"));

            if (!forms.Any())
            {
                return;
            }
            foreach (var form in forms)
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
                            clicker.SelectByText(clicker.Options.Last(a => !a.Text.Equals(selectedElem)).Text);
                            selectedElem = clicker.SelectedOption.Text;
                        }

                        foreach (var textField in form.FindElements(By.CssSelector("textarea")))
                        {
                            textField.SendKeys(applicantTestInput);
                        }

                        form.FindElement(By.CssSelector("input")).Click();
                        //OutputCheck(applicantTestInput);
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
                        //OutputCheck(applicantTestInput);
                    }
                    else
                    {
                        foreach (var textField in form.FindElements(By.CssSelector("textarea")))
                        {
                            textField.SendKeys(applicantTestInput);
                        }

                        form.FindElements(By.XPath("//input[@*='Miles to kilometers']")).ToList().ForEach(a=>Console.WriteLine(a.GetAttribute("value")));
                        
                        //OutputCheck(applicantTestInput);
                    }
                }
                catch (Exception) { }
            }
        }
        private void OutputCheck(string testData)
        {
            var exception = browser.FindElements(By.XPath($"//*[contains(text(),{testData})]"));
            Console.WriteLine($"{exception.First(a => a.TagName.Equals("pre")).Text} with data: {testData}");
            if (exception.Any())
            {

            }
            else
            {

            }

            browser.Navigate().GoToUrl(startPage);
        }
    }
}
