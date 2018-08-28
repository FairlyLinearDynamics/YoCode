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

                    var xpath = featureKeyWord != null ? $"//*[@value=\"{featureKeyWord}\" and @type=\"submit\"]" : "//*[@type=\"submit\"]";
                    IWebElement elementToClick;

                    try
                    {
                        elementToClick = form.FindElement(By.XPath(xpath));
                    }
                    catch
                    {
                        elementToClick = form.FindElement(By.XPath("//*[@type=\"submit\"]"));
                    }

                    // Assume that there are 2 dropdown menues with conversion options
                    if (selectors.Count > 1)
                    {
                        try
                        {
                            var source = browser.FindElements(By.XPath($"//select/option[contains(text(),\"{featureKeyWord}\")]"));
                            source[0].Click();

                            // TODO: Check if kilometer is present in UI before using it
                            var target = browser.FindElements(By.XPath($"//select/option[contains(text(),\"Kilometer\")]"));
                            target[1].Click();
                        }
                        catch
                        { 
                            // TODO: Think of possible errs
                        }

                        foreach (var textField in form.FindElements(By.CssSelector("textarea")))
                        {
                            textField.Clear();
                            textField.SendKeys(applicantTestInput);
                        }

                        elementToClick.Click();
                    }
                    
                    // Assume that there is one dropdown menu with conversion options
                    else if (selectors.Count == 1)
                    {
                        SelectElement selectFromDropDown = new SelectElement(selectors.First());
                        selectFromDropDown.SelectByIndex(1);

                        foreach (var textField in form.FindElements(By.CssSelector("textarea")))
                        {
                            textField.Clear();
                            textField.SendKeys(applicantTestInput);
                        }

                        elementToClick.Click();
                    }

                    // No dropdown menu, only buttons
                    else
                    {
                        foreach (var textField in form.FindElements(By.CssSelector("textarea")))
                        {
                            textField.Clear();
                            textField.SendKeys(applicantTestInput);
                        }

                        elementToClick.Click();
                    }
                }
                catch { }
            }
        }
    }
}
