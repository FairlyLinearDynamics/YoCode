﻿using Microsoft.Extensions.Configuration;

namespace YoCode
{
    internal interface IAppSettingsBuilder
    {
        IConfiguration ReadJSONFile();

        string GetCMDToolsPath();
        string GetDotCoverDir();
    }
}