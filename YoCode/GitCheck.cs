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

        public static bool ContainsAll(this string line, IEnumerable<string> keywords)
        {
            return keywords.All(line.Contains);
        }
    }

    public class GitCheck
    {
        private readonly string repositoryPath;

        private string Output { get; set; } = "No output found";
        private string LastAuthor { get; set; } = "None";
        public bool GitUsed { get; private set; }

        public GitCheck(string path)
        {
                repositoryPath = path;
                ExecuteTheCheck();
        }

        public void ExecuteTheCheck()
        {

            ProcessRunner pr = new ProcessRunner("git",repositoryPath, "log");
            // ProcessRunner pr = new ProcessRunner("dotnet", repositoryPath, "test");

            pr.ExecuteTheCheck();

            Output = pr.Output;
            LastAuthor = GetLastAuthor(Output);
            GitUsed = GitHasBeenUsed(LastAuthor, GetHostDomains());
        }


        public static string GetLastAuthor(string output)
        {
            var sr = new StringReader(output);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if(line.ContainsAll(GetKeyWords()))
                {
                    return line;
                }
            }
            return "";
        }

        public static bool GitHasBeenUsed(string lastAuthor, List<string> hostDomains)
        {
            if (lastAuthor.ContainsAny(GetHostDomains()) || string.IsNullOrEmpty(lastAuthor))
            {
                return false;
            }
            return true;
        }

        public static IEnumerable<string> GetKeyWords()
        {
            return new List<string> { "Author:", "<", ">", "@", "." };
        }

        public static List<string> GetHostDomains()
        {
            return new List<string> { "@nonlinear.com", "@waters.com" };
        }

        public ProcessInfo SetupProcessInfo(string processName,string workingDir,string arguments)
        {
            ProcessInfo pi;
            pi.processName = processName;
            pi.workingDir = repositoryPath;
            pi.arguments = arguments;

            return pi;
        }
    }
}