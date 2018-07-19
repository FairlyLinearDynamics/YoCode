using System.Collections.Generic;

namespace YoCode
{
    public class GitCheck
    {
        private readonly string repositoryPath;
        private readonly IFeatureRunner featureRunner;

        public GitCheck(string path, IFeatureRunner featureRunner)
        {
            repositoryPath = path;
            GitEvidence.FeatureTitle = "Git was used";


            this.featureRunner = featureRunner;
            ExecuteTheCheck();
        }

        public void ExecuteTheCheck()
        {
            var processDetails = new ProcessDetails("git.exe", repositoryPath, "log");

            var evidence = featureRunner.Execute(processDetails, "Git was used");
            if (evidence.FeatureFailed)
            {
                return;
            }

            var lastAuthor = evidence.Output.GetLineWithAllKeywords(GetKeyWords());
            GitEvidence.FeatureImplemented = GitHasBeenUsed(lastAuthor);

            if (GitEvidence.FeatureImplemented)
            {
                GitEvidence.GiveEvidence($"Commit outputs: \n{evidence.Output}\nLast {lastAuthor}");
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