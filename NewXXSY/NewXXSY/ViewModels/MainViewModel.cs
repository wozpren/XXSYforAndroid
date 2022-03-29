using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NewXXSY.Models;
using NewXXSY.Server;
using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NewXXSY.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<Ads[]> adsCollection;
        public ObservableCollection<Ads[]> AdsCollection
        {
            get { return adsCollection; }
            set
            {
                adsCollection = value;
                RaisePropertyChanged(nameof(AdsCollection));
            }
        }

        private ObservableCollection<Plate> plateCollection;
        public ObservableCollection<Plate> PlateCollection
        {
            get { return plateCollection; }
            set
            {
                plateCollection = value;
                RaisePropertyChanged(nameof(PlateCollection));
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
                        var bar = eventArgs.ItemData as Ads;
                        GoAds(bar.Url);
                    },
                    obj =>
                    {
                        var eventArgs = obj as Syncfusion.ListView.XForms.ItemTappedEventArgs;
                        var bar = eventArgs.ItemData as Ads;
                        return bar.Url != "null";
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

        private RelayCommand qiandao;
        public RelayCommand QianDao
        {
            get
            {
                if(qiandao == null)
                {
                    qiandao = new RelayCommand(() =>
                    {
                        if(!qian)
                        {
                            qian = true;
                            MessagingCenter.Send(this, "签到开始");
                            Qian();
                        }

                    });
                }
                return qiandao;
            }
        }

        private RelayCommand<object> tapList;
        public RelayCommand<object> TapList
        {
            get 
            { 
                if(tapList == null)
                {
                    tapList = new RelayCommand<object>(obj =>
                    {
                        var eventArgs = obj as Syncfusion.ListView.XForms.ItemTappedEventArgs;
                        var plate = eventArgs.ItemData as Plate;
                        Goto(plate.Fid);
                    });
                }
                return tapList; 
            }
            set
            {
                tapList = value;
                RaisePropertyChanged(nameof(TapList));
            }
        }

        private string formhash;
        private bool qian;
        public MainViewModel()
        {
            adsCollection = new ObservableCollection<Ads[]>();
            plateCollection = new ObservableCollection<Plate>();
            Load();
        }

        private async void Goto(string fid)
        {
            await (App.Current.MainPage as Shell).GoToAsync($"//Main/TieList?fid={fid}");
        }

        private async void Qian()
        {
            var context = await HttpServer.Instance.GetHtml($"plugin.php?id=k_misign:sign&operation=qiandao&formhash={formhash}&format=empty");
            if(context.Contains("今日已签"))
            {
                MessagingCenter.Send(this, "签到", false);
            }
            else
            {
                MessagingCenter.Send(this, "签到", true);

            }
            qian = false;
        }

        public async void GoAds(string uri)
        {
            var tid = Regex.Match(uri, "tid=([0-9]*)").Groups[1].Value;
            await Shell.Current.GoToAsync($"////Main/TiePage?tid={tid}");
        }


        private async void Load()
        {
            var html = await HttpServer.Instance.GetHtml();
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            try
            {
                var nodes = document.DocumentNode.SelectNodes(@"//body[@id=""nv_forum""]/div[@class=""wp cl""]//ul/li");
                formhash = document.DocumentNode.SelectSingleNode("//input[@name='formhash']")?.GetAttributeValue("value", "null");
                int i = 0;
                Ads[] adss = new Ads[4];
                foreach (var ad in nodes)
                {
                    var url = ad.SelectSingleNode("./a").GetAttributeValue("href", "null");
                    var name = ad.InnerText;
                    var ads = new Ads() { Name = name, Url = url };
                    adss[i] = ads;
                    i++;

                    if (i % 4 == 0)
                    {
                        AdsCollection.Add(adss);
                        adss = new Ads[4];
                        i = 0;
                    }

                }

                var titlenodes = document.DocumentNode.SelectNodes(@"//body[@id='nv_forum']//div[@id='ct']//div[@class='fl bm']/div");
                foreach (var node in titlenodes)
                {
                    var title = node.SelectSingleNode(".//h2")?.InnerText;

                    if(title == "ERA相关" || title == "深渊创作" || title == "无尽之路")
                    {
                        var tdnode = node.SelectNodes(".//table//td");
                        foreach (var td in tdnode)
                        {
                            var a = td.SelectSingleNode(".//a");
                            if (a == null)
                                break;

                            var url = a?.GetAttributeValue("href", "null");
                            var img = td.SelectSingleNode(".//img");
                            var im = img.GetAttributeValue("src", "null");
                            var name = img.GetAttributeValue("alt", "null");

                            string em = "";
                            var emm = td.SelectSingleNode("./dl/dt/em");
                            if (emm != null)
                                em = emm.InnerText;
                            var topic = td.SelectSingleNode("./dl/dd").InnerText;
                            var news = td.SelectSingleNode("./dl/dd[2]").InnerText;
                            news = news.Replace("&nbsp;", "");
                            news = news.Replace("\r\n", "");

                            var palte = new Plate()
                            {
                                Name = name,
                                Group = title,
                                Fid = Regex.Match(url, "fid=([0-9]*)").Groups[1].Value,
                                Today = em,
                                Topic = topic,
                                New = news,
                            };
                            if (im.EndsWith("jpg"))
                            {
                                palte.Image = new Xamarin.Forms.UriImageSource()
                                {
                                    CachingEnabled = true,
                                    Uri = new Uri(im),
                                    CacheValidity = new TimeSpan(5, 0, 0, 0)
                                };
                            }


                            PlateCollection.Add(palte);

                        }
                    }
                    else
                    {
                        var trnode = node.SelectNodes(".//table//tr");
                        foreach (var tr in trnode)
                        {
                            var a = tr.SelectSingleNode(".//a");
                            if (a == null)
                                break;
                            var url = a?.GetAttributeValue("href", "null");
                            var im = a.SelectSingleNode("./img");
                            var img = im.GetAttributeValue("src", "null");
                            var name = im.GetAttributeValue("alt", "null");
                            if (name == "版主申请") continue;

                            var em = "";
                            var pq = tr.SelectSingleNode("./td/h2/em");
                            if(pq != null)
                            {
                                em = pq.InnerText;
                            }
                            var zt = tr.SelectSingleNode("./td/span").InnerText;
                            var tiezi = tr.SelectSingleNode("./td/span[2]").InnerText;
                            var topic = $"主题: {zt}, 帖数: {tiezi}";

                            var nt = tr.SelectSingleNode("./td/div/a").InnerText;
                            var ar = tr.SelectSingleNode("./td/div/cite").InnerText;
                            ar = ar.Replace("&nbsp;", "");
                            ar = ar.Replace("\r\n", "");

                            var New = nt + ar;

                            var palte = new Plate()
                            {
                                Name = name,
                                Group = title,
                                Fid = Regex.Match(url, "fid=([0-9]*)").Groups[1].Value,
                                Today = em,
                                Topic = topic,
                                New = New,
                            };
                            if (img.EndsWith("jpg"))
                            {
                                palte.Image = new Xamarin.Forms.UriImageSource()
                                {
                                    CachingEnabled = true,
                                    Uri = new Uri(img),
                                    CacheValidity = new TimeSpan(5, 0, 0, 0)
                                };
                            }


                            PlateCollection.Add(palte);

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


        }


        
    }
}
