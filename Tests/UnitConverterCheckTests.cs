using Xunit;
using FluentAssertions;
using YoCode;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace YoCode_XUnit
{
    public class UnitConverterCheckTests
    {
        public UnitConverterCheck test;

        public List<double> expectedConversionResults;
        public List<double> expectedConversionInputs;
        public double mult;

        List<string> expectedActionLines;
        List<string> expectedActionLinesResult;

        public UnitConverterCheckTests()
        {
            test = new UnitConverterCheck("fake port");

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

            expectedConversionResults.Should().BeEquivalentTo(test.MakeConversion(expectedConversionInputs, mult));
        }

        [Fact]
        public void Test_ExtractActionsFromList()
        {
            expectedActionLinesResult.Should().BeEquivalentTo(test.ExtractActionsFromList(expectedActionLines));
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

        [Fact]
        public void Test_FindActualResultForExpectation()
        {
            var listOfActualResults = new List<UnitConverterResults>();

            var actualElem1 = new UnitConverterResults
            {
                input = 2,
                action = "Inches to Centimeters",
                output = 5.08
            };

            var actualElem2 = new UnitConverterResults
            {
                input = 5,
                action = "Miles to kilometers",
                output = 8.04672
            };

            var actualElem3 = new UnitConverterResults
            {
                input = 20,
                action = "Yards to meters",
                output = 18.288
            };

            var testElem1 = new UnitConverterResults
            {
                input = 20,
                action = "Yards to meters",
                output = 6
            };

            var testElem2 = new UnitConverterResults
            {
                input = 5,
                action = "Miles to kilometers",
                output = 8.2
            };

            listOfActualResults.Add(actualElem1);
            listOfActualResults.Add(actualElem2);
            listOfActualResults.Add(actualElem3);

            actualElem3.Should().BeEquivalentTo(UnitConverterCheck.FindActualResultForExpectation(actualElem3, listOfActualResults));
            actualElem3.Should().BeEquivalentTo(UnitConverterCheck.FindActualResultForExpectation(testElem1, listOfActualResults));
        }
    }

 }
