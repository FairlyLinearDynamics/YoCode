namespace YoCode
{
    interface IPrint
    {
        void AddFeature(FeatureData data);
        void AddMessage(string message);
        void AddBanner();
        void WriteReport();
        void AddFinalScore(double score);
    }
}
