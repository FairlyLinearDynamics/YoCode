﻿using System;
using System.Collections.Generic;
using System.Text;

namespace YoCode
{
    public struct UIKeywords
    {
        public static string[] UNIT_KEYWORDS = { "miles", "kilometres", "mile", "kilometre", "kilometers", "kilometer", "km", "mi" };
        public static string[] GARBAGE_INPUT = { "", Environment.NewLine, $"{Environment.NewLine}5",
            $"5{Environment.NewLine}{Environment.NewLine}5", $"5{Environment.NewLine}{Environment.NewLine}", "a b c"};
    }
}