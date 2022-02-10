using System;

namespace WebView2.RecreateWhitelistBug.WebView.Navigation
{
    public interface IWebViewNavigation
    {
        Uri Source { get; }
        
        event EventHandler<Uri> ChangeSourceRequest;

        void NavigateTo(string uri);

        void NavigateTo(Uri uri);
    }
}