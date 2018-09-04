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
            FileChangeEvidence.Feature = Feature.FilesChangedCheck;
            FileChangeEvidence.HelperMessage = messages.FilesChangedCheck;

            if (!Repository.IsValid(path))
            {
                FileChangeEvidence.SetInconclusive(new SimpleEvidenceBuilder("Git Repository Not Found"));
            }
            else
            {
                ExecuteTheCheck(path);
            }
        }

        public void ExecuteTheCheck(string path)
        {
            using (var repo = new Repository(path))
            {
                UncommitedFiles = GetUncommitedFiles(repo);

                if (!GitCheck.LastCommitWasByNonEmployee(repo.Commits) && !UncommitedFiles.Any())
                {
                    FileChangeEvidence.SetFailed(new SimpleEvidenceBuilder("Git Repository Not Found"));
                    return;
                }

                GetFileDifferences(repo);

                FillInEvidence(repo);
            }
        }

        private void FillInEvidence(Repository repo)
        {
            FileChangeEvidence.SetPassed(new FileDiffEvidenceBuilder(GetFileDifferences(repo), BuildFileChangeOutput()));
            FileChangeEvidence.FeatureRating = 1;
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

        private Patch GetFileDifferences(Repository Repo)
        {
            Tree head = Repo.Head.Tip.Tree;

            Tree lastNonlinearCommit = Repo.Head.Commits.ToList().First
                (a => a.Author.Email.ContainsAny(GitCheck.GetHostDomains())).Tree;

            return Repo.Diff.Compare<Patch>(lastNonlinearCommit, head);
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
