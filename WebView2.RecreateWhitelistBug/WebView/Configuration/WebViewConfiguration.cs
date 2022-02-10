using System;
using System.Collections.Generic;

namespace WebView2.RecreateWhitelistBug.WebView.Configuration
{
    public class WebViewConfiguration : IWebViewConfiguration
    {
        public WebViewConfiguration() { }

        public WebViewConfiguration(
            string webView2RuntimePath = null,
            Guid? sessionId = null,
            string userDataFolder = null,
            bool clearCacheOnStartup = false,
            Dictionary<string, string> environmentData = null,
            string authServerWhitelist = null
        )
        {
            WebView2RuntimePath = webView2RuntimePath;
            SessionId = sessionId == null || sessionId.Value.Equals(Guid.Empty) ? Guid.NewGuid() : sessionId.Value;
            UserDataFolder = userDataFolder;
            ClearCacheOnStartup = clearCacheOnStartup;
            EnvironmentData = environmentData;
            AuthServerWhitelist = authServerWhitelist ?? string.Empty;
        }

        public string WebView2RuntimePath { get; set; } = null;

        public Guid SessionId { get; set; }

        public string UserDataFolder { get; set; } = null;

        public bool ClearCacheOnStartup { get; set; } = true;

        public Dictionary<string, string> EnvironmentData { get; set; } = null;

        public string AuthServerWhitelist { get; set; } = string.Empty;
    }
}