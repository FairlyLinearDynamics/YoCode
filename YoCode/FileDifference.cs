using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YoCode
{
    internal class FileDifference
    {
        public FileDifference(string path)
        {
            if (!Repository.IsValid(path))
            {
                Console.WriteLine("Invalid Repo");
            }
            else
            {
                ExecuteTheCheck(path);
            }
        }

        public static Dictionary<string,string> ExecuteTheCheck(string path)
        {
            var dic = new Dictionary<string,string>();
            var list = new List<List<string>>();

            using (var Repo = new Repository(path))
            {
                Tree head = Repo.Head.Tip.Tree;
                Tree lastNonlinearCommit = Repo.Head.Commits.ToList().First
                (a => a.Author.Email.ContainsAny(GitCheck.GetHostDomains())).Tree;

                var diff = Repo.Diff.Compare<Patch>(lastNonlinearCommit, head);

                foreach(var d in diff)
                {
                    dic.Add(d.Path, d.Patch);
                }

                var files = diff.Content.Split("diff --git ").ToList();
                files.RemoveAt(0);

                files.ForEach(a => list.Add(a.Split("\n").ToList()));


                foreach (var item in list)
                {
                    item.ForEach(a => Console.WriteLine(a + Environment.NewLine));
                    Console.WriteLine("==================================================================");
                }

                return dic;
            }
        }
    }
}
