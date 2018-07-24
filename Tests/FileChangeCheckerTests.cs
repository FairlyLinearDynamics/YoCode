using Xunit;
using Xunit.Abstractions;
using FluentAssertions;
using System.IO;
using YoCode;
using System.Text;
using Moq;
using System.Collections.Generic;

namespace YoCode_XUnit
{
    public class FileChangeCheckerTests
    {

        Mock<IPathManager> mock = new Mock<IPathManager>();

        List<string> fakePaths1 = new List<string>() { @"\one", @"\two" };
        List<string> fakePaths2 = new List<string>() { @"\one", @"\two", @"\three" };

        string fakeOriginal = @"C:\Users\ukekar\Downloads\junior-test";
        string fakeModified = @"C:\Users\ukekar\Downloads\drew-gibbon";

        IPathManager fakeDir;

        public FileChangeCheckerTests()
        {
            fakeDir = mock.Object;

            mock.Setup(w => w.originalTestDirPath).Returns(fakeOriginal);
            mock.Setup(w => w.modifiedTestDirPath).Returns(fakeModified);
        }

        private FileContent CreateFakeStream(int i)
        {
            var fakeStream = new MemoryStream();
            fakeStream.Write(Encoding.ASCII.GetBytes("thing "+ i +" that gets hashed"));
            fakeStream.Position = 0;
            return new FileContent { path = "thing " + i + " that gets hashed", content = fakeStream };
        }

        private List<FileContent> FakeListOfFileContent(int listLength)
        {
            var fileContent = new List<FileContent>();
            for(int i=0; i<listLength; ++i)
            {
                fileContent.Add(CreateFakeStream(i));
            }
            return fileContent;
        }

        [Fact]
        public void FileChangeChecker_ProjectModified()
        {
            mock.Setup(w => w.OriginalPaths).Returns(fakePaths1);
            mock.Setup(w => w.ModifiedPaths).Returns(fakePaths1);

            mock.Setup(w => w.ReturnOriginalPathFileStream()).Returns(FakeListOfFileContent(fakePaths1.Count));
            mock.Setup(w => w.ReturnModifiedPathFileStream()).Returns(FakeListOfFileContent(fakePaths2.Count));

            new FileChangeChecker(fakeDir).FileChangeEvidence.FeatureImplemented.Should().BeTrue();
        }

        [Fact]
        public void ProjectIsModifiedWithDifferentFileOrder()
        {
            var fakeFileContents = FakeListOfFileContent(fakePaths1.Count);
            var revertedFakeFileContents = FakeListOfFileContent(fakePaths1.Count);
            revertedFakeFileContents.Reverse();

            mock.Setup(w => w.OriginalPaths).Returns(fakePaths1);
            mock.Setup(w => w.ModifiedPaths).Returns(fakePaths1);

            mock.Setup(w => w.ReturnOriginalPathFileStream()).Returns(fakeFileContents);
            mock.Setup(w => w.ReturnModifiedPathFileStream()).Returns(revertedFakeFileContents);

            new FileChangeChecker(fakeDir).FileChangeEvidence.FeatureImplemented.Should().BeFalse();

        }
    }
}
