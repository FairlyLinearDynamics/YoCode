using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YoCode
{
    internal class FileChangeFinder
    {
        private List<string> FileList { get; } = new List<string>();
        private List<string> UncommitedFiles { get; set; } = new List<string>();

        public FileChangeFinder(string path)
        {
            FileChangeEvidence.FeatureTitle = "Files changed";
            FileChangeEvidence.Feature = Feature.FilesChangedCheck;
            FileChangeEvidence.HelperMessage = messages.FilesChangedCheck;

            if (!Repository.IsValid(path))
            {
                FileChangeEvidence.SetInconclusive("Git Repository Not Found");
            }
            else
            {
                ExecuteTheCheck(path);
            }
        }

        public void ExecuteTheCheck(string path)
        {
            using (var Repo = new Repository(path))
            {
                UncommitedFiles = GetUncommitedFiles(Repo);

                if (!GitCheck.LastCommitWasByNonEmployee(Repo.Commits) && !UncommitedFiles.Any())
                {
                    FileChangeEvidence.SetFailed("Last Commit By Waters Employee");
                    return;
                }

                GetFileDifferences(Repo);

                FillInEvidence();
            }
        }

        private void FillInEvidence()
        {
            FileChangeEvidence.FeatureImplemented = true;
            FileChangeEvidence.FeatureRating = 1;
            FileChangeEvidence.GiveEvidence(BuildFileChangeOutput());
        }

        private static List<string> GetUncommitedFiles(Repository repository)
        {

            var newInIndex = new List<string>();
            foreach (var item in repository.RetrieveStatus(new StatusOptions()))
            {
                if (FileIsNotCommited(item))
                {
                    newInIndex.Add(item.FilePath);
                }
            }

            return newInIndex;
        }

        private static bool FileIsNotCommited(StatusEntry item)
        {
            /*NewInIndex = staged but not commited, NewInWorkdir = untracked*/
            return item.State == FileStatus.NewInIndex || item.State == FileStatus.NewInWorkdir || 
                item.State == FileStatus.ModifiedInWorkdir || item.State == FileStatus.ModifiedInIndex;
        }

        private void GetFileDifferences(Repository Repo)
        {
            Tree head = Repo.Head.Tip.Tree;

            Tree lastNonlinearCommit = Repo.Head.Commits.ToList().First
                (a => a.Author.Email.ContainsAny(GitCheck.GetHostDomains())).Tree;

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
