using FluentAssertions;
using System.Collections.Generic;
using Xunit;
using YoCode;

namespace YoCode_XUnit
{
    public class UnitConverterTests
    {
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

