using Xunit;
using FluentAssertions;
using YoCode;
using System.Collections.Generic;
using System;
using Moq;

namespace YoCode_XUnit
{
    public class PathManagerTests
    {
        [Fact]
        public void GetFilesInDirectoryCorrectlyReturnsListOfFiles()
        {
            Mock<IPathManager> mock = new Mock<IPathManager>();
            var fakeDir = mock.Object;

            string fakeList1 = @"\";
            string fakeList2 = @"\";

            PathManager dir = new PathManager(fakeList1, fakeList2);

            string path = @"..\..\..\TestData\";

            List<string> result = new List<string>();

            result = (List<string>)dir.GetFilesInDirectory(path, FileTypes.html);

            result.Count.Should().Be(5);
        }
    }
}
