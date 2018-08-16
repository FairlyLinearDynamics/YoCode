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
            Repo = new Repository(path);
            GetAddedAndModifiedFiles(path);
            FileList.ForEach(Console.WriteLine);

            GetUncommitedFiles(path);

            FileChangeEvidence.FeatureTitle = "Files changed";

            GetFileDifferences();

            //Console.WriteLine();
            //Console.WriteLine(BuildFileChangeOutput());

            //if(FileList.Any())
            //{
            //    FileChangeEvidence.FeatureImplemented = true;
            //    FileChangeEvidence.GiveEvidence(GetChangedFiles());
            //}
            //else
            //{
            //    FileChangeEvidence.SetFailed("No Files Changed");
            //}
        }

        private void GetUncommitedFiles(string path)
        {
            using (var repository = new Repository(path))
            {
                foreach (var item in repository.RetrieveStatus(new StatusOptions()))
                {
                    if(item.State == FileStatus.NewInIndex)
                    {
                        UncommitedFiles.Add(item.FilePath);
                        Console.WriteLine(item.FilePath + "    " + item.State);
                    }
                    
                }
            }
        }

        private void GetAddedAndModifiedFiles(string path)
        {

            //make not check every commit but only the ones we need
            var repo = new Repository(path);
            foreach (Commit commit in repo.Commits.Reverse())
            {
                foreach (var parent in commit.Parents.Where(a => !a.Author.Email.ContainsAny(ignoredAuthorEmails)))
                {
                    foreach (TreeEntryChanges change in repo.Diff.Compare<TreeChanges>(parent.Tree,
                    commit.Tree))
                    {
                        if (!FileList.Contains($"{change.Status} : {change.Path}"))
                        {
                            FileList.Add($"{change.Status} : {change.Path}");
                        }

                        if (change.Status == ChangeKind.Deleted)
                        {
                            FixChangeList(change);
                        }
                    }
                }
                
            }
        }

        private void FixChangeList(TreeEntryChanges change)
        {
            for (int i = 0; i < FileList.Count; i++)
            {
                if (FileList[i].Contains(change.Path))
                {
                    FileList.RemoveAt(i);
                }
            }
        }

        private void GetFileDifferences()
        {
            Tree head = Repo.Head.Tip.Tree;

            Tree lastNonlinearCommit = Repo.Head.Commits.ToList().First
                (a => a.Author.Email.ContainsAny(ignoredAuthorEmails)).Tree;

            foreach (var pec in Repo.Diff.Compare<Patch>(head, lastNonlinearCommit))
            {
                var lineDifference = pec.LinesAdded + pec.LinesDeleted;

                foreach (var item in FileList)
                {
                    if (item.Contains(pec.Path))
                    {
                        //item.Append
                    }
                }
                FileList.Add($"{pec.Path} = {lineDifference} ({pec.LinesAdded}+ and {pec.LinesDeleted}-)");
            }
        }

        public string BuildFileChangeOutput()
        {
            return String.Join(Environment.NewLine, FileList);
        }

        public FeatureEvidence FileChangeEvidence { get; } = new FeatureEvidence();
    }
}
