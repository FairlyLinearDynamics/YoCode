using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace YoCode
{
    public class FileChangeChecker
    {
        public static bool ProjectIsModified(IDirectory dir)
        {


            if (dir.OriginalPaths.Count != dir.ModifiedPaths.Count)
            {
                return true;
            }
            else
            {
                List<Stream> originalFileStreams = dir.ReturnOriginalPathFileStream();
                List<Stream> modifiedFileStreams = dir.ReturnModifiedPathFileStream();

                for (int i = 0; i < modifiedFileStreams.Count; i++)
                {
                    if (FileIsModified(originalFileStreams[i], modifiedFileStreams[i]))
                    {
                        return true;
                    }
                }
                return false;
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


