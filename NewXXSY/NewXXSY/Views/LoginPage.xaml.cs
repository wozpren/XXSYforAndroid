using NewXXSY.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using Android.Webkit;

namespace NewXXSY.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent();

			LoginWeb.Source = "https://xxsy.su/";
		}

  //      private async void Button_Clicked(object sender, EventArgs e)
  //      {
		//	//var cookie = (CookieInput.InputView as Entry).Text;
		//	//if(cookie == null) return;

  // //         var co = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(cookie);
  // //         StringBuilder sb = new StringBuilder();
  // //         foreach (var item in co)
  // //         {
  // //             sb.Append($"{item["name"]}={item["value"]};\n");
  // //         }

  // //         var coo = sb.ToString();
  // //         HttpServer.Instance.SetCookie(coo);
  // //         var html = await HttpServer.Instance.GetHtml();
		//	//if(!html.Contains("新人报道"))
  // //         {
		//	//	HttpServer.Instance.RemoveCookie();
		//	//	(CookieInput.InputView as Entry).Text = "失败";
		//	//}
  // //         else
  // //         {
		//	//	(CookieInput.InputView as Entry).Text = "成功，请重启应用";
		//	//}
		//}
    }
}