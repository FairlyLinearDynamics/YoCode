using System;

namespace YoCode
{
    internal struct UIKeywords
    {
        public static string[] UNIT_KEYWORDS = { "miles", "kilometres", "mile", "kilometre", "kilometers", "kilometer", "km", "mi" };

        public static string[] GARBAGE_INPUT = { "", $"{Environment.NewLine}5",
            $"5{Environment.NewLine}{Environment.NewLine}5", $"5{Environment.NewLine}{Environment.NewLine}", "a b c"};
    }
}
