using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace YoCode
{
    public class GitCheck
    {
        public string GIT_INSTALL_DIRECTORY { get; set; } = @"C:\Program Files\Git";
        //public string REPOSITORY_PATH = @"..\..\..\..\..\YoCode";
        public string REPOSITORY_PATH = @"C:\Users\ukmzil\source\repos\Tests Sent by People\Real\jacob-millward";

        public List<string> hostDomains = new List<string>();


        public GitCheck()
        {
        }

        public bool ExecuteTheCheck()
        {
            var p = new Process();
            return ExecuteTheCheck(p);
        }

        public bool ExecuteTheCheck(Process p)
        {
            p.StartInfo = SetProcessStartInfo(REPOSITORY_PATH);
            p.Start();

            string output = p.StandardOutput.ReadToEnd();
            string lastAuthor = getLastAuthor(output);
            Console.WriteLine(lastAuthor);
            return GitHasBeenUsed(lastAuthor,hostDomains);
        }

        public ProcessStartInfo SetProcessStartInfo(String PATH)
        {
            var psi = new ProcessStartInfo();
            psi.CreateNoWindow = true;
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;
            psi.WorkingDirectory = PATH;
            psi.FileName = GIT_INSTALL_DIRECTORY + @"\bin\git.exe";
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
                if (ContainsAny(line,getKeyWords() ) )
                {
                    return line;
                }
            }
            return "No commits/authors found";
        }

        public bool GitHasBeenUsed(string lastAuthor, List<string> hostDomains)
        {
            if (ContainsAny(lastAuthor,hostDomains))
            {
                return false;
            }
            return true;
        }
    
        public static bool ContainsAny( string line,List<string> keywords)
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

        public List<String> getKeyWords()
        {
            var keywords = new List<string>();
            keywords.Add("<");
            keywords.Add(">");
            keywords.Add("@");
            keywords.Add("Author");
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
