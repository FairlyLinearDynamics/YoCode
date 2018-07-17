using System.Collections.Generic;

namespace YoCode
{
    public class GitCheck
    {
        private readonly string repositoryPath;

        public GitCheck(string path)
        {
            repositoryPath = path;
            ExecuteTheCheck();
        }

        public void ExecuteTheCheck()
        {
            ProcessRunner pr = new ProcessRunner("git.exe", repositoryPath, "log");
            pr.ExecuteTheCheck();
            var Output = pr.Output;
            var LastAuthor = Output.GetLineWithAllKeywords(GetKeyWords());

            GitEvidence.FeatureTitle = "Git was used";

            if(pr.TimedOut)
            {
                GitEvidence.FeatureImplemented = false;
                GitEvidence.GiveEvidence("Timed out");
                return;
            }

            GitEvidence.FeatureImplemented = GitHasBeenUsed(LastAuthor);

            if (GitEvidence.FeatureImplemented)
            {
                GitEvidence.GiveEvidence($"Commit outputs: \n{Output}\nLast Author: {LastAuthor}");
            }
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

        public FeatureEvidence GitEvidence { get; } = new FeatureEvidence();
    }
}