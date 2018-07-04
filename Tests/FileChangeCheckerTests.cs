using Xunit;
using FluentAssertions;
using System.IO;
using YoCode;
using System.Text;
using Moq;
using System.Collections.Generic;
using System;

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
        [Fact]
        public void CheckIfProjectIsModifiedOutputsCorrectValue()
        {
            var mock = new Mock<IDirectory>();

            var fakeDirectory = mock.Object;

            List<String> fakePaths = new List<String>() { "hi", "hello" };

            List<Stream> fakeList = new List<Stream>();

            List<Stream> fakeList2 = new List<Stream>();

            for (int i = 0; i < fakePaths.Count; i++)
            {
                var fakeStream = new MemoryStream();
                fakeStream.Write(Encoding.ASCII.GetBytes("thing " + i + "that gets hashed"));
                fakeStream.Position = 0;
                fakeList2.Add(fakeStream);

            }
            for (int i = 0; i < fakePaths.Count; i++)
            {
                var fakeStream = new MemoryStream();
                fakeStream.Write(Encoding.ASCII.GetBytes("thing " + i + "that gets hashed"));
                fakeStream.Position = 0;
                fakeList.Add(fakeStream);

            }
            mock.Setup(w => w.OriginalPaths).Returns(fakePaths);
            mock.Setup(w => w.ModifiedPaths).Returns(fakePaths);


            mock.Setup(w => w.ReturnOriginalPathFileStream()).Returns(fakeList);
            mock.Setup(w => w.ReturnModifiedPathFileStream()).Returns(fakeList2);

            FileChangeChecker.ProjectIsModified(fakeDirectory).Should().Be(false);
        }
    }
}
