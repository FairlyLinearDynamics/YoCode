using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoCode
{
    class UIFeatureImplemented
    {
        private readonly IWebDriver browser;
        private readonly string[] keyWords;
        
        public UIFeatureImplemented(IWebDriver browser, string[] keyWords)
        {
            this.browser = browser;
            this.keyWords = keyWords;
            ExecuteCheck();
        }

        private void ExecuteCheck()
        {
            foreach (HtmlTags tag in Enum.GetValues(typeof(HtmlTags)))
            {
                if (SearchForElement(tag, keyWords))
                {
                    return;
                }
            }
        }

        private bool SearchForElement(HtmlTags htmlTag, string[] keyWords)
        {
            foreach (var tag in browser.FindElements(By.CssSelector(htmlTag.ToString())))
            {
                if (tag.TagName == nameof(HtmlTags.input))
                {
                    if (keyWords.Any(a => tag.GetAttribute("value").Contains(a, StringComparison.OrdinalIgnoreCase)))
                    {
                        FeatureTagFound = tag.GetAttribute("value");
                        return true;
                    }
                }
                else if (keyWords.Any(a => tag.Text.Equals(a, StringComparison.OrdinalIgnoreCase)))
                {
                    FeatureTagFound = tag.Text;
                    return true;
                }
            }
            return false;
        }

        public string FeatureTagFound { get; set; } 
    }
}
