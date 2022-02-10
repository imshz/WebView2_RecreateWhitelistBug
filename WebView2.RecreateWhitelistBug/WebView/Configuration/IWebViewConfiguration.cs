using System;
using System.Collections.Generic;

namespace WebView2.RecreateWhitelistBug.WebView.Configuration
{
    public interface IWebViewConfiguration
    {
        string WebView2RuntimePath { get; }

        Guid SessionId { get; set; }

        string UserDataFolder { get; set; }

        bool ClearCacheOnStartup { get; set; }

        Dictionary<string, string> EnvironmentData { get; set; }

        string AuthServerWhitelist { get; set; }
    }

}