using WebView2.RecreateWhitelistBug.WebView.Configuration;
using WebView2.RecreateWhitelistBug.WebView.Navigation;

namespace WebView2.RecreateWhitelistBug
{
    public class WebPageViewModel
    {
        public WebPageViewModel(
            string name,
            IWebViewConfiguration configuration,
            IWebViewNavigation navigation)
        {
            Title = name;
            
            Configuration = configuration;
            Navigation = navigation;
        }
        
        public string Title { get; set; }
        
        public IWebViewConfiguration Configuration { get; private set; }
        
        public IWebViewNavigation Navigation { get; private set; }
    }
}