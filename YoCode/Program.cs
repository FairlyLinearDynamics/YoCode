using System;
using System.IO;

namespace YoCode
{
    public class Program
    {

        static void Main(string[] args)
        {
            var consoleOutput = new PrintToConsole();
            var testResults = new TestResults();
            consoleOutput.PrintIntroduction();

            string[] keyPattern = new string[] { "Miles", "Kilometers", "Km" };
            string html = @"C:\Users\ukekar\Downloads\no-to-interview\0A2F986A7029D8AF3D51499176F359ED06B832A25834FA29F1713D7B35FBAE19\UnitConverterWebApp\Views\Home\Index.cshtml";

            // UI test
            UICheck uiTest = new UICheck(html, keyPattern);
            testResults.UiCheck = uiTest.ContainsFeature;



            consoleOutput.PrintFinalResults(testResults);


            //Console.Write(UICheck.UIContainsFeature(htmls, keyPattern));
        }
    }
}
