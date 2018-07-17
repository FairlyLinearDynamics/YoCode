using Xunit;
using FluentAssertions;
using YoCode;

namespace YoCode_XUnit
{
    public class GitCheckTests
    {
        private readonly string testLastAuthor;

        public GitCheckTests()
        {
            testLastAuthor = "Author: matas.zilaitis < matas.zilaitis@gmail.com > ";
        }

        [Fact]
        public void Test_GitHasBeenUsed()
        {
            GitCheck.GitHasBeenUsed(testLastAuthor).Should().Be(true);

        }

    }
}
