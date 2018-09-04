using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoCode
{
    class UIConversionCheck
    {
        IWebDriver browser;
        private const int TitleColumnFormatter = -40;
        private const int ValueColumnFormatter = -10;
        private const int decimalPoints = 1000;

        private static double unitConvertValue = 1.60934;

        private StringBuilder uiConversionResultsOutput = new StringBuilder();

        public UIConversionCheck(IWebDriver browser, UIFoundTags foundKeyWord)
        {
            UIConversionEvidence.FeatureTitle = "Units were converted successfully using UI";
            UIConversionEvidence.Feature = Feature.UIConversionCheck;
            UIConversionEvidence.HelperMessage = messages.UIConversionCheck;

            this.browser = browser;

            var uiInputhandler = new InputingToUI(browser, foundKeyWord);
            foreach(var key in UIKeywords.PROPER_INPUT)
            {
                var errs = uiInputhandler.InputData(key);
                if (errs.Any())
                {
                    SetCheckUndefined(errs);
                    return;
                }

                try
                {
                    browser.FindElement(By.XPath($"//*[contains(text(),\"{GetCorrectClampedNum(Double.Parse(key) * unitConvertValue)}\")]"));
                }
                catch (NoSuchElementException)
                {
                    uiConversionResultsOutput.AppendLine("Values were converted incorrectly");
                    UIConversionEvidence.FeatureRating = 0;
                    browser.Navigate().Back();
                    return;
                }
                browser.Navigate().Back();
            }

            UIConversionEvidence.SetPassed(new SimpleEvidenceBuilder("Successfully converted from miles to kilometres"));
            UIConversionEvidence.FeatureRating = 1;
        }

        private void SetCheckUndefined(List<UICheckErrEnum> errs)
        {
            UIConversionEvidence.SetInconclusive(new SimpleEvidenceBuilder(UIEnumErrFormat.ConvertEnum(errs).ToList()));
        }

        private double GetCorrectClampedNum(double num)
        {
            num *= decimalPoints;
            var temp = (int)num;
            return (double)temp / decimalPoints;
        }

        public FeatureEvidence UIConversionEvidence { get; set; } = new FeatureEvidence();
    }
}
