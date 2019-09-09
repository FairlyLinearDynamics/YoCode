namespace YoCode
{
    internal interface IRunParameterChecker
    {
        ToolPath DupFinderPath { get; set; }
        ToolPath DotCoverPath { get; set; }

        string TestCodeBaseCost { get; set; }
        string TestDuplicationCost { get; set; }
        string AppCodeBaseCost { get; set; }
        string AppDuplicationCost { get; set; }

    }
}