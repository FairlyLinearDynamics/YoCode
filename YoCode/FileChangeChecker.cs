using System;
using System.IO;
using System.Security.Cryptography;
using System.Linq;

namespace YoCode
{
    public class FileChangeChecker
    {
        public static bool ProjectIsModified(IPathManager dir, FeatureEvidence evidenceList)
        {
            if (dir.OriginalPaths.Count() != dir.ModifiedPaths.Count())
            {
                evidenceList.FeatureImplemented = true;
                return true;
            }
            else
            {
                var originalFileStreams = dir.ReturnOriginalPathFileStream().OrderBy((c => c.path));
                var modifiedFileStreams = dir.ReturnModifiedPathFileStream().OrderBy((c => c.path));

                try
                {
                    var streamsList = originalFileStreams.Zip(modifiedFileStreams, (o, m) => (original: o, modified: m));

                    foreach (var (original, modified) in streamsList)
                    {
                        if (FileIsModified(original.content, modified.content))
                        {
                            evidenceList.GiveEvidence($"\\{new DirectoryInfo(modified.path).Parent.Name}\\{Path.GetFileName(modified.path)}");
                        }
                    }
                    if (evidenceList.EvidencePresent)
                    {
                        evidenceList.FeatureImplemented = true;
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
        private static bool FileIsModified(Stream originalFile, Stream modifiedFile)
        {
            using (var sha1 = SHA1.Create())
            {
                    string originalChecksum = BitConverter.ToString(sha1.ComputeHash(originalFile));

                    string modifiedCheckSum = BitConverter.ToString(sha1.ComputeHash(modifiedFile));

                    return originalChecksum == modifiedCheckSum ? false : true;
            }
        }
    }
}


