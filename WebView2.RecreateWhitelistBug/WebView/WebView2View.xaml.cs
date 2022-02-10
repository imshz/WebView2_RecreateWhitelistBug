using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Web.WebView2.Core;
using WebView2.RecreateWhitelistBug.WebView.Configuration;
using WebView2.RecreateWhitelistBug.WebView.Navigation;

namespace WebView2.RecreateWhitelistBug.WebView
{
    /// <summary>
    /// Interaction logic for WebView.xaml
    /// </summary>
    public partial class WebView2View : UserControl
    {
        #region Navigation

        public static readonly DependencyProperty NavigationProperty = DependencyProperty.Register("Navigation", typeof(IWebViewNavigation), typeof(WebView2View), new FrameworkPropertyMetadata(OnNavigationChanged));

        private static readonly List<EventHandler<Uri>> NavigateToSubscriptions = new List<EventHandler<Uri>>();

        private static void OnNavigationChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.OldValue != null)
            {
                var navigation = (IWebViewNavigation)eventArgs.NewValue;

                NavigateToSubscriptions.ForEach(action => navigation.ChangeSourceRequest -= action);
                NavigateToSubscriptions.Clear();
            }

            if (eventArgs.NewValue != null)
            {
                var navigation = (IWebViewNavigation)eventArgs.NewValue;
                var navigationUri = new Uri(navigation.Source.AbsoluteUri + "?sid=" + ((WebView2View) dependencyObject)._currentConfiguration?.SessionId);
                
                ((WebView2View) dependencyObject).ChromiumControl.Source = ((WebView2View)dependencyObject)._currentConfiguration == null ? navigation.Source : navigationUri;

                void EventHandler(object sender, Uri uri)
                {
                    ((WebView2View) dependencyObject).ChromiumControl.Source = uri;
                }

                navigation.ChangeSourceRequest += EventHandler;

                NavigateToSubscriptions.Add(EventHandler);
            }
        }
        
        public IWebViewNavigation Navigation
        {
            get => (IWebViewNavigation) GetValue(NavigationProperty);
            set => SetValue(NavigationProperty, value);
        }

        #endregion

        

        #region Configuration

        public static readonly DependencyProperty ConfigurationProperty = DependencyProperty.Register("Configuration", typeof(IWebViewConfiguration), typeof(WebView2View), new FrameworkPropertyMetadata(OnConfigurationChanged));

        public IWebViewConfiguration Configuration
        {
            get => (IWebViewConfiguration) GetValue(ConfigurationProperty);
            set => SetValue(ConfigurationProperty, value);
        }

        private IWebViewConfiguration _currentConfiguration;

        private static async void OnConfigurationChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.NewValue != null)
            {
                var configuration = ((WebView2View) dependencyObject)._currentConfiguration = (IWebViewConfiguration) eventArgs.NewValue;

                // Clear userdata-folder
                if(configuration.ClearCacheOnStartup
                       && Directory.Exists(Path.Combine(configuration.UserDataFolder, "EBWebView", "Default", "Cache")))
                        TryRecursiveDeleteDirectory(new DirectoryInfo(Path.Combine(configuration.UserDataFolder, "EBWebView", "Default", "Cache")));

                // Setup whitelist
                var env = await CoreWebView2Environment.CreateAsync(
                    configuration.WebView2RuntimePath,
                    configuration.UserDataFolder,
                    new CoreWebView2EnvironmentOptions($@"--auth-server-whitelist=""{configuration.AuthServerWhitelist}"""));

                await ((WebView2View)dependencyObject).ChromiumControl.EnsureCoreWebView2Async(env);
            }
            else
            {
                ((WebView2View) dependencyObject)._currentConfiguration = null;
            }
        }

        private static void TryRecursiveDeleteDirectory(DirectoryInfo baseDir)
        {
            try
            {
                if (!baseDir.Exists)
                    return;

                foreach (var dir in baseDir.EnumerateDirectories())
                {
                    TryRecursiveDeleteDirectory(dir);
                }

                var files = baseDir.GetFiles();
                foreach (var file in files)
                {
                    file.IsReadOnly = false;
                    file.Delete();
                }

                baseDir.Delete();
            } catch(Exception) { }
        }

        #endregion
        
        private Microsoft.Web.WebView2.Wpf.WebView2 ChromiumControl => (Microsoft.Web.WebView2.Wpf.WebView2)Content;

        public WebView2View()
        {
            InitializeComponent();

            ChromiumControl.CoreWebView2InitializationCompleted += OnCoreWebView2InitializationCompleted;
            ChromiumControl.WebMessageReceived += OnWebMessageReceived;
        }

        private async void OnCoreWebView2InitializationCompleted(object sender, EventArgs e)
        {
            ChromiumControl.CoreWebView2.Settings.IsWebMessageEnabled = true;
        }
        
        private async void OnWebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            
        }
    }
}
