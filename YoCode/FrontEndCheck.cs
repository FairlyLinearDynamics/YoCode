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
            FrontEndEvidence.FeatureImplemented = CheckIfUIContainsFeature(); ;
            FrontEndEvidence.GiveEvidence(InputData());
            //browser.Close();
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

        private string InputData()
        {
            //var form = browser.FindElements(By.CssSelector("form"));

            //if (!form.Any())
            //{
            //    return null;
            //}

            //var selectors = browser.FindElements(By.CssSelector("select"));
            //if (selectors.Count > 1)
            //{
                
            //}

            var tags = browser.FindElements(By.CssSelector("select"));

            SelectElement click1 = new SelectElement(tags[0]);
            click1.SelectByText("Yard");

            SelectElement click2 = new SelectElement(tags[1]);
            click2.SelectByText("Mile");

            var textBoxes = browser.FindElements(By.CssSelector("textarea"));
            foreach(var texBox in textBoxes)
            {
                texBox.SendKeys("1");
            }

            var inputForm = browser.FindElement(By.CssSelector("form"));
            var child = inputForm.FindElement(By.CssSelector("div"));
            child.Submit();

            var output = browser.FindElements(By.CssSelector("textarea"));
            foreach(var textBox in output)
            {
                FrontEndEvidence.GiveEvidence(textBox.Text);
            }

            return tags.Count.ToString();
        }


        public FeatureEvidence FrontEndEvidence { get; private set; } = new FeatureEvidence();

    }
}