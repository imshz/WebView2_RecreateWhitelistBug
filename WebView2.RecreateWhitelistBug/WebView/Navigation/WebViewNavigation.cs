using System;

namespace WebView2.RecreateWhitelistBug.WebView.Navigation
{
    public class WebViewNavigation : IWebViewNavigation
    {
        public WebViewNavigation(string uri)
        {
            Source = new Uri(uri);
        }
        
        public Uri Source { get; }

        public event EventHandler<Uri> ChangeSourceRequest;

        public void NavigateTo(string uri)
        {
            NavigateTo(new Uri(uri));
        }

        public void NavigateTo(Uri uri)
        {
            ChangeSourceRequest?.Invoke(this, uri);
        }
    }
}