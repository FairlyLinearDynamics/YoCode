using System.Collections.Generic;
using System.Text;

namespace YoCode
{
    internal class SimpleEvidenceBuilder : IEvidence
    {
        List<string> evidences;

        public SimpleEvidenceBuilder(List<string> evidences)
        {
            this.evidences = evidences;
        }

        public SimpleEvidenceBuilder(string evidence) : this(new List<string>() { evidence }) { }

        public string BuildEvidenceForConsole()
        {
            var evidenceSB = new StringBuilder();
            evidences.ForEach(a=>evidenceSB.AppendLine(a));
            return evidenceSB.ToString();
        }

        public string BuildEvidenceForHTML()
        {
            return WebElementBuilder.FormatListOfStrings(evidences);
        }
    }
}
