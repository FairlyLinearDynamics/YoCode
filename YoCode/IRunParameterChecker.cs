namespace YoCode
{
    internal interface IRunParameterChecker
    {
        string CMDToolsPath { get; set; }
        string CodeBaseCost { get; set; }
        string DotCoverDir { get; set; }
        string DuplicationCost { get; set; }
    }
}