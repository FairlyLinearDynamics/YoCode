using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YoCode
{
    class UIConversionCheck
    {
        private const int decimalPoints = 1000;
        private const double unitConvertValue = 1.60934;

        public UIConversionCheck(IWebDriver browser, UIFoundTags foundKeyWord)
        {
            UIConversionEvidence.Feature = Feature.UIConversionCheck;
            UIConversionEvidence.HelperMessage = messages.UIConversionCheck;

            var uiInputHandler = new InputingToUI(browser, foundKeyWord);
            foreach(var key in UIKeywords.PROPER_INPUT)
            {
                var errs = uiInputHandler.InputData(key);
                if (errs.Any())
                {
                    SetCheckUndefined(errs);
                    return;
                }

                try
                {
                    browser.FindElement(By.XPath($"//*[contains(text(),\"{GetCorrectClampedNum(double.Parse(key) * unitConvertValue)}\")]"));
                }
                catch (NoSuchElementException)
                {
                    UIConversionEvidence.SetFailed(new SimpleEvidenceBuilder("Values were converted incorrectly"));
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

        private static double GetCorrectClampedNum(double num)
        {
            num *= decimalPoints;
            var temp = (int)num;
            return (double)temp / decimalPoints;
        }

        public FeatureEvidence UIConversionEvidence { get; } = new FeatureEvidence();
    }
}
