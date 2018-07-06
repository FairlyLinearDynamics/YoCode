using System.Collections.Generic;
using System.Linq;

namespace YoCode
{
    public class TestResults
    {
        public bool AnyFileChanged { get; set; }

        public bool UiCheck => Lines.Any();
        public List<int> Lines { get; set; }

        public bool GitUsed { get; set; }

        public bool SolutionFileExist { get; set; }

    }
}