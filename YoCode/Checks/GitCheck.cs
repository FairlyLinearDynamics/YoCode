using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using LibGit2Sharp;

namespace YoCode
{
    internal class GitCheck
    {
        private readonly string repositoryPath;

        public GitCheck(string path)
        {
            repositoryPath = path;
            GitEvidence.Feature = Feature.GitCheck;
            GitEvidence.HelperMessage = messages.GitCheck;

            if (Repository.IsValid(repositoryPath))
            {
                ExecuteTheCheck();
            }
            else
            {
                GitEvidence.SetInconclusive(new SimpleEvidenceBuilder("Invalid git repository"));
            }
        }

        public void ExecuteTheCheck()
        {
            var output = new List<string>();
            using (var repo = new Repository(repositoryPath))
            {
                var commitLog = repo.Commits;

                var Output = CollectGitLogOutput(output, commitLog);

                FillInEvidence(commitLog, Output);
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

        private static string CollectGitLogOutput(List<string> output, IQueryableCommitLog commitLog)
        {
            const string RFC2822Format = "ddd dd MMM HH:mm:ss yyyy K";

            foreach (Commit c in commitLog.Where(c => !c.Author.Email.ContainsAny(GetHostDomains())))
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

        public static List<string> GetHostDomains()
        {
            return new List<string> { "@nonlinear.com", "@waters.com" };
        }

        public FeatureEvidence GitEvidence { get; } = new FeatureEvidence();
    }
}