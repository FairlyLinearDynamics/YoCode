using Xunit;
using Xunit.Abstractions;
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

        string fakeOriginal = @"C:\Users\ukekar\Downloads\junior-test";
        string fakeModified = @"C:\Users\ukekar\Downloads\drew-gibbon";

        IPathManager fakeDir;
        List<FileContent> fakeList1;
        List<FileContent> fakeList2;

        private readonly ITestOutputHelper _output;

        public FileChangeCheckerTests(ITestOutputHelper output)
        {
            _output = output;

            fakeDir = mock.Object;

            mock.Setup(w => w.originalTestDirPath).Returns(fakeOriginal);
            mock.Setup(w => w.modifiedTestDirPath).Returns(fakeModified);

            fakeList1 = new List<FileContent>();
            fakeList2 = new List<FileContent>();
        }

        private FileContent CreateFakeStream(int i)
        {
            var fakeStream = new MemoryStream();
            fakeStream.Write(Encoding.ASCII.GetBytes("thing "+ i +" that gets hashed"));
            fakeStream.Position = 0;
            return new FileContent { path = "thing " + i + " that gets hashed", content = fakeStream };
        }

        [Fact]
        public void FileChangeChecker_ProjectUnmodifiedCheck()
        {
            for (int i = 0; i < fakePaths1.Count; i++)
            {
                fakeList1.Add(CreateFakeStream(i));
                fakeList2.Add(CreateFakeStream(i));

            }           
            mock.Setup(w => w.OriginalPaths).Returns(fakePaths1);
            mock.Setup(w => w.ModifiedPaths).Returns(fakePaths1);

            mock.Setup(w => w.ReturnOriginalPathFileStream()).Returns(fakeList1);
            mock.Setup(w => w.ReturnModifiedPathFileStream()).Returns(fakeList2);

            new FileChangeChecker(fakeDir).FileChangeEvidence.FeatureImplemented.Should().BeFalse();
        }

        [Fact]
        public void ProjectIsModifiedWithDifferentFileOrder()
        {
            for (int i = fakePaths1.Count - 1; i >= 0; i--)
            {
                fakeList2.Add(CreateFakeStream(i));
            }

            for (int i = 0; i < fakePaths1.Count; i++)
            {
                fakeList1.Add(CreateFakeStream(i));
            }

            mock.Setup(w => w.OriginalPaths).Returns(fakePaths1);
            mock.Setup(w => w.ModifiedPaths).Returns(fakePaths1);

            mock.Setup(w => w.ReturnOriginalPathFileStream()).Returns(fakeList1);
            mock.Setup(w => w.ReturnModifiedPathFileStream()).Returns(fakeList2);

            new FileChangeChecker(fakeDir).FileChangeEvidence.FeatureImplemented.Should().BeFalse();

        }

        [Fact]
        public void FileChangeChecker_FeatureEvidence_EvidencePresentFieldTrue()
        {

            for (int i = 0; i < fakePaths1.Count; i++)
            {
                fakeList1.Add(CreateFakeStream(i));
                fakeList2.Add(CreateFakeStream(i));

            }
            fakeList2.Add(CreateFakeStream(10));
            mock.Setup(w => w.OriginalPaths).Returns(fakePaths1);
            mock.Setup(w => w.ModifiedPaths).Returns(fakePaths1);

            fakeList2.RemoveAt(0);

            mock.Setup(w => w.ReturnOriginalPathFileStream()).Returns(fakeList1);
            mock.Setup(w => w.ReturnModifiedPathFileStream()).Returns(fakeList2);

            var fileCheck = new FileChangeChecker(fakeDir);
            fileCheck.FileChangeEvidence.EvidencePresent.Should().Be(true);
        }

        [Fact]
        public void FileChangeChecker_FeatureEvidence_FeatureImplementedFieldTrue()
        {

            for (int i = 0; i < fakePaths1.Count; i++)
            {
                fakeList1.Add(CreateFakeStream(i));
                fakeList2.Add(CreateFakeStream(i));

            }
            fakeList2.Add(CreateFakeStream(10));
            mock.Setup(w => w.OriginalPaths).Returns(fakePaths1);
            mock.Setup(w => w.ModifiedPaths).Returns(fakePaths1);

            fakeList2.RemoveAt(0);

            mock.Setup(w => w.ReturnOriginalPathFileStream()).Returns(fakeList1);
            mock.Setup(w => w.ReturnModifiedPathFileStream()).Returns(fakeList2);

            var fileCheck = new FileChangeChecker(fakeDir);
            _output.WriteLine($"{fileCheck.FileChangeEvidence.EvidencePresent}");
            fileCheck.FileChangeEvidence.FeatureImplemented.Should().Be(true);
        }
    }
}
