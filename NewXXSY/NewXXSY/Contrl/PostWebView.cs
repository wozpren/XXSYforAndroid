using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NewXXSY.Contrl
{
    public class PostWebView : WebView
    {
        public static readonly BindableProperty RefreshProperty = BindableProperty.Create(
            propertyName: "Refresh",
            returnType: typeof(string),
            declaringType: typeof(PostWebView),
            defaultValue: default(string));

        public string Refresh
        {
            get 
            {   
                Reload();
                return (string)GetValue(RefreshProperty); 
            }
            set { SetValue(RefreshProperty, value); }
        }


    }
}
