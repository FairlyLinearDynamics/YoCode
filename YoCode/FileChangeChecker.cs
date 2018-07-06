﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Linq;

namespace YoCode
{
    public class FileChangeChecker
    {
        public static bool ProjectIsModified(IDirectory dir)
        {
            if (dir.OriginalPaths.Count() != dir.ModifiedPaths.Count())
            {
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
                            return true;
                        }
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
        public static bool FileIsModified(Stream originalFile, Stream modifiedFile)
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

