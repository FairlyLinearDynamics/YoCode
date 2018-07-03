using System;
using System.IO;

namespace YoCode
{
    public static class FileChangeChecker
    {
        public static Boolean IsModified (FileStream originalFile, FileStream modifiedFile)
        {
            string originalChecksum = BitConverter.ToString(System.Security.Cryptography.SHA1.Create().ComputeHash(originalFile));

            string modifiedCheckSum = BitConverter.ToString(System.Security.Cryptography.SHA1.Create().ComputeHash(modifiedFile));

            return originalChecksum == modifiedCheckSum ? false : true;
        }
    }
}
