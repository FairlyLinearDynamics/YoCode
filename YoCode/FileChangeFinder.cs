using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YoCode
{
    internal class FileChangeFinder
    {
        private Repository Repo { get; }
        private List<string> FileList { get; } = new List<string>();
        private List<string> UncommitedFiles { get; } = new List<string>();
        private readonly List<string> ignoredAuthorEmails = new List<string> { "@nonlinear.com", "@waters.com" };

        public FileChangeFinder(string path)
        {
            FileChangeEvidence.FeatureTitle = "Files changed";

            if(!Repository.IsValid(path))
            {
                FileChangeEvidence.SetFailed("Git Repository Not Found");
                return;
            }

            Repo = new Repository(path);

            GetFileDifferences();
            GetUncommitedFiles(path);

            if (FileList.Any() || UncommitedFiles.Any())
            {
                FileChangeEvidence.FeatureImplemented = true;
                FileChangeEvidence.GiveEvidence(BuildFileChangeOutput());
            }
            else
            {
                FileChangeEvidence.SetFailed("No Files Changed");
            }
        }

        private void GetUncommitedFiles(string path)
        {
            using (var repository = new Repository(path))
            {
                foreach (var item in repository.RetrieveStatus(new StatusOptions()))
                {
                    if (item.State == FileStatus.NewInIndex)
                    {
                        UncommitedFiles.Add(item.FilePath);
                    }
                }
            }
        }

        private void GetFileDifferences()
        {
            Tree head = Repo.Head.Tip.Tree;

            Tree lastNonlinearCommit = Repo.Head.Commits.ToList().First
                (a => a.Author.Email.ContainsAny(ignoredAuthorEmails)).Tree;

            foreach (var pec in Repo.Diff.Compare<Patch>(lastNonlinearCommit, head))
            {
                var lineDifference = pec.LinesAdded + pec.LinesDeleted;
                FileList.Add($"{pec.Status} : {pec.Path} = {lineDifference} ({pec.LinesAdded}+ and {pec.LinesDeleted}-)");
            }
        }

        public string BuildFileChangeOutput()
        {
            if (UncommitedFiles.Any())
            {
                FileList.Add(String.Empty);
                FileList.Add("Untracked/Uncommited Files:");
                FileList.Add(String.Empty);
                FileList.AddRange(UncommitedFiles);
            }
            return String.Join(Environment.NewLine, FileList);
        }

        public FeatureEvidence FileChangeEvidence { get; } = new FeatureEvidence();
    }
}
