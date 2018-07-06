using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace YoCode
{
    public class GitCheck
    {
        //needs to be fixed
        public string REPOSITORY_PATH = @"C:\Users\ukekar\Downloads\drew-gibbon";

        public string Output { get; set; }
        public string LastAuthor { get; set; }     
        public bool GitUsed { get; set; }

        public List<string> hostDomains = new List<string>();

        public bool ExecuteTheCheck()
        {
            var p = new Process();
            return ExecuteTheCheck(p);
        }

        public bool ExecuteTheCheck(Process p)
        {
            p.StartInfo = SetProcessStartInfo(REPOSITORY_PATH);
            p.Start();
            Output = p.StandardOutput.ReadToEnd();
            LastAuthor = getLastAuthor(Output);
            GitUsed = GitHasBeenUsed(LastAuthor,getHostDomains());
            return GitUsed;
        }

        public ProcessStartInfo SetProcessStartInfo(String PATH)
        {
            var psi = new ProcessStartInfo();
            psi.CreateNoWindow = true;
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;
            psi.WorkingDirectory = PATH;
            psi.FileName = "git.exe";
            psi.Arguments = "log ";

            return psi;
        }

        // get name and email address of the last author
        public string getLastAuthor(string output)
        {
            var sr = new StringReader(output);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (ContainsAll(line,getKeyWords() ) )
                {
                    return line;
                }
            }
            return "";
        }


        public bool GitHasBeenUsed(string lastAuthor, List<string> hostDomains)
        {
            if (ContainsAny(lastAuthor,getHostDomains()) || string.IsNullOrEmpty(lastAuthor) )
            {
                return false;
            }
            return true;
        }
    
        public static bool ContainsAny(string line,List<string> keywords)
        {
            foreach(string keyword in keywords)
            {
                if (line.Contains(keyword))
                {
                    return true;
                }
            }
            return false;
    
        }

        public static bool ContainsAll(string line, List<string> keywords)
        {
            foreach (string keyword in keywords)
            {
                if (line.Contains(keyword))
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public List<String> getKeyWords()
        {
            var keywords = new List<string>();
            keywords.Add("Author:");
            keywords.Add("<");
            keywords.Add(">");
            keywords.Add("@");
            keywords.Add(".");

            return keywords;
        }

        public List<String> getHostDomains()
        {
            var hostDomains = new List<string>();
            hostDomains.Add("@nonlinear.com");
            hostDomains.Add("@waters.com");

            return hostDomains;
        }       
    }
}
