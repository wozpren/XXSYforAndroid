using NewXXSY.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NewXXSY.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TiePage : ContentPage
	{
		public TiePage ()
		{
			InitializeComponent ();
			BindingContext = new TieViewModel();
		}

        private void PostWebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
			var web = (sender as WebView);
			web.Reload ();

		}

        private void PostWebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            Console.WriteLine("!!!");
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}