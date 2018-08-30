using OpenQA.Selenium;

namespace YoCode
{ 
    public struct UIFoundTags
    {
        public bool SeparateTags => mileTagText != kmtagText;

        public string mileTagText;
        public string kmtagText;

        public string mileTagValue;
        public string kmTagValue;
    }
}
