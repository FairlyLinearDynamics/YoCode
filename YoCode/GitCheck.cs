using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace YoCode
{
    class GitCheck
    {
        StringReader sr;
        public ProcessStartInfo psi { get; set; }
        public Process process { get; set; }
        public string GIT_INSTALL_DIRECTORY { get; set; } = @"C:\Program Files\Git";
        public string REPOSITORY_PATH = @"..\..\..\..\..\YoCode";
        public List<string> hostDomains = new List<string>();


        public GitCheck()
        {
            hostDomains.Add("@nonlinear.com");
            hostDomains.Add("@waters.com");
        }

        public bool executeTheCheck()
        {
            setProcessStartInfo(REPOSITORY_PATH);
            openProcess();
            string lastAuthor = getLastAuthor(getOutput());
            return gitHasBeenUsed(lastAuthor,hostDomains);
        }

        // Refactor GIT_INSTALL_DIRECTORY / ARGUMENTS?

        public void setProcessStartInfo(String PATH)
        {
            psi = new ProcessStartInfo();
            psi.CreateNoWindow = true;
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;
            psi.WorkingDirectory = PATH;
            psi.FileName = GIT_INSTALL_DIRECTORY + @"\bin\git.exe";
            psi.Arguments = "log ";
        }


        public void openProcess()
        {
            process = new Process();
            process.StartInfo = psi;
            process.Start();
        }

        public string getOutput()
        {
            return process.StandardOutput.ReadToEnd();
        }

        public void printOutput()
        {
            Console.Write(getOutput());
        }

        // probably will delete this
        private List<String> getAuthorList(string output)
        {
            sr = new StringReader(output);
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
            sr = new StringReader(output);
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
