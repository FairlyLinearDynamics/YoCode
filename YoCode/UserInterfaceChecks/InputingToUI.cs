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
        string featureKeyWord;

        public InputingToUI(IWebDriver browser, string featureKeyWord = null)
        {
            this.browser = browser;
            this.featureKeyWord = featureKeyWord;
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

                    var xpath = featureKeyWord != null ? $"//input[@value=\"{featureKeyWord}\"]" : "//input[@*]";
                    var elementToClick = form.FindElement(By.XPath(xpath));

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

                        elementToClick.Click();
                    }
                    else if (selectors.Count == 1)
                    {
                        SelectElement selectFromDropDown = new SelectElement(selectors.First());
                        selectFromDropDown.SelectByIndex(1);

                        foreach (var textField in form.FindElements(By.CssSelector("textarea")))
                        {
                            textField.SendKeys(applicantTestInput);
                        }

                        elementToClick.Click();
                    }
                    else
                    {
                        foreach (var textField in form.FindElements(By.CssSelector("textarea")))
                        {
                            textField.SendKeys(applicantTestInput);
                        }

                        elementToClick.Click();
                    }
                }
                catch (Exception) {
                }
            }
        }
    }
}
