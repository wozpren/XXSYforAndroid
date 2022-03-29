using GalaSoft.MvvmLight;
using HtmlAgilityPack;
using NewXXSY.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NewXXSY.ViewModels
{
    public class AppShellViewModel : ViewModelBase
    {
        private string notice;
        public string Notice
        {
            get => notice;
            set => Set(nameof(Notice), ref notice, value);
        }

        public AppShellViewModel()
        {
            HttpServer.Instance.LoadNewPage += UpdateNotice;
        }

        private void UpdateNotice(string html)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            try
            {
                
                var node = document.DocumentNode.SelectSingleNode(@"//div[@id='um']/p/a[@id='myprompt']");
                if (node == null)
                    return;
                var n = Regex.Match(node.InnerText, @"\d+");
                if (n.Success && int.TryParse(n.Value, out int num))
                {
                    Notice = n.Value;
                }
                else
                    Notice = "";
            }
            catch(NullReferenceException ex)
            {
                App.Current.MainPage.DisplayAlert("发生了一个错误", ex.StackTrace, "Ok");
            }

        }
    }
}
