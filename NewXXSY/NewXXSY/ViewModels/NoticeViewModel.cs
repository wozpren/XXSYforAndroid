using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HtmlAgilityPack;
using NewXXSY.Models;
using NewXXSY.Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace NewXXSY.ViewModels
{
    public class NoticeViewModel : ViewModelBase
    {
        private bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set => Set(nameof(IsRefreshing), ref isRefreshing, value);
        }

        private bool first;
        public bool First
        {
            get => isRefreshing;
            set => Set(nameof(First), ref first, value);
        }

        private ObservableCollection<Message> messageCollection;
        public ObservableCollection<Message> MessageCollection
        {
            get { return messageCollection; }
            set
            {
                messageCollection = value;
                RaisePropertyChanged(nameof(MessageCollection));
            }
        }

        private RelayCommand<object> tapBar;
        public RelayCommand<object> TapBar
        {
            get
            {
                if (tapBar == null)
                {
                    tapBar = new RelayCommand<object>(obj =>
                    {
                        var eventArgs = obj as Syncfusion.ListView.XForms.ItemTappedEventArgs;
                        var bar = (Message)eventArgs.ItemData;
                        Goto(bar.PID, bar.UID);
                    });
                }
                return tapBar;
            }
            set
            {
                tapBar = value;
                RaisePropertyChanged(nameof(TapBar));
            }
        }

        public NoticeViewModel()
        {
            MessageCollection = new ObservableCollection<Message>();
            First = true;
            Load();
        }

        private async void Goto(string pid, string tid)
        {
            await (App.Current.MainPage as Shell).GoToAsync($"TiePage?jump=true&pid={pid}&tid={tid}");
        }

        public async void Load()
        {
            var html = await HttpServer.Instance.GetHtml("home.php?mod=space&do=notice&view=mypost&type=post");
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            try
            {
                var list = document.DocumentNode.SelectNodes("//div[@class='nts']/dl");
                foreach (var bar in list)
                {
                    var avatar = bar.SelectSingleNode(".//img").GetAttributeValue("src", "null");
                    if (avatar.Contains("uc_server"))
                    {
                        var id = Regex.Match(avatar, "uid=([0-9]*)").Groups[1].Value;
                        id = Util.IDtoURL(id);
                        avatar = $"https://xxsy.su/uc_server/data/avatar/{id}_avatar_small.jpg";
                    }
                    var time = bar.SelectSingleNode("./dt/span").InnerText;
                    time = time.Replace("&nbsp;", " ");

                    var title = bar.SelectSingleNode(".//dd[@class='ntc_body']").InnerText;
                    title = title.Replace("&nbsp;", " ");

                    var url = bar.SelectSingleNode(".//dd[@class='ntc_body']/a[@target='_blank']").GetAttributeValue("href", "null");
                    var tid = Regex.Match(url, "tid=([0-9]*)").Groups[1].Value;
                    var pid = Regex.Match(url, "pid=([0-9]*)").Groups[1].Value;
                    MessageCollection.Add(new Message(avatar, time, title, pid, tid));
                }
                First = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                _ = App.Current.MainPage.DisplayAlert("发生了一个错误", ex.Message + ex.StackTrace, "Ok");
            }

        }
    }
}
