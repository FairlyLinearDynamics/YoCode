using System;
using System.Collections.Generic;
using System.Text;

namespace YoCode
{
    class ProjectBuilder : ProcessRunner
    {
        public ProjectBuilder(string processName, string workingDir, string arguments) : base(processName, workingDir, arguments)
        {
        }
    }
}
