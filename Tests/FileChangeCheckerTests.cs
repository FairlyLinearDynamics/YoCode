﻿using Xunit;
using FluentAssertions;
using System.IO;
using YoCode;
using System.Text;

namespace YoCode_XUnit
{
    public class FileChangeCheckerTests
    {
        [Fact]
        public void CheckIfFileCheckerHashesCorrectly()
        {
            var original = new MemoryStream();
            original.Write(Encoding.ASCII.GetBytes("thing that gets hashed"));
            original.Position = 0;

            var modified = new MemoryStream();
            modified.Write(Encoding.ASCII.GetBytes("another thing that gets hashed"));
            modified.Position = 0;

            FileChangeChecker.FileIsModified(original, modified).Should().Be(true);

        }
    }

    using System;
using System.Collections.Generic;
using System.IO;

namespace YoCode
    {


        public class Directory : IDirectory
        {
            public List<String> OriginalPaths { get; }
            public List<String> ModifiedPaths { get; }

            public Directory(List<String> originalPaths, List<String> modifiedPaths)
            {
                OriginalPaths = originalPaths;
                ModifiedPaths = modifiedPaths;
            }

            private List<Stream> ReturnPathFileStream(List<string> paths)
            {
                List<Stream> streamList = new List<Stream>();

                for (int i = 0; i < paths.Count; i++)
                {
                    FileStream fs = File.Create(paths[i]);
                    streamList.Add(fs);
                }
                return streamList;
            }

            public List<Stream> ReturnOriginalPathFileStream()
            {
                return ReturnPathFileStream(OriginalPaths);
            }

            public List<Stream> ReturnModifiedPathFileStream()
            {
                return ReturnPathFileStream(ModifiedPaths);
            }
        }
    }
}
