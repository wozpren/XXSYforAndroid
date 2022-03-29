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
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NewXXSY.ViewModels
{

    [QueryProperty(nameof(Jump), "jump")]
    [QueryProperty(nameof(Pid), "pid")]
    [QueryProperty(nameof(Tid), "tid")]
    public class TieViewModel : ViewModelBase
    {

        private string tid;
        public string Tid
        {
            get => tid;
            set
            {
                tid = value;
                if(!enter)
                {
                    enter = true;
                    Load();
                }
            }
        }

        private string pid;
        public string Pid
        {
            get => pid;
            set
            {
                pid = value;
            }
        }


        private string jump;
        public string Jump
        {
            set => jump = value;
            get => jump;
        }

        private string title;
        public string Title
        {
            get => title;
            set
            {
                title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }

        private int index;
        public int Index
        {
            get => index;
            set 
            { 
                index = value;
                RaisePropertyChanged(nameof(Index));
                if (index == 0 && page > 1 && totallPage > 1)
                    Up();
                if (index >= PostCollection.Count-1 && page < totallPage)
                    Down();
            }
        }

        private bool loaded;
        public bool Loaded
        {
            get => loaded;
            set => Set(nameof(Loaded), ref loaded, value);
        }

        private bool enter;
        private int page;
        private int totallPage = 1;
        private string formhash;
        private string fid;

        private ObservableCollection<Post> postCollection;
        public ObservableCollection<Post> PostCollection
        {
            get { return postCollection; }
            set
            {
                postCollection = value;
                RaisePropertyChanged(nameof(PostCollection));
            }
        }

        private RelayCommand<object> replyPost;
        public RelayCommand<object> ReplyPost
        {
            get
            {
                if (replyPost == null)
                {
                    replyPost = new RelayCommand<object>(obj => Goto());
                }
                return replyPost;
            }
            set
            {
                replyPost = value;
                RaisePropertyChanged(nameof(replyPost));
            }
        }

        public TieViewModel()
        {
            postCollection = new ObservableCollection<Post>();
            page = 1;
        }

        public void Up()
        {
            page--;
            Load();
        }
        public void Down()
        {
            page++;
            Load();
        }

        private void Goto()
        {
            Shell.Current.GoToAsync($"ReplyView?tid={tid}&fid={fid}&formhash={formhash}");
        }



        private async void Load()
        {
            if (Loaded) return;

            string html;
            Loaded = true;
            if (jump == "true")
            {
                html = await HttpServer.Instance.GetHtml($"forum.php?mod=redirect&goto=findpost&ptid={tid}&pid={pid}");
            }
            else
                html = await HttpServer.Instance.GetHtml($"forum.php?mod=viewthread&tid={tid}&extra=page%3D1&page={page}");

            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            try
            {
                Title = doc.DocumentNode.SelectSingleNode("//h1").InnerText.Replace("\r\n", "");
                var posts = doc.DocumentNode.SelectNodes("//div[contains(@id, 'post_')and@style='margin-bottom:10px;']");
                formhash = doc.DocumentNode.SelectSingleNode("//input[@name='formhash']")?.GetAttributeValue("value", "null");
                fid = doc.DocumentNode.SelectSingleNode("//input[@name='srhfid']")?.GetAttributeValue("value", "null");

                var pageNode = doc.DocumentNode.SelectSingleNode("//div[@class='pg']/label/span");
                if (pageNode != null)
                {
                    var p = pageNode.GetAttributeValue("title", "1");
                    var re = Regex.Match(p, "[0-9]+", RegexOptions.Multiline).Value;
                    totallPage = int.Parse(re);
                }
                PostCollection.Clear();
                foreach (var post in posts)
                {
                    var authormsg = post.SelectSingleNode(".//div[contains(@id, 'favatar')]");
                    var name = authormsg.SelectSingleNode(".//div[@class='authi']").InnerText;
                    var Floor = post.SelectSingleNode(".//div[@class='pi']//em").InnerText;
                    HtmlWebViewSource htmlWebViewSource = new HtmlWebViewSource();
                    htmlWebViewSource.Html = post.SelectSingleNode(".//div[@class='pct']//table[@cellspacing]").InnerHtml;
                    var pid = post.GetAttributeValue("id", "null").Replace("post_","");
                    PostCollection.Add(new Post() { Author = name , Html = htmlWebViewSource, Pid = pid , Floor = Floor});
                }

                if(jump == "true")
                {
                    var pas = doc.DocumentNode.SelectSingleNode("//div[@class='pg']/strong");
                    if(int.TryParse(pas.InnerText, out int pass))
                    {
                        page = pass;
                    }
                }
                if(page > 1 && totallPage > 1)
                {
                    HtmlWebViewSource htmlWebViewSource = new HtmlWebViewSource();
                    htmlWebViewSource.Html = "<h1>当你看到这一行就说明你在读取上一页，请耐心等待</h1>";
                    PostCollection.Insert(0, new Post() { Html = htmlWebViewSource });
                }
                if(page < totallPage)
                {
                    HtmlWebViewSource htmlWebViewSource = new HtmlWebViewSource();
                    htmlWebViewSource.Html = "<h1>当你看到这一行就说明你在读取下一页，请耐心等待</h1>";
                    PostCollection.Add(new Post() { Html = htmlWebViewSource });
                }
                if(jump == "true")
                {
                    Index = GetIndex(pid);
                    jump = "";
                }
                else
                {
                    if (page == 1)
                        Index = 0;
                    else
                        Index = 1;
                }


                Loaded = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                _ = App.Current.MainPage.DisplayAlert("发生了一个错误",ex.Message + ex.StackTrace, "Ok");
            }

        }

        private int GetIndex(string pid)
        {
            for (int i = 0; i < PostCollection.Count; i++)
            {
                if (PostCollection[i].Pid == pid)
                    return i;
            }
            if (page == 1)
                return 0;
            else
                return 1;
        }
    }
}
