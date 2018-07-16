using System;
using System.IO;
using System.Security.Cryptography;
using System.Linq;

namespace YoCode
{
    public class FileChangeChecker
    {
        IPathManager directory;

        // TODO: Fix bugs #46 and #45
        public FileChangeChecker(IPathManager dir)
        {
            FileChangeEvidence.FeatureTitle = "Files changed";
            directory = dir;
            ProjectIsModified();
        }

        public bool ProjectIsModified()
        {
            if (directory.OriginalPaths.Count() != directory.ModifiedPaths.Count())
            {
                FileChangeEvidence.FeatureImplemented = true;
                return true;
            }
            else
            {
                var originalFileStreams = directory.ReturnOriginalPathFileStream().OrderBy((c => c.path));
                var modifiedFileStreams = directory.ReturnModifiedPathFileStream().OrderBy((c => c.path));

                try
                {
                    var streamsList = originalFileStreams.Zip(modifiedFileStreams, (o, m) => (original: o, modified: m));

                    foreach (var (original, modified) in streamsList)
                    {
                        if (FileIsModified(original.content, modified.content))
                        {
                            FileChangeEvidence.GiveEvidence($"\\{new DirectoryInfo(modified.path).Parent.Name}\\{Path.GetFileName(modified.path)}");
                        }
                    }
                    if (FileChangeEvidence.EvidencePresent)
                    {
                        FileChangeEvidence.FeatureImplemented = true;
                        return true;
                    }
                    return false;
                }
                finally
                {
                    foreach (var o in originalFileStreams) o.content.Dispose();
                    foreach (var m in modifiedFileStreams) m.content.Dispose();
                }

            }
        }
        private bool FileIsModified(Stream originalFile, Stream modifiedFile)
        {
            using (var sha1 = SHA1.Create())
            {
                    string originalChecksum = BitConverter.ToString(sha1.ComputeHash(originalFile));

                    string modifiedCheckSum = BitConverter.ToString(sha1.ComputeHash(modifiedFile));

                    return originalChecksum == modifiedCheckSum ? false : true;
            }
        }

        public FeatureEvidence FileChangeEvidence { get; private set; } = new FeatureEvidence();
    }
}


