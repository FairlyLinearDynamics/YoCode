using Xunit;
using FluentAssertions;
using YoCode;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace YoCode_XUnit
{
    public class UnitConverterCheckTests
    {
        public List<double> expectedConversionResults;
        public List<double> expectedConversionInputs;
        public double mult;
        public UnitConverterCheck test;

        List<string> expectedActionLines;
        List<string> expectedActionLinesResult;
        public string sampleHTMLFile;


        public UnitConverterCheckTests()
        {
            test = new UnitConverterCheck("fake port");

            sampleHTMLFile = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <title>Unit Conversion</title>\r\n</head>\r\n<body>\r\n    <p>Enter values to convert</p>\r\n\r\n    <form method=\"post\" action=\"/Home/Convert\">\r\n        <textarea id=\"text\" name=\"text\" rows=\"10\" cols=\"40\"></textarea>\r\n\r\n        <input type=\"submit\" name=\"action\" value=\"Yards to meters\" />\r\n        <input type=\"submit\" name=\"action\" value=\"Inches to centimeters\" />\r\n        <input type=\"submit\" name=\"action\" value=\"Miles to kilometers\" />\r\n    <input name=\"__RequestVerificationToken\" type=\"hidden\" value=\"CfDJ8LRzOZu9uOhJh8pJmTiIAGSjPGsB5Qd0XuHQN4P629OUe2_mn_LCSCTckABECHS6Ub1-tYA8k-0aqwEcILhWXlSx1Ud17qaXT6At3hsi16cswmjtkqLjaf1Mzklr7fxufBUswWFoK8eXhUHAA4ID4SY\" /></form>\r\n\r\n</body>\r\n</html>";

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



    }

 }
