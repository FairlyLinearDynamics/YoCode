namespace YoCode
{
    public class TestResults
    {
        bool uiCheck;
        bool gitUsed;
        bool solutionFileExist;

        public bool UiCheck { set => uiCheck = value; }
        public bool GitUsed { set => gitUsed = value; }
        public bool AnyFileChanged { get; set; }
        public bool SolutionFileExist { set => solutionFileExist = value; }

        public string UiCheckResult()
        {
            return (uiCheck) ? "Yes" : "No";
        }

        public string GitUsedResult()
        {
            return (gitUsed) ? "Yes" : "No";
        }

        public string SolutionFileExistResult()
        {
            return (solutionFileExist) ? "Yes" : "No";
        }
    }
}