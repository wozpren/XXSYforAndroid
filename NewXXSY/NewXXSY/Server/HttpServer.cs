using Android.Webkit;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NewXXSY.Server
{
    public class HttpServer
    {
        public event Action<string> LoadNewPage;


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
            return CookieManager.Instance.GetCookie(HttpServer.url);
        }

        public void RemoveCookie()
        {
            CookieManager.Instance.RemoveAllCookie();
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

        public async Task<string> GetHtml(string uri = "")
        {
            try
            {
                string rspText = await client.GetStringAsync("https://www.xxsy.su/"+ uri);
                LoadNewPage?.Invoke(rspText);
                return rspText;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        public async Task<string> GetAsync(string uri)
        {
            HttpRequestMessage reqMsg = new HttpRequestMessage(HttpMethod.Get, "https://www.xxsy.su/" + uri)
            {
                Version = HttpVersion.Version11,
            };
            HttpResponseMessage rspMsg = null;
            try
            {
                rspMsg = await client.SendAsync(reqMsg).ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (Exception ex)
            {
                rspMsg?.Dispose();
                Console.WriteLine(ex);
                return null;
            }
            using (rspMsg)
            {
                if (!rspMsg.IsSuccessStatusCode)
                {
                    return null;
                }
                var urls = rspMsg.Headers.GetValues("location");
                foreach (var url in urls)
                {
                    Console.WriteLine(url);
                }

            }
            return null;
        }

        public async Task<string> PostAsync(string uri, HttpContent content)
        {
            HttpRequestMessage reqMsg = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = content,
                Version = HttpVersion.Version11,
            };

            reqMsg.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            reqMsg.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36 Edg/97.0.1072.69");
            reqMsg.Headers.Add("Cookie", GetCookie());

            HttpResponseMessage rspMsg = null;
            try
            {
                rspMsg = await client.SendAsync(reqMsg).ConfigureAwait(continueOnCapturedContext: false);
            }
            catch(Exception ex)
            {
                rspMsg?.Dispose();
                Console.WriteLine(ex);
                return null;
            }
            using (rspMsg)
            {
                if (!rspMsg.IsSuccessStatusCode)
                {
                    return null;
                }
                return await rspMsg.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
            }
        }
    }



}
