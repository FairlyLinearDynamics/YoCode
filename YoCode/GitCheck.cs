using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace YoCode
{
    class GitCheck
    {

        public string GIT_INSTALL_DIRECTORY { get; set; } = @"C:\Program Files\Git";
        //public string REPOSITORY_PATH = @"..\..\..\..\..\YoCode";
        public string REPOSITORY_PATH = @"C:\Users\ukmzil\source\repos\Tests Sent by People\Real\jacob-millward";

        public List<string> hostDomains = new List<string>();


        public GitCheck()
        {
            hostDomains.Add("@nonlinear.com");
            hostDomains.Add("@waters.com");
            hostDomains.Add("@millward.io");

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
            return gitHasBeenUsed(lastAuthor,hostDomains);
        }

        // Refactor GIT_INSTALL_DIRECTORY / ARGUMENTS?

            // Change type to ProcessStartInfo#

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
        

        // probably will delete this
        private List<String> getAuthorList(string output)
        {
            var sr = new StringReader(output);
            List<String> authors = new List<String>();
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Contains("Author"))
                {
                    authors.Add(line);
                }
            }
            return authors;
        }

        // get name and email address of the last author
        private string getLastAuthor(string output)
        {
            var sr = new StringReader(output);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Contains("Author"))
                {
                    return line;
                }
            }
            return "No commits/authors found";
        }

        private bool gitHasBeenUsed(string lastAuthor, List<string> hostDomains)
        {
            foreach (string hostdomain in hostDomains)
            {
                if (lastAuthor.Contains(hostdomain))
                {
                    return false;
                }
            }
            return true;


        }



    }
}
