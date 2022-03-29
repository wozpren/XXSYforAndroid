using GalaSoft.MvvmLight;
using NewXXSY.Server;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using HtmlAgilityPack;
using NewXXSY.Models;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using System.Text.RegularExpressions;

namespace NewXXSY.ViewModels
{
    [QueryProperty(nameof(Fid), "fid")]
    public class TieListViewModel : ViewModelBase
    {

        private bool loaded;
        public bool Loaded
        {
            get => loaded;
            set
            {
                loaded = value;
                RaisePropertyChanged(nameof(Loaded));
            }
        }

        private bool loadMore;
        public bool LoadMore
        {
            get => loadMore;
            set
            {
                loadMore = value;
                RaisePropertyChanged(nameof(LoadMore));
            }
        }

        private int page;
        public int Page
        {
            get => page;
            set
            {
                page = value;
                RaisePropertyChanged(nameof(Page));
                LoadPage();
            }
        }

        private ObservableCollection<TieBar> tieBarCollection;
        public ObservableCollection<TieBar> TieBarCollection
        {
            get { return tieBarCollection; }
            set
            {
                tieBarCollection = value;
                RaisePropertyChanged(nameof(TieBarCollection));
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
                        var bar = eventArgs.ItemData as TieBar;
                        Goto(bar.Tid);
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

        private RelayCommand<object> loadMoreItems;
        public RelayCommand<object> LoadMoreItems
        {
            get
            {
                if (loadMoreItems == null)
                {
                    loadMoreItems = new RelayCommand<object>(obj =>
                    {
                        if(!LoadMore)
                        {
                            LoadMore = true;
                            Page++;
                        }

                    });
                }
                return loadMoreItems;
            }
            set
            {
                loadMoreItems = value;
            }
        }

        private bool Enter;

        public TieListViewModel()
        {
            Loaded = true;
            page = 1;
            tieBarCollection = new ObservableCollection<TieBar>();
        }

        private string fid;
        public string Fid
        {
            set
            {
                fid = value;
                if(!Enter)
                {
                    Enter = true;
                    LoadPage();
                }
            }
        }

        private async void Goto(string tid)
        {
            await (App.Current.MainPage as Shell).GoToAsync($"//Main/TieList/TiePage?jump=false&pid=null&tid={tid}");
        }
        private async void LoadPage()
        {
            try
            {
                var html = await HttpServer.Instance.GetHtml($"forum.php?mod=forumdisplay&fid={fid}&page={page}");
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);

                var nodes = document.DocumentNode.SelectNodes("//div[@id='threadlist']//form//table/tbody");
                foreach (var node in nodes)
                {
                    var n = node.SelectSingleNode(".//a[@class='s xst']");
                    if(n != null)
                    {
                        var title = n.InnerText;
                        var tid = n.GetAttributeValue("href", "null");
                        tid = Regex.Match(tid, "tid=([0-9]*)").Groups[1].Value;

                        var mes = node.SelectSingleNode("./tr/td[@class='by']");
                        var img = mes.SelectSingleNode(".//img").GetAttributeValue("src", "");
                        if(img.Contains("uc_server"))
                        {
                            var id = Regex.Match(img, "uid=([0-9]*)").Groups[1].Value;
                            id = Util.IDtoURL(id);
                            img = $"https://xxsy.su/uc_server/data/avatar/{id}_avatar_small.jpg";
                        }

                        var name = mes.SelectSingleNode("./cite").InnerText.Replace("\r\n", "");
                        var time = mes.SelectSingleNode("./em").InnerText.Replace("&nbsp;", "");

                        var reply = node.SelectSingleNode("./tr/td[@class='num']/a").InnerText;
                        var watch = node.SelectSingleNode("./tr/td[@class='num']/em").InnerText;
                        var other = node.SelectSingleNode("./tr/td[@class='by'][2]");
                        var newname = other.SelectSingleNode("./cite").InnerText;
                        var newtime = other.SelectSingleNode("./em").InnerText.Replace("&nbsp;", "");


                        TieBarCollection.Add(new TieBar 
                        { 
                            Title = title, 
                            Tid = tid,
                            Avtar = img,
                            Author = name,
                            Time = time,
                            Reply = reply,
                            Watch = watch,
                            NewAuthor = newname,
                            NewTime = newtime,
                        });

                    }
                }
                Loaded = false;
                LoadMore = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
