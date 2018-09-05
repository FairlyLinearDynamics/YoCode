using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace YoCode
{
    internal class GitCheck : ICheck
    {
        private readonly string repositoryPath;

        public GitCheck(ICheckConfig checkConfig)
        {
            repositoryPath = checkConfig.PathManager.ModifiedTestDirPath;
            GitEvidence.Feature = Feature.GitCheck;
            GitEvidence.HelperMessage = messages.GitCheck;
        }

        private void ExecuteTheCheck()
        {
            using (var repo = new Repository(repositoryPath))
            {
                var commitLog = repo.Commits;

                var gitLogOutput = CollectGitLogOutput(commitLog);

                FillInEvidence(commitLog, gitLogOutput);
            }
        }

        private void FillInEvidence(IQueryableCommitLog commitLog, string output)
        {
            if (LastCommitWasByNonEmployee(commitLog))
            {
                GitEvidence.FeatureRating = 1;
                GitEvidence.SetPassed(new SimpleEvidenceBuilder("Commits:" + Environment.NewLine + output));
            }
            else
            {
                GitEvidence.SetFailed(new SimpleEvidenceBuilder("Last Commit By Waters Employee"));
            }
        }

        private static string CollectGitLogOutput(IQueryableCommitLog commitLog)
        {
            const string RFC2822Format = "ddd dd MMM HH:mm:ss yyyy K";

            var output = new List<string>();

            foreach (var c in commitLog.Where(c => !c.Author.Email.ContainsAny(GetHostDomains())))
            {
                output.Add(string.Format("commit {0}", c.Id));

                if (c.Parents.Count() > 1)
                {
                    output.Add($"Merge: {string.Join(" ", c.Parents.Select(p => p.Id.Sha.Substring(0, 7)).ToArray())}");
                }

                output.Add(string.Format("Author: {0} <{1}>", c.Author.Name, c.Author.Email));
                output.Add($"Date:   {c.Author.When.ToString(RFC2822Format, CultureInfo.InvariantCulture)}" + Environment.NewLine);
                output.Add(c.Message + Environment.NewLine);
            }
            return string.Join(Environment.NewLine, output);
        }

        public static bool LastCommitWasByNonEmployee(IQueryableCommitLog c)
        {
            return !c.First().Author.Email.ContainsAny(GetHostDomains());
        }

        public static IEnumerable<string> GetHostDomains()
        {
            return new List<string> { "@nonlinear.com", "@waters.com" };
        }

        private FeatureEvidence GitEvidence { get; } = new FeatureEvidence();

        public Task<List<FeatureEvidence>> Execute()
        {
            return Task.Run(() =>
            {
                if (Repository.IsValid(repositoryPath))
                {
                    ExecuteTheCheck();
                }
                else
                {
                    GitEvidence.SetInconclusive(new SimpleEvidenceBuilder("Invalid git repository"));
                }

                return new List<FeatureEvidence> {GitEvidence};
            });
        }
    }
}