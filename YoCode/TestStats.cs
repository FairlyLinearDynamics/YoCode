﻿using System;
using System.Collections.Generic;
using System.Text;

namespace YoCode
{
    public struct TestStats
    {
        public int totalTests;
        public int testsPassed;
        public int testsFailed;
        public int testsSkipped;
        public double percentage => (testsPassed * 100.0) / totalTests;

    };
}
