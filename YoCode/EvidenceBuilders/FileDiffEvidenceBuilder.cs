using LibGit2Sharp;
using System;
using System.Linq;
using System.Text;

namespace YoCode
{
    internal class FileDiffEvidenceBuilder : IEvidence
    {
        private Patch diffs;
        private string bonusEvidence;

        public FileDiffEvidenceBuilder(Patch diffs, string otherChanges)
        {
            this.diffs = diffs;
            bonusEvidence = otherChanges;
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
            evidence.AppendLine(bonusEvidence);
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
            evidence.AppendLine(WebElementBuilder.FormatParagraph(this.bonusEvidence));
            return evidence.ToString();
        }
    }
}
