namespace YoCode
{
    internal struct TestStats
    {
        public int totalTests;
        public int testsPassed;
        public int testsFailed;
        public int testsSkipped;
        public double PercentagePassed => (testsPassed * 100.0) / totalTests;
    }
}
