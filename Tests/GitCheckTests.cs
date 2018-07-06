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
        List<string> testDomainList;
        public GitCheckTests()
        {
            testString = "commit fd891f3dbcf0fd935814b56f87b2e0f768fe5bef (HEAD -> i8-gitcheck)\n" +
                "\nAuthor: matas.zilaitis < matas.zilaitis@gmail.com > " +
                "\n Date:   Thu Jul 5 16:57:15 2018 + 0100 " +
                "\nChanged the class logic a bit thanks to Mike";
            testLastAuthor = "Author: matas.zilaitis < matas.zilaitis@gmail.com > ";
            gc = new GitCheck();

            testDomainList = new List<string>();
            testDomainList.Add("@nonlinear.com");
            testDomainList.Add("@nonlinear.com");

        }


        [Fact]
        public void Test_GetLastAuthor()
        {
            testOutput = "Author: matas.zilaitis < matas.zilaitis@gmail.com > ";
            testOutput.Should().BeEquivalentTo(gc.getLastAuthor(testString));
            
        }


        [Fact]
        public void Test_GitHasBeenUsed()
        {
            bool testBool = true;
            testBool.Should().Be(gc.GitHasBeenUsed(testLastAuthor,gc.getHostDomains()));
            
        }

        [Fact] 
        public void Test_ContainsAny()
        {
            string testString = "Author: matas.zilaitis < matas.zilaitis@gmail.com > ";
            false.Should().Be(GitCheck.ContainsAny(testString,gc.getHostDomains()));

        }

        [Fact]
        public void Test_ContainsAll()
        {
            string testString = "Author: matas.zilaitis < matas.zilaitis@gmail.com > ";
            true.Should().Be(GitCheck.ContainsAll(testString, gc.getKeyWords()));

        }
    }
}
