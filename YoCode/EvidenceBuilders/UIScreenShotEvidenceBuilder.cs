using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace YoCode
{
    class UIScreenShotEvidenceBuilder : IEvidence
    {
        private string ScreenShot;
        private string bonusEvidence;

        public UIScreenShotEvidenceBuilder(string screenShot, string evidence)
        {
            ScreenShot = screenShot;
            bonusEvidence = evidence;
        }

        public string BuildEvidenceForConsole()
        {
            return bonusEvidence;
        }

        public string BuildEvidenceForHTML()
        {
            var resultEvidence = new StringBuilder();
            resultEvidence.AppendLine(WebElementBuilder.FormatAndEncapsulateParagraph(bonusEvidence));
            resultEvidence.AppendLine(WebElementBuilder.FormatImageElement(ScreenShot));
            return resultEvidence.ToString();
        }
    }
}
