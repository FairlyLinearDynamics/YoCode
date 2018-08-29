namespace YoCode
{
    internal interface IRunParameterChecker
    {
        string CMDToolsPath { get; set; }
        string DotCoverDir { get; set; }

        string TestCodeBaseCost { get; set; }
        string TestDuplicationCost { get; set; }
        string AppCodeBaseCost { get; set; }
        string AppDuplicationCost { get; set; }

        bool ParametersAreValid(bool isJunior);
    }
}