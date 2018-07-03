using Xunit;
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
}
