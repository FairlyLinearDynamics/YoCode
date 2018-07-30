using Xunit;
using FluentAssertions;
using YoCode;
using System.Collections.Generic;

namespace YoCode_XUnit
{
    public class UnitConverterCheckTests
    {
        public List<double> expectedConversionResults;
        public List<string> expectedConversionInputs;
        public double mult;
        public UnitConverterCheck test;

        public UnitConverterCheckTests()
        {
            test = new UnitConverterCheck("fake port");
            expectedConversionResults = new List<double> { 2.54, 5.08, 12.7, 25.4, 50.8, 127 };
            expectedConversionInputs = new List<string> { "1", "2", "5", "10", "20", "50" };
            mult = 2.54;
        }

        [Fact]
        public void Test_MakeConversion()
        {
            expectedConversionResults.Should().BeEquivalentTo(test.MakeConversion(expectedConversionInputs, mult));
        }







    }

 }
