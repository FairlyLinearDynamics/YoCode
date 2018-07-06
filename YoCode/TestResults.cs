namespace YoCode
{
    public class TestResults
    {
        bool uiCheck;
        bool solutionExists;
        bool anyFileChanged;

        public bool UiCheck { set => uiCheck = value; }
        public bool SolutionExists { set => solutionExists = value; }
        public bool AnyFileChanged { set => anyFileChanged = value; }

        public string UiCheckResult()
        {
            return (uiCheck) ? "Yes" : "No";
        }

        public string SolutionExistsResult()
        {
            return (solutionExists) ? "Yes" : "No";
        }

        public string AnyFileChangedResult()
        {
            return (anyFileChanged) ? "Yes" : "No";
        }
    }
}