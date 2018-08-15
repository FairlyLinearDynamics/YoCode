namespace YoCode
{
    internal interface IDupFinder
    {
        FeatureEvidence Execute(string solutionTitle, string solutionPath);
    }
}