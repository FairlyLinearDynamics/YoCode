using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using YoCode;
using FluentAssertions;

namespace YoCode_XUnit
{
    public class FileImportTests
    {
        public String testPATH;
        public FileImport fi;
        public List<String> testList;


        public FileImportTests()
        {

            testPATH = @"C:\Users\ukmzil\source\repos\sampledirectory";
            fi = new FileImport();
            testList = new List<String>();

        }



        [Fact]
        public void Test_GetAllFilesInDirectory()
        {
            testList.Add(@"C:\Users\ukmzil\source\repos\sampledirectory\1.txt");
            testList.Add(@"C:\Users\ukmzil\source\repos\sampledirectory\2.txt");
            testList.Add(@"C:\Users\ukmzil\source\repos\sampledirectory\3.txt");
            testList.Add(@"C:\Users\ukmzil\source\repos\sampledirectory\15.cs");
            testList.Add(@"C:\Users\ukmzil\source\repos\sampledirectory\25.cs");
            testList.Add(@"C:\Users\ukmzil\source\repos\sampledirectory\index.cshtml");


            testList.Should().BeEquivalentTo(fi.GetAllFilesInDirectory(testPATH));
        }

        [Fact]
        public void Test_GetFilesInDirectoryWithPattern()
        {
            String testPattern = "*.cs";
            testList.Add(@"C:\Users\ukmzil\source\repos\sampledirectory\15.cs");
            testList.Add(@"C:\Users\ukmzil\source\repos\sampledirectory\25.cs");

            testList.Should().BeEquivalentTo(fi.GetFilesInDirectory(testPATH, testPattern));

        }

        [Fact]
        public void Test_GetFilesInDirectoryWithListOfPatterns()
        {
            List<String> testPatterns = new List<string> { "*.cs", "*.cshtml" };
            testList.Add(@"C:\Users\ukmzil\source\repos\sampledirectory\15.cs");
            testList.Add(@"C:\Users\ukmzil\source\repos\sampledirectory\25.cs");
            testList.Add(@"C:\Users\ukmzil\source\repos\sampledirectory\index.cshtml");

            testList.Should().BeEquivalentTo(fi.GetFilesInDirectory(testPATH, testPatterns));

        }


    }
}


