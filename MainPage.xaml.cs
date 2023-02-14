using Microsoft.AspNetCore.Components.WebView.Maui;

namespace MauiBlazorUIBestPractices;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        Loaded += ContentPage_Loaded;

        BlazorWebViewHandler.BlazorWebViewMapper.AppendToMapping("CustomBlazorWebViewMapper", (handler, view) =>
        {
#if WINDOWS
            //Workaround for WinUI splash screen
            if (AppInfo.Current.RequestedTheme == AppTheme.Dark)
            {
                handler.PlatformView.DefaultBackgroundColor = Microsoft.UI.Colors.Black;
            }
#endif

#if IOS || MACCATALYST
            handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
            handler.PlatformView.Opaque = false;
#endif

#if ANDROID
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
        });
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
#if WINDOWS && RELEASE
        var webView2 = (blazorWebView.Handler.PlatformView as Microsoft.UI.Xaml.Controls.WebView2);
        await webView2.EnsureCoreWebView2Async();

        var settings = webView2.CoreWebView2.Settings;
        settings.AreBrowserAcceleratorKeysEnabled = false;
        settings.IsZoomControlEnabled = false;
#endif
    }
}