using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace YoCode
{
    public class FileChangeChecker
    {
        public static bool ProjectIsModified(Directory dir)
        {
            if (dir.OriginalPaths.Count != dir.ModifiedPaths.Count)
            {
                return true;
            }
            return false;
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


