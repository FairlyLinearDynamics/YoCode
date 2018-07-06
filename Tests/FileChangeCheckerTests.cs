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

        Mock<IDirectory> mock = new Mock<IDirectory>();

        List<String> fakePaths1 = new List<String>() { "one", "two" };

        List<String> fakePaths2 = new List<String>() { "one", "two", "three" };

        private FileContent CreateFakeStream(int i)
        {
            var fakeStream = new MemoryStream();
            fakeStream.Write(Encoding.ASCII.GetBytes("thing "+ i +" that gets hashed"));
            fakeStream.Position = 0;
            return new FileContent { path = "thing " + i + " that gets hashed", content = fakeStream };
        }

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
            var fakeDirectory = mock.Object;

            List<FileContent> fakeList = new List<FileContent>();

            List<FileContent> fakeList2 = new List<FileContent>();

            for (int i = 0; i < fakePaths1.Count; i++)
            {
                fakeList.Add(CreateFakeStream(i));
                fakeList2.Add(CreateFakeStream(i));

            }           
            mock.Setup(w => w.OriginalPaths).Returns(fakePaths1);
            mock.Setup(w => w.ModifiedPaths).Returns(fakePaths1);

            mock.Setup(w => w.ReturnOriginalPathFileStream()).Returns(fakeList);
            mock.Setup(w => w.ReturnModifiedPathFileStream()).Returns(fakeList2);

            FileChangeChecker.ProjectIsModified(fakeDirectory).Should().Be(false);
        }

        [Fact]
        public void ProjectIsModifiedCorrectlyFindsDifferentListLengths()
        {
            var fakeDir = mock.Object;

            mock.Setup(w => w.OriginalPaths).Returns(fakePaths1);
            mock.Setup(w => w.ModifiedPaths).Returns(fakePaths2);

            FileChangeChecker.ProjectIsModified(fakeDir).Should().Be(true);
        }

        [Fact]
        public void ProjectIsModifiedWithDifferentFileOrder()
        {
            var fakeDir = mock.Object;

            List<FileContent> reverseFakeList = new List<FileContent>();

            List<FileContent> fakeList = new List<FileContent>();

           
            for (int i = fakePaths1.Count - 1; i >= 0; i--)
            {
                reverseFakeList.Add(CreateFakeStream(i));
            }

            for (int i = 0; i < fakePaths1.Count; i++)
            {
                fakeList.Add(CreateFakeStream(i));
            }

            mock.Setup(w => w.OriginalPaths).Returns(fakePaths1);
            mock.Setup(w => w.ModifiedPaths).Returns(fakePaths1);

            mock.Setup(w => w.ReturnOriginalPathFileStream()).Returns(fakeList);
            mock.Setup(w => w.ReturnModifiedPathFileStream()).Returns(reverseFakeList);

            FileChangeChecker.ProjectIsModified(fakeDir).Should().Be(false);

        }
    }
}
