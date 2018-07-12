using System.Collections.Generic;
using System.Diagnostics;

namespace YoCode
{
    public class GitCheck
    {
        private readonly string repositoryPath;

        private string Output { get; set; } = "No output found";
        private string LastAuthor { get; set; } = "None";
        public bool GitUsed { get; private set; }

        public GitCheck(string path)
        {
                repositoryPath = path;
                ExecuteTheCheck();
        }

        public void ExecuteTheCheck()
        {
            ProcessRunner pr = new ProcessRunner("git",repositoryPath, "log");

            pr.ExecuteTheCheck();

            Output = pr.Output;
            LastAuthor = Output.GetLineWithAllKeywords(GetKeyWords());
            GitUsed = GitHasBeenUsed(LastAuthor);
        }

        public static bool GitHasBeenUsed(string lastAuthor)
        {
            if (lastAuthor.ContainsAny(GetHostDomains()) || string.IsNullOrEmpty(lastAuthor))
            {
                return false;
            }
            return true;
        }

        public static List<string> GetKeyWords()
        {
            return new List<string> { "Author:", "<", ">", "@", "." };
        }

        public static List<string> GetHostDomains()
        {
            return new List<string> { "@nonlinear.com", "@waters.com" };
        }

        public ProcessInfo SetupProcessInfo(string processName,string arguments)
        {
            ProcessInfo pi;
            pi.processName = processName;
            pi.workingDir = repositoryPath;
            pi.arguments = arguments;

            return pi;
        }
    }
}