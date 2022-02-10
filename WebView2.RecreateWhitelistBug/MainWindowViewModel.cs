using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WebView2.RecreateWhitelistBug.Properties;
using WebView2.RecreateWhitelistBug.WebView.Configuration;
using WebView2.RecreateWhitelistBug.WebView.Navigation;
using WebView2.RecreateWhitelistBug.Wpf;

namespace WebView2.RecreateWhitelistBug
{
    public class WebPage
    {
        public string Name { get; set; }

        public string Uri { get; set; }
    }

    public interface IMainWindowsViewModel : INotifyPropertyChanged { }

    public class MainWindowViewModel : IMainWindowsViewModel
    {
        public MainWindowViewModel()
        {
            BrowseWebPageCommand = new Command(BrowseWebPageCommandExecuted, () => true);

            BrowseableWebPages = new List<WebPage>
            {
                new WebPage{ Name = "VG", Uri = "http://vg.no" },
                new WebPage{ Name = "Dagbladet", Uri = "http://dagbladet.no" },
                new WebPage{ Name = "Sky News", Uri = "https://news.sky.com" },
                new WebPage{ Name = "BBC News", Uri = "https://www.bbc.com/news" }
            };
        }
        
        private IReadOnlyCollection<WebPage> _browseableWebPages;
        public IReadOnlyCollection<WebPage> BrowseableWebPages
        {
            get { return _browseableWebPages; }
            set
            {
                _browseableWebPages = value;

                OnPropertyChanged();
            }
        }

        private WebPage _selectedWebPage;
        public WebPage SelectedWebPage
        {
            get { return _selectedWebPage; }
            set
            {
                _selectedWebPage = value;
                OnPropertyChanged();
            }
        }

        private ICommand _browseWebPageCommand;
        public ICommand BrowseWebPageCommand
        {
            get { return _browseWebPageCommand; }
            set
            {
                _browseWebPageCommand = value;
                OnPropertyChanged();
            }
        }

        private string _runtimePath = @"C:\Program Files (x86)\NDLO CIS\Microsoft WebView2 Runtime";
        public string RuntimePath
        {
            get { return _runtimePath; }
            set
            {
                _runtimePath = value;
                OnPropertyChanged();
            }
        }

        private string _userDataFolderPath = @"C:\temp\webview2userdata";
        public string UserDataFolderPath
        {
            get { return _userDataFolderPath; }
            set
            {
                _userDataFolderPath = value;
                OnPropertyChanged();
            }
        }

        private string _whitelist = "10.0.0.123, weeee-nonexisting-greatewebsite.no";
        public string Whitelist
        {
            get { return _whitelist; }
            set
            {
                _whitelist = value;
                OnPropertyChanged();
            }
        }

        private void BrowseWebPageCommandExecuted()
        {
            if (SelectedWebPage != null)
            {
                var r = new Random();

                // Initial settings
                var webViewConfiguration = new WebViewConfiguration(RuntimePath, Guid.NewGuid(), UserDataFolderPath, false, new Dictionary<string, string>
                {
                    {"key1", Guid.NewGuid().ToString()},
                    {"key2", Guid.NewGuid().ToString()}
                }, Whitelist);

                var dataContext = new WebPageViewModel(
                    SelectedWebPage.Name,
                    webViewConfiguration,
                    new WebViewNavigation(SelectedWebPage.Uri));

                var webPageWindow = new WebPageWindow
                {
                    DataContext = dataContext
                };

                webPageWindow.Show();
            }
        }

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}