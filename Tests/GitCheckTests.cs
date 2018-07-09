using System;
using Xunit;
using FluentAssertions;
using YoCode;
using System.Collections.Generic;

namespace YoCode_XUnit
{
    public class GitCheckTests
    {
        string testOutput,testString,testLastAuthor;
        GitCheck gc;

        public GitCheckTests()
        {
            testString = "commit fd891f3dbcf0fd935814b56f87b2e0f768fe5bef (HEAD -> i8-gitcheck)\n" +
                "\nAuthor: matas.zilaitis < matas.zilaitis@gmail.com > " +
                "\n Date:   Thu Jul 5 16:57:15 2018 + 0100 " +
                "\nChanged the class logic a bit thanks to Mike";
            testLastAuthor = "Author: matas.zilaitis < matas.zilaitis@gmail.com > ";
            gc = new GitCheck(@"C: \Users\ukmzil\source\repos\Tests Sent by People\Real\drew - gibbon");

        }


        [Fact]
        public void Test_GetLastAuthor()
        {
            gc.getLastAuthor(testString).Should().BeEquivalentTo(testLastAuthor);
        }


        [Fact]
        public void Test_GitHasBeenUsed()
        {
            gc.GitHasBeenUsed(testLastAuthor, gc.getHostDomains()).Should().Be(true);

        }

        [Fact] 
        public void Test_ContainsAny()
        {
            GitCheck.ContainsAny(testLastAuthor, gc.getHostDomains()).Should().Be(false);


        }

        [Fact]
        public void Test_ContainsAll()
        {
            (GitCheck.ContainsAll(testLastAuthor, gc.getKeyWords())).Should().Be(true);
        }
    }
}
