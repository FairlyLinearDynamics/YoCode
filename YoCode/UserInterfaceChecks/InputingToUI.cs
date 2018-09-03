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
        UIFoundTags foundTagsInfo;

        public InputingToUI(IWebDriver browser, UIFoundTags foundTagsInfo)
        {
            this.browser = browser;
            this.foundTagsInfo = foundTagsInfo;
        }

        public List<UICheckErrEnum> InputData(string applicantTestInput)
        {
            var forms = browser.FindElements(By.CssSelector("form"));
            List<UICheckErrEnum> errs = new List<UICheckErrEnum>();

            if (!forms.Any())
            {
                errs.Add(UICheckErrEnum.noForm);
            }
            foreach (var form in forms)
            {
                try
                {
                    var selectors = form.FindElements(By.CssSelector("select"));

                    IWebElement elementToClick = !foundTagsInfo.SeparateTags
                        ? form.FindElement(By.XPath($"//*[@type=\"submit\" and @value=\"{foundTagsInfo.mileTagValue}\"]"))
                        :form.FindElement(By.XPath("//*[@type=\"submit\"]"));

                    // Assume that there are two dropdown menues with conversion options
                    if (selectors.Count == 2)
                    {
                        try
                        {
                            selectors[0].FindElement(By.XPath($"option[contains(text(),\"{foundTagsInfo.mileTagText}\")]")).Click();
                            selectors[1].FindElement(By.XPath($"option[contains(text(),\"{foundTagsInfo.kmtagText}\")]")).Click();
                        }
                        catch
                        {
                            errs.Add(UICheckErrEnum.noOptionInDoubleDropdownMenu);
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
                        try
                        {
                            selectors[0].FindElement(By.XPath($"option[contains(text(),\"{foundTagsInfo.mileTagText}\")]")).Click();
                        }
                        catch(NoSuchElementException)
                        {
                            selectors[0].FindElement(By.XPath($"option[contains(text(),\"{foundTagsInfo.kmtagText}\")]")).Click();
                        }
                        catch
                        {
                            errs.Add(UICheckErrEnum.noOptionInSingleDropdownMenu);
                        }

                        foreach (var textField in form.FindElements(By.CssSelector("textarea")))
                        {
                            textField.Clear();
                            textField.SendKeys(applicantTestInput);
                        }

                        elementToClick.Click();
                    }

                    // No dropdown menu, only buttons
                    else if(!selectors.Any())
                    {
                        foreach (var textField in form.FindElements(By.CssSelector("textarea")))
                        {
                            textField.Clear();
                            textField.SendKeys(applicantTestInput);
                        }

                        elementToClick.Click();
                    }
                    else
                    {
                        errs.Add(UICheckErrEnum.noProperSelectTags);
                    }
                }
                catch { }
            }
            return errs;
        }
    }
}
