using Xunit;
using FluentAssertions;
using YoCode;
using System.Collections.Generic;

namespace YoCode_XUnit
{
    public class BackEndHelperFunctionsTests
    {

        public List<double> expectedConversionResults;
        public List<double> expectedConversionInputs;
        public double mult;

        private readonly List<string> expectedActionLines;
        private readonly List<string> expectedActionLinesResult;


        public BackEndHelperFunctionsTests()
        {

            expectedActionLines = new List<string>
            {
                    "      <input type=\"submit\" name=\"action\" value=\"Yards to meters\" />\r\n",
                    "      <input type=\"submit\" name=\"action\" value=\"Inches to centimeters\" />\r\n",
                    "      <input type=\"submit\" name=\"action\" value=\"Miles to kilometers\" />\r\n"
            };

            expectedActionLinesResult = new List<string> { "Yards to meters", "Inches to centimeters", "Miles to kilometers" };
        }

        [Fact]
        public void Test_MakeConversion()
        {
            expectedConversionResults = new List<double> { 2.54, 5.08, 12.7, 25.4, 50.8, 127 };
            expectedConversionInputs = new List<double> { 1, 2, 5, 10, 20, 50 };
            mult = 2.54;

            expectedConversionResults.Should().BeEquivalentTo(BackEndHelperFunctions.MakeConversion(expectedConversionInputs, mult));
        }

        [Fact]
        public void Test_ExtractActionsFromList()
        {
            expectedActionLinesResult.Should().BeEquivalentTo(BackEndHelperFunctions.ExtractActionsFromList(expectedActionLines,"value=\"","\""));
            BackEndHelperFunctions.ExtractActionsFromList(expectedActionLines,"value=\"","\"").Should().BeEquivalentTo(expectedActionLinesResult);

        }

        [Fact]
        public void Test_CheckActions()
        {
            var testInToCmKeys = new List<string> { "inc", "in", "inch", "inches", "cm", "centimetres", "centimetre" };
            var testMiToKmKeys = new List<string> { "miles", "mi", "mile", "kilo", "kilometres", "kilometre" };
            var testYdToMeKeys = new List<string> { "yards", "yard", "yardstometers", "tometers" };

            "Inches to centimeters".ToLower().ContainsAny(testInToCmKeys).Should().BeTrue("testInToCmKeys failed");
            "Yards to meters".ToLower().ContainsAny(testYdToMeKeys).Should().BeTrue("testYdToMeKeys failed");
            "Miles to kilometers".ToLower().ContainsAny(testMiToKmKeys).Should().BeTrue("testMiToKmKeys failed");
        }


    }
}
