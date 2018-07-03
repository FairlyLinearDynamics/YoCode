using System;
using Xunit;
using FluentAssertions;

namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            true.Should().Be(true);
        }
        [Fact]
        public void Test2()
        {
            true.Should().Be(false);
        }
    }
}
