using Java.Lang;
using NewXXSY.Server;
using NewXXSY.Views;
using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NewXXSY
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTcxNzMxQDMxMzkyZTM0MmUzMGtGaFpNMXdVRnJiemFVckhvQ2w1cm9Mb3VuajNlbmU4bExFY3UvVDJWeWs9");
            InitializeComponent();
            DependencyService.Get<ILanguageService>().SetLanguage("en_US");
            var cookie = HttpServer.Instance.GetCookie();
            if (cookie != null && cookie.Contains("seccodec"))
            {
                Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
                Routing.RegisterRoute(nameof(TieList), typeof(TieList));
                Routing.RegisterRoute(nameof(TiePage), typeof(TiePage));
                Routing.RegisterRoute(nameof(ReplyView), typeof(ReplyView));
                Routing.RegisterRoute(nameof(NoticePage), typeof(NoticePage));
                Routing.RegisterRoute(nameof(PMPage), typeof(PMPage));

                MainPage = new AppShell();
            }
            else
            {
                MainPage = new LoginPage();
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
