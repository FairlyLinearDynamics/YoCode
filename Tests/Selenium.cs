using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace YoCode_XUnit
{
    public class Selenium
    {
        IWebDriver browser;

        public Selenium()
        {
            browser = new FirefoxDriver(@"C:\Users\ukekar\source\repos\YoCode\Tests\bin\Debug\netcoreapp2.1");
            browser.Navigate().GoToUrl("http://localhost:5000/");
        }

        [Fact]
        public void TagFound()
        {
            var tags = browser.FindElements(By.CssSelector("option"));
            var result = "";
            foreach(var tag in tags)
            {
                if (tag.Text.Equals("Yard"))
                {
                    result = tag.Text;
                }
            }
            result.Should().Be("Yard");
            browser.Close();

        }
    }
}
