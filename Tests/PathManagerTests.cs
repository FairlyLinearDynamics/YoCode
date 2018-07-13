using Xunit;
using FluentAssertions;
using YoCode;
using System.Collections.Generic;
using System;

namespace YoCode_XUnit
{
    public class PathManagerTests
    {
        // TODO: fix test, need to somehow mock file path instead of passing null
        [Fact]
        public void GetFilesInDirectoryCorrectlyReturnsListOfFiles()
        {
            string fakeList1 = null;
            string fakeList2 = null;

            PathManager dir = new PathManager(fakeList1, fakeList2);

            string path = @"..\..\..\TestData\";

            List<string> result = new List<string>();

            result = (List<string>)dir.GetFilesInDirectory(path, FileTypes.html);

            result.Count.Should().Be(5);
        }
    }
}
