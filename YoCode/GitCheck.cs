using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace YoCode
{
    public static class HelperMethods
    {
        public static bool ContainsAny(this string line, IEnumerable<string> keywords)
        {
            return keywords.Any(line.Contains);
        }
    }

    public class GitCheck
    {
        //needs to be fixed
        private readonly string repositoryPath;
        private string Output { get; set; } = "No output found";
        private string LastAuthor { get; set; } = "None";
        public bool GitUsed { get; private set; }
        private bool FailedToGetRepo { get; set; }

        public GitCheck(string path)
        {
            if (Directory.Exists(path))
            {
                repositoryPath = path;
                ExecuteTheCheck();
            }
            else
            {
                Console.WriteLine("Invalid directory");
                FailedToGetRepo = true;
            }

        }

        public void ExecuteTheCheck()
        {   

            ProcessRunner pr = new ProcessRunner(setupProcessInfo("git.exe", "workingDir", "log"));
            pr.ExecuteTheCheck();

            RunCheck r = new RunCheck();
            r.ExecuteTheCheck();

            Output = pr.Output;
            LastAuthor = GetLastAuthor(Output);
            GitUsed = GitHasBeenUsed(LastAuthor, GetHostDomains());
        }


        // get name and email address of the last author
        public static string GetLastAuthor(string output)
        {
            var sr = new StringReader(output);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (ContainsAll(line, GetKeyWords()))
                {
                    return line;
                }
            }
            return "";
        }


        public static bool GitHasBeenUsed(string lastAuthor, List<string> hostDomains)
        {
            if (ContainsAny(lastAuthor, GetHostDomains()) || string.IsNullOrEmpty(lastAuthor))
            {
                return false;
            }
            return true;
        }

        public static bool ContainsAny(string line, IEnumerable<string> keywords)
        {
            return line.ContainsAny(keywords);
        }

        public static bool ContainsAll(string line, IEnumerable<string> keywords)
        {
            return keywords.All(line.Contains);
        }

        public static IEnumerable<string> GetKeyWords()
        {
            return new List<string> { "Author:", "<", ">", "@", "." };
        }

        public static List<string> GetHostDomains()
        {
            return new List<string> { "@nonlinear.com", "@waters.com" };
        }

        public ProcessInfo setupProcessInfo(string processName,string workingDir,string arguments)
        {
            ProcessInfo pi;
            pi.processName = "git.exe";
            pi.workingDir = repositoryPath;
            pi.arguments = "log";

            return pi;
        }



    }
}