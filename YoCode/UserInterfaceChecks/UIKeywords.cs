using System;

namespace YoCode
{
    internal struct UIKeywords
    {
        public static string[] MILE_KEYWORDS = { "miles", "mile", "mi" };
        public static string[] KM_KEYWORDS = { "kilometres", "kilometre", "kilometers", "kilometer", "km"};

        public static string[] GARBAGE_INPUT = { "", $"{Environment.NewLine}5",
            $"5{Environment.NewLine}{Environment.NewLine}5", $"5{Environment.NewLine}{Environment.NewLine}", "a b c"};

        public static string[] PROPER_INPUT = { "0", "1", "2", "3" };
    }
}
