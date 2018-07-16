using System;
using System.IO;
using System.Security.Cryptography;
using System.Linq;
using System.Diagnostics;

namespace YoCode
{
    public class FileChangeChecker
    {
        IPathManager directory;

        // TODO: Fix bugs #46
        public FileChangeChecker(IPathManager dir)
        {
            FileChangeEvidence.FeatureTitle = "Files changed";
            directory = dir;
            ProjectIsModified();
        }

        private void ProjectIsModified()
        {

            var originalFileStreams = directory.ReturnOriginalPathFileStream();
            var modifiedFileStreams = directory.ReturnModifiedPathFileStream();

            try
            {
                foreach (var modified in modifiedFileStreams)
                {

                    var similar = originalFileStreams.Where(a => Path.GetRelativePath(directory.originalTestDirPath,a.path).Equals(
                        Path.GetRelativePath(directory.modifiedTestDirPath,modified.path)));

                    if (similar.Count() != 0)
                    {
                        if (FileIsModified(similar.First().content, modified.content))
                        {
                            FileChangeEvidence.GiveEvidence($"Changed file: \\{new DirectoryInfo(modified.path).Parent.Name}\\{Path.GetFileName(modified.path)}");
                        }
                    }
                    else
                    {
                        FileChangeEvidence.GiveEvidence($"Added file: \\{new DirectoryInfo(modified.path).Parent.Name}\\{Path.GetFileName(modified.path)}");
                    }
                }

                if (FileChangeEvidence.EvidencePresent)
                {
                    FileChangeEvidence.FeatureImplemented = true;
                }
            }
            finally
            {
                foreach (var o in originalFileStreams) o.content.Dispose();
                foreach (var m in modifiedFileStreams) m.content.Dispose();
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


