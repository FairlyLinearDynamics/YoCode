# To run this program you will need to: #

* Download CMD Tools from https://www.jetbrains.com/resharper/download/download-thanks.html?platform=windows&code=RSCLT

* Create appsettings.json file inside YoCode project with the following code:

```
{
  "duplicationCheckSetup": {
    "CMDtoolsDir": "PATH TO CMD LIBRARY (..\CMD)"
  }
}
```
  
* Create testappsettings.json file inside YoCodeAutomatedTests project with the following code:
```
{
  "AutomatedTesting": {
    "TestDataPath": "C:\\YoCodeTestData"
  },
  "YoCodeLocation": {
    "DLLFolderPath":  "PATH TO FOLDER WITH YoCode.dll file (..\YoCode\YoCode\bin\Debug\netcoreapp2.1)"
  }
}
```
