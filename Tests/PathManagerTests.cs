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
        // TODO: fix test, need to somehow mock file path instead of passing null
        [Fact]
        public void GetFilesInDirectoryCorrectlyReturnsListOfFiles()
        {
            Mock<IPathManager> mock = new Mock<IPathManager>();
            var fakeDir = mock.Object;

            //mock.Setup(w=>w.);
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
