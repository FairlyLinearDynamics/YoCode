using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace YoCode
{
    class UIScreenShotEvidenceBuilder : IEvidence
    {
        public Screenshot ScreenShot { get; set; }
        string bonusEvidence;

        public UIScreenShotEvidenceBuilder(Screenshot screenShot, string evidence)
        {
            ScreenShot = screenShot;
            bonusEvidence = evidence;
        }

        public string BuildEvidenceForConsole()
        {
            throw new NotImplementedException();
        }

        public string BuildEvidenceForHTML()
        {
            return WebElementBuilder.FormatAndEncapsulateParagraph(bonusEvidence);
        }
    }
}
