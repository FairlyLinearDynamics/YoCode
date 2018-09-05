namespace YoCode
{
    internal interface ICheckConfig
    {
        IPathManager PathManager { get; }
    }

    internal class CheckConfig : ICheckConfig
    {
        public CheckConfig(IPathManager pathManager, RunParameterChecker parameters)
        {
            PathManager = pathManager;
            RunParameters = parameters;
        }

        public IPathManager PathManager { get; }
        public RunParameterChecker RunParameters { get; }
    }
}