
namespace YoCode
{
    class ProjectBuilder
    {

        string ProcessName { get; } = "dotnet";
        string Arguments { get; } = "build";

        public ProjectBuilder(string workingDir)
        {
            ProcessRunner pr = new ProcessRunner(ProcessName, workingDir, Arguments);
            pr.ExecuteTheCheck();
        }

        public string FindBuildResult(string output)
        {

            //do something with string
            return output;
        }
    }
}
