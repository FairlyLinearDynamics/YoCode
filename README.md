# YoCode
A code analysis tool to help evaluate code faster and more comfortably than ever before

![](https://github.com/FairlyLinearDynamics/YoCode/blob/i204-ReadmeUpdate/Images/YoCode2.gif)

## About
YoCode is a .NET Core application and it only really works on very specific kinds of projects, but it always tries to do the following checks:
* Runs all unit tests of the project under test.
* Inputs certain values directly to the web app (if one exists) through the backend and check the output.
* Uses Selenium to find a text box for the web app, tries to input certain values and checks the output.
* Runs an external tool that calculates the code coverage.
* Checks the amount of code and duplication in it.
* Uses Git to check which files have been changed.
* Looks for certain HTML elements of the web app and checks for evidence (keywords) that would suggest that the user interface was implemented.
* Checks .cshtml files of the project under test for keywords that would suggest that the user interface was implemented.
* Uses Git to check if the project under tests contains a valid Git Repository, displays the commits if it does.
* Inputs certain values directly to the web app through the backend and checks that those values will have the expected results
* Uses Selenium to look for places to input values and tries to input numbers, then tries to find buttons with keywords and click on it, checks if the output is correct
* Takes a screenshot of the web apps UI (if there is one)

![](https://github.com/FairlyLinearDynamics/YoCode/blob/i204-ReadmeUpdate/Images/YoCode_ScreenShot.png)

## To Set Up YoCode:

* Download CMD Tools from https://www.jetbrains.com/resharper/download/download-thanks.html?platform=windows&code=RSCLT
* Download dotCover from https://www.jetbrains.com/dotcover/download/#section=commandline

* Create appsettings.json file inside YoCode project (same directory as YoCode.csproj) with the following code:

```
{
  "duplicationCheckSetup": {
    "CMDtoolsDir": "PATH TO CMD LIBRARY (..\CMD)"
  },
  "codeCoverageCheckSetup": {
    "dotCoverDir": "PATH TO DOTCOVER (e.g. ...\JetBrains.dotCover.CommandLineTools.2018.1.4)"
  },
  "featureWeightings": {
    "Junior": "..\\..\\..\\FeatureWeightings\\JuniorWeightings.json",
    "Original": "..\\..\\..\\FeatureWeightings\\OriginalWeightings.json"
  },
  "OriginalTest-Tests": {
    "CodeBaseCost": 550,
    "DuplicationCost": 110
  },
  "OriginalTest-App": {
    "CodeBaseCost": 1755,
    "DuplicationCost": 388
  },
  "JuniorTest-Tests": {
    "CodeBaseCost": 626,
    "DuplicationCost": 127
  },
  "JuniorTest-App": {
    "CodeBaseCost": 1755,
    "DuplicationCost": 388
  }
}
```
  
* Create testappsettings.json file inside YoCodeAutomatedTests project with the following code:
```
{
  "AutomatedTesting": {
    "TestDataPath": "PATH TO YOCODE AUTOMATED TESTS (e.g. C:\\YoCodeTestData)"
  },
  "YoCodeLocation": {
    "DLLFolderPath":  "PATH TO FOLDER WITH YoCode.dll (..\YoCode\YoCode\bin\Debug\netcoreapp2.1)"
  }
}
```
## To Run YoCode:
* Open Command Line in the directory where YoCode.dll is (...\YoCode\YoCode\bin\Debug\netcoreapp2.1)
* start each command with "dotnet YoCode.dll " (example command: `dotnet YoCode.dll --help`)

The application takes only one parameter: path to the modified test directory (`--input=PATH TO TEST HERE`)

List of possible commands:
* `--input=PATH` - the only mandatory command before every normal run, specifies the location of the project under test
* `--help` - displays help, can be run on its own

It's possible to append certain flags to a command.
List of possible flags:
* `--noloading` - disables the comforting loading screen while waiting for results
* `--silent` - does not automatically open the HTML report at the end of execution
* `--output=PATH` - specify a custom path for the HTML report to be generated in
* `--nohtml` - does not generate a HTML report at all

Example command: `dotnet YoCode.dll --input=C:\Project1 --noloading --silent --output=C:\Downloads`
