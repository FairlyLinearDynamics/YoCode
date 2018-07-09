using Xunit;
using FluentAssertions;
using YoCode;
using System.Collections.Generic;
using System;

namespace YoCode_XUnit
{
    public class PathManagerTests
    {
        [Fact]
        public void GetFilesInDirectoryCorrectlyReturnsListOfFiles()
        {
            List<string> fakeList1 = new List<string>();
            List<string> fakeList2 = new List<string>();

            PathManager dir = new PathManager(fakeList1, fakeList2);

            string path = @"..\..\..\TestData\";

            List<string> result = new List<string>();

            result = (List<string>)dir.GetFilesInDirectory(path, FileTypes.html);

            result.Count.Should().Be(5);
        }
    }
}
