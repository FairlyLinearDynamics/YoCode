# To run this program you will need to: #

* Download CMD Tools from https://www.jetbrains.com/resharper/download/download-thanks.html?platform=windows&code=RSCLT
* Download dotCover from https://www.jetbrains.com/dotcover/download/#section=commandline

* Create appsettings.json file inside YoCode project with the following code:

```
{
  "duplicationCheckSetup": {
    "CMDtoolsDir": "PATH TO CMD LIBRARY (..\CMD)"
  },
  "codeCoverageCheckSetup": {
    "dotCoverDir": " PATH TO DOTCOVER (e.g. ...\JetBrains.dotCover.CommandLineTools.2018.1.4)"
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
