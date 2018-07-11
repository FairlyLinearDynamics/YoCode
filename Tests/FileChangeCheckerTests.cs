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

        Mock<IPathManager> mock = new Mock<IPathManager>();

        List<string> fakePaths1 = new List<string>() { "one", "two" };

        List<string> fakePaths2 = new List<string>() { "one", "two", "three" };

        private FileContent CreateFakeStream(int i)
        {
            var fakeStream = new MemoryStream();
            fakeStream.Write(Encoding.ASCII.GetBytes("thing "+ i +" that gets hashed"));
            fakeStream.Position = 0;
            return new FileContent { path = "thing " + i + " that gets hashed", content = fakeStream };
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

            new FileChangeChecker(fakeDirectory).FileChangeEvidence.EvidencePresent.Should().BeFalse();
        }

        [Fact]
        public void ProjectIsModifiedCorrectlyFindsDifferentListLengths()
        {
            var fakeDir = mock.Object;

            mock.Setup(w => w.OriginalPaths).Returns(fakePaths1);
            mock.Setup(w => w.ModifiedPaths).Returns(fakePaths2);

            new FileChangeChecker(fakeDir).FileChangeEvidence.FeatureImplemented.Should().BeTrue();
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

            new FileChangeChecker(fakeDir).FileChangeEvidence.EvidencePresent.Should().BeFalse();

        }

        [Fact]
        public void FileChangeChecker_FeatureEvidence_EvidencePresentFieldTrue()
        {
            var fakeDirectory = mock.Object;

            List<FileContent> fakeList = new List<FileContent>();

            List<FileContent> fakeList2 = new List<FileContent>();

            for (int i = 0; i < fakePaths1.Count; i++)
            {
                fakeList.Add(CreateFakeStream(i));
                fakeList2.Add(CreateFakeStream(i));

            }
            fakeList2.Add(CreateFakeStream(10));
            mock.Setup(w => w.OriginalPaths).Returns(fakePaths1);
            mock.Setup(w => w.ModifiedPaths).Returns(fakePaths1);

            fakeList2.RemoveAt(0);

            mock.Setup(w => w.ReturnOriginalPathFileStream()).Returns(fakeList);
            mock.Setup(w => w.ReturnModifiedPathFileStream()).Returns(fakeList2);

            var fileCheck = new FileChangeChecker(fakeDirectory);
            fileCheck.FileChangeEvidence.EvidencePresent.Should().Be(true);
        }

        [Fact]
        public void FileChangeChecker_FeatureEvidence_EvidenceFieldTrue()
        {
            var fakeDirectory = mock.Object;

            List<FileContent> fakeList = new List<FileContent>();

            List<FileContent> fakeList2 = new List<FileContent>();

            for (int i = 0; i < fakePaths1.Count; i++)
            {
                fakeList.Add(CreateFakeStream(i));
                fakeList2.Add(CreateFakeStream(i));

            }
            fakeList2.Add(CreateFakeStream(10));
            mock.Setup(w => w.OriginalPaths).Returns(fakePaths1);
            mock.Setup(w => w.ModifiedPaths).Returns(fakePaths1);

            fakeList2.RemoveAt(0);

            mock.Setup(w => w.ReturnOriginalPathFileStream()).Returns(fakeList);
            mock.Setup(w => w.ReturnModifiedPathFileStream()).Returns(fakeList2);

            var fileCheck = new FileChangeChecker(fakeDirectory);
            fileCheck.FileChangeEvidence.Evidence.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void FileChangeChecker_FeatureEvidence_FeatureImplementedFieldTrue()
        {
            var fakeDir = mock.Object;

            mock.Setup(w => w.OriginalPaths).Returns(fakePaths1);
            mock.Setup(w => w.ModifiedPaths).Returns(fakePaths2);

            var fileCheck = new FileChangeChecker(fakeDir);
            fileCheck.FileChangeEvidence.FeatureImplemented.Should().Be(true);
        }
    }
}
