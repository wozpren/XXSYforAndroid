using Android.Content;
using Java.Util;
using NewXXSY.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewXXSY.Droid
{
    internal class LanguageService : ILanguageService
    {
        public void SetLanguage(string lang)
        {
            if (!string.IsNullOrEmpty(lang))
            {
                // Get application context
                Context context = Android.App.Application.Context;

                // Set application locale by selected language
                Locale.Default = new Locale(lang);
                context.Resources.Configuration.SetLocale(Locale.Default);
                context.ApplicationContext.CreateConfigurationContext(context.Resources.Configuration);
                context.Resources.DisplayMetrics.SetTo(context.Resources.DisplayMetrics);
            }
        }
    }
}
