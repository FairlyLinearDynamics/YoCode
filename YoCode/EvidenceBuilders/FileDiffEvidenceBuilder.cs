using LibGit2Sharp;
using System;
using System.Linq;
using System.Text;

namespace YoCode
{
    internal class FileDiffEvidenceBuilder : IEvidence
    {
        private Patch diffs;
        private string otherChanges;

        public FileDiffEvidenceBuilder(Patch diffs, string otherChanges)
        {
            this.diffs = diffs;
            this.otherChanges = otherChanges;
        }

        public string BuildEvidenceForConsole()
        {
            var evidence = new StringBuilder();
            foreach (var diff in diffs)
            {
                var lineDifference = diff.LinesAdded + diff.LinesDeleted;
                evidence.AppendLine($"{diff.Status} : {diff.Path} = " +
                            $"{lineDifference} ({diff.LinesAdded}+ and {diff.LinesDeleted}-)");
            }
            evidence.AppendLine(otherChanges);
            return evidence.ToString();
        }

        public string BuildEvidenceForHTML()
        {
            var evidence = new StringBuilder();
            foreach (var diff in diffs)
            {
                var lineDifference = diff.LinesAdded + diff.LinesDeleted;
                evidence.AppendLine(WebElementBuilder.FormatParagraph($"{diff.Status} : {WebElementBuilder.FormatFileDiffButton(diff.Path)} = " +
                            $"{lineDifference} ({diff.LinesAdded}+ and {diff.LinesDeleted}-)"));

                evidence.AppendLine(WebElementBuilder.FormatFileDiff(diff.Patch.Split("\n").ToList()));
            }
            evidence.AppendLine(WebElementBuilder.FormatParagraph(otherChanges));
            return evidence.ToString();
        }
    }
}
