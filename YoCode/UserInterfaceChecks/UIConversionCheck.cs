﻿using OpenQA.Selenium;
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

        public UIConversionCheck(IWebDriver browser, string foundKeyWord)
        {
            UIConversionEvidence.FeatureTitle = "Units were converted successfully using UI";
            UIConversionEvidence.Feature = Feature.UIConversionCheck;

            this.browser = browser;

            var uiInputhandler = new InputingToUI(browser, foundKeyWord);
            foreach(var key in UIKeywords.PROPER_INPUT)
            {
                uiInputhandler.InputData(key);
                try
                {
                    browser.FindElement(By.XPath($"//*[contains(text(),\"{GetCorrectClampedNum(Double.Parse(key) * unitConvertValue)}\")]"));
                }
                catch (NoSuchElementException)
                {
                    UIConversionEvidence.SetFailed("Values were converted incorrectly");
                    UIConversionEvidence.FeatureRating = 0;
                    browser.Navigate().Back();
                    return;
                }
                browser.Navigate().Back();
            }

            UIConversionEvidence.FeatureImplemented = true;
            UIConversionEvidence.FeatureRating = 1;
            UIConversionEvidence.GiveEvidence("Successfully converted from miles to kilometres");
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
