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
        private readonly string[] mileKeyWords = UIKeywords.MILE_KEYWORDS;
        private readonly string[] kmKeyWords = UIKeywords.KM_KEYWORDS;
        
        public UIFeatureImplemented(IWebDriver browser)
        {
            UIFeatureImplementedEvidence.FeatureTitle = "Found feature evidence in user interface";
            UIFeatureImplementedEvidence.Feature = Feature.UIFeatureImplemented;
            UIFeatureImplementedEvidence.HelperMessage = messages.UIFeatureImplemented;

            this.browser = browser;
            ExecuteCheck();
        }

        private void ExecuteCheck()
        {
            var milesTag = GetKeywordTagTextInHTML(UIKeywords.MILE_KEYWORDS);
            var kmTag = GetKeywordTagTextInHTML(UIKeywords.KM_KEYWORDS);

            if (milesTag != null && kmTag != null) 
            {
                FoundTagsInfo.mileTagText = UIKeywords.MILE_KEYWORDS.Any(a => milesTag.Text.ToLower().Contains(a)) 
                    ? milesTag.Text : milesTag.GetAttribute("value");

                FoundTagsInfo.kmtagText = UIKeywords.KM_KEYWORDS.Any(a=> kmTag.Text.ToLower().Contains(a)) 
                    ? kmTag.Text : kmTag.GetAttribute("value");

                FoundTagsInfo.mileTagValue = milesTag.GetAttribute("value");
                FoundTagsInfo.kmTagValue = kmTag.GetAttribute("value");

                string evidence;
                if (FoundTagsInfo.SeparateTags)
                {
                    evidence = $"Found \"{FoundTagsInfo.mileTagText}\"" + $" and \"{FoundTagsInfo.kmtagText}\" keywords in user interface";
                }
                else
                {
                    evidence = $"Found \"{FoundTagsInfo.mileTagText}\" keyword in user interface";
                }

                UIFeatureImplementedEvidence.SetPassed(new SimpleEvidenceBuilder(evidence));
                UIFeatureImplementedEvidence.FeatureRating = 1;
            }
            else
            {
                UIFeatureImplementedEvidence.SetFailed(new SimpleEvidenceBuilder("Did not find any evidence in user interface"));
                UIFeatureImplementedEvidence.FeatureRating = 0;
            }
        }

        private IWebElement GetKeywordTagTextInHTML(string[] keywords)
        {
            foreach (var tag in Enum.GetNames(typeof(HtmlTags)))
            {
                foreach (var presentTag in browser.FindElements(By.CssSelector(tag)))
                {
                    if (keywords.Any(a => presentTag.GetAttribute("value").Contains(a, StringComparison.OrdinalIgnoreCase)))
                    {
                        return presentTag;
                    }
                    else if(keywords.Any(a => presentTag.Text.Contains(a, StringComparison.OrdinalIgnoreCase)))
                    {
                        return presentTag;
                    }
                }

            }
            return null;
        }

        public FeatureEvidence UIFeatureImplementedEvidence { get; set; } = new FeatureEvidence();
        public UIFoundTags FoundTagsInfo = new UIFoundTags();
    }
}
