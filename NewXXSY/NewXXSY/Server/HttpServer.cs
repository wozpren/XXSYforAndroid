using Android.Webkit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NewXXSY.Server
{
    public class HttpServer
    {

        public static string url = "https://xxsy.su/";
        #nullable disable
        public static HttpServer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HttpServer();
                }
                return instance;
            }
        }
        private static HttpServer instance;

        #nullable enable
        public string? GetCookie()
        {
            if (Application.Current.Properties.TryGetValue("Cookie", out object cookie))
                return cookie as string;
            else
                return null;
        }

        public void SetCookie(string cookie)
        {
            RemoveCookie();
            Application.Current.Properties.Add("Cookie", cookie);
        }

        public void RemoveCookie()
        {
            if(Application.Current.Properties.ContainsKey("Cookie"))
            {
                Application.Current.Properties.Remove("Cookie");
            }
            Application.Current.SavePropertiesAsync();
        }
#nullable disable


        public HttpClient client;



        public HttpServer()
        {
            var handler = new HttpClientHandler() { UseCookies = true };
            client = new HttpClient(handler);

            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            client.DefaultRequestHeaders.Add("Accept-Language", "en");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36 Edg/97.0.1072.69");
            client.DefaultRequestHeaders.Add("Cookie", GetCookie());

        }


        public async Task<string> GetHtml()
        {
            string rspText = await client.GetStringAsync("https://www.xxsy.su/");
            return rspText;
        }
    }



}
