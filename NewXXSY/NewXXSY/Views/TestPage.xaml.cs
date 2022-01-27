using HtmlAgilityPack;
using NewXXSY.Server;
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
	public partial class TestPage : ContentPage
	{
		string html;
		public TestPage ()
		{
			InitializeComponent();
			Load();
		}

		private async void Load()
		{
			HtmlLabel.Text = html = await HttpServer.Instance.GetHtml();
		}

		private void Button_Pressed(object sender, EventArgs e)
        {
			var xpath = (XpathInput.InputView as Entry).Text;
			HtmlDocument document = new HtmlDocument();
			document.LoadHtml(html);
            try
            {
				var node = document.DocumentNode.SelectSingleNode(@"//body[@id='nv_forum']//div[@id='ct']//div[@class='fl bm']/div");
				node = node.SelectSingleNode(xpath);
				if (node != null)
				{
					HtmlLabel.Text = node.InnerHtml;
				}
				else
				{
					HtmlLabel.Text = "错误";
				}
			}
            catch (Exception ex)
            {

				HtmlLabel.Text = ex.Message;
			}

		}
    }
}