using GalaSoft.MvvmLight;
using NewXXSY.Models;
using NewXXSY.Server;
using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Collections.ObjectModel;
using System.Text;

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

        public MainViewModel()
        {
            adsCollection = new ObservableCollection<Ads[]>();
            plateCollection = new ObservableCollection<Plate>();
            Load();
        }

        private async void Load()
        {
            var html = await HttpServer.Instance.GetHtml();
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            try
            {
                var nodes = document.DocumentNode.SelectNodes(@"//body[@id=""nv_forum""]/div[@class=""wp cl""]//ul/li");
                int i = 0;
                Ads[] adss = new Ads[4];
                foreach (var ad in nodes)
                {
                    var url = ad.GetAttributeValue("href", "null");
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
                            var palte = new Plate()
                            {
                                Name = name,
                                Group = title,
                                Url = url
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

                            var palte = new Plate()
                            {
                                Name = name,
                                Group = title,
                                Url = url
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
