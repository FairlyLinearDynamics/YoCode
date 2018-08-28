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
            UIFeatureImplementedEvidence.FeatureTitle = "Found feature evidence in user interface";
            UIFeatureImplementedEvidence.Feature = Feature.UIFeatureImplmeneted;

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
            UIFeatureImplementedEvidence.FeatureImplemented = false;
            UIFeatureImplementedEvidence.FeatureRating = 0;
            UIFeatureImplementedEvidence.GiveEvidence($"Did not find any evidence in user interface");
        }

        private bool SearchForElement(HtmlTags htmlTag, string[] keyWords)
        {
            foreach (var tag in browser.FindElements(By.CssSelector(htmlTag.ToString())))
            {
                if (keyWords.Any(a => tag.GetAttribute("value").Contains(a, StringComparison.OrdinalIgnoreCase)))
                {
                    UIFeatureImplementedEvidence.FeatureImplemented = true;
                    UIFeatureImplementedEvidence.FeatureRating = 1;
                    UIFeatureImplementedEvidence.GiveEvidence($"Found \"{tag.GetAttribute("value")}\" keyword in user interface");
                    FeatureTagFound = tag.GetAttribute("value");
                    return true;
                }
                else if (keyWords.Any(a => tag.Text.Equals(a, StringComparison.OrdinalIgnoreCase)))
                {
                    UIFeatureImplementedEvidence.FeatureImplemented = true;
                    UIFeatureImplementedEvidence.FeatureRating = 1;
                    UIFeatureImplementedEvidence.GiveEvidence($"Found \"{tag.GetAttribute("value")}\" keyword in user interface");
                    FeatureTagFound = tag.Text;
                    return true;
                }
            }
            return false;
        }

        public FeatureEvidence UIFeatureImplementedEvidence { get; set; } = new FeatureEvidence();
        public string FeatureTagFound { get; set; } = null;
    }
}
