using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NewXXSY.Server;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace NewXXSY.ViewModels
{
	[QueryProperty(nameof(Formhash), "formhash")]
	[QueryProperty(nameof(Fid), "fid")]
	[QueryProperty(nameof(Tid), "tid")]
	internal class ReplyViewModel:ViewModelBase
    {
		private string tid;
		public string Tid
		{
			set { tid = value; }
		}
		private string fid;
		public string Fid
		{
			set { fid = value; }
		}
		private string formhash;
		public string Formhash
		{
			set { formhash = value; }
		}

		private bool replying;
		public bool Replying
		{
			get => replying;
			set { replying = value; }
		}


		private string htmlText;
		public string HtmlText
        {
			get => htmlText;
			set => Set(nameof(HtmlText), ref htmlText, value);
		}

		private RelayCommand replyPost;
		public RelayCommand ReplyPost
		{
			get
			{
				if (replyPost == null)
				{
					replyPost = new RelayCommand(() =>
					{
						Reply();
					});
				}
				return replyPost;
			}
		}



		private bool Sent;


		public async void Reply()
		{
			if (!Sent)
			{
				Sent = true;
				MessagingCenter.Send(this, "回帖开始");
				string text = DoHtmlToUBB(HtmlText);
				FormUrlEncodedContent multipartFormDataContent = new FormUrlEncodedContent(new KeyValuePair<string, string>[6]
				{
				new KeyValuePair<string, string>("formhash", formhash),
				new KeyValuePair<string, string>("posttime", new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString()),
				new KeyValuePair<string, string>("wysiwyg", "1"),
				new KeyValuePair<string, string>("noticeauthor", "1"),
				new KeyValuePair<string, string>("message", text),
				new KeyValuePair<string, string>("usesig", "1")
				});
				await HttpServer.Instance.PostAsync(MakeUrl(), multipartFormDataContent);
				Sent = false;
				MessagingCenter.Send(this, "回帖结束");
			}
		}

		private string MakeUrl()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(HttpServer.url);
			stringBuilder.Append($"forum.php?mod=post&action=reply&fid={fid}&tid={tid}&extra=&replysubmit=yes");
			return stringBuilder.ToString();
		}

		private string DoHtmlToUBB(string _Html)
		{
			_Html = Regex.Replace(_Html, "<br[^>]*>", "[/b]", RegexOptions.Multiline);
			_Html = Regex.Replace(_Html, "<p[^>\\/]*\\/>", "\\n", RegexOptions.Multiline);
			_Html = Regex.Replace(_Html, "<ul>(.*?)</ul>", "[list]$1[/list]");
			_Html = Regex.Replace(_Html, "<ol>(.*?)</ol>", "[list=1]$1[/list]");
			_Html = Regex.Replace(_Html, "<li>(.*?)</li>", "[*]$1");
			_Html = Regex.Replace(_Html, "<em>(.*?)</em>", "[i]$1[/i]");
			_Html = Regex.Replace(_Html, "<strong>(.*?)</strong>", "[b]$1[/b]");
			_Html = Regex.Replace(_Html, "<span style=\"text-decoration:\\sunderline;\">(.*?)</span>", "[u]$1[/u]");
			_Html = Regex.Replace(_Html, "---", "[hr]", RegexOptions.Multiline);
			_Html = Regex.Replace(_Html, "<a[^>]*href=['\"\\s]*([^\\s'\"]*)[^>]*>(.*?)<\\/a>", "[url=$1]$2[/url]");
			_Html = Regex.Replace(_Html, "<div>(.*?)</div>", "$1");
			_Html = Regex.Replace(_Html, "&amp;", "&");
			_Html = Regex.Replace(_Html, "&nbsp;", " ");
			_Html = Regex.Replace(_Html, "&lt;", "<");
			_Html = Regex.Replace(_Html, "&gt;", ">");
			Regex regex = new Regex("<[^>]*?>(.*)<[^>]*?>");
			while (regex.IsMatch(_Html))
			{
				_Html = regex.Replace(_Html, "$1");
			}
			return _Html;
		}
	}
}
