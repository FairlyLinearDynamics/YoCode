using Xunit;
using FluentAssertions;
using YoCode;

namespace YoCode_XUnit
{
    public class GitCheckTests
    {
        private readonly string testString;
        private readonly string testLastAuthor;

        public GitCheckTests()
        {
            testString = "commit fd891f3dbcf0fd935814b56f87b2e0f768fe5bef (HEAD -> i8-gitcheck)\n" +
                "\nAuthor: matas.zilaitis < matas.zilaitis@gmail.com > " +
                "\n Date:   Thu Jul 5 16:57:15 2018 + 0100 " +
                "\nChanged the class logic a bit thanks to Mike";
            testLastAuthor = "Author: matas.zilaitis < matas.zilaitis@gmail.com > ";
        }


        [Fact]
        public void Test_GetLastAuthor()
        {
            GitCheck.GetLastAuthor(testString).Should().BeEquivalentTo(testLastAuthor);
        }


        [Fact]
        public void Test_GitHasBeenUsed()
        {
            GitCheck.GitHasBeenUsed(testLastAuthor, GitCheck.GetHostDomains()).Should().Be(true);

        }

        [Fact] 
        public void Test_ContainsAny()
        {
            GitCheck.ContainsAny(testLastAuthor, GitCheck.GetHostDomains()).Should().Be(false);
        }

        [Fact]
        public void Test_ContainsAll()
        {
            GitCheck.ContainsAll(testLastAuthor, GitCheck.GetKeyWords()).Should().Be(true);
        }
    }
}
