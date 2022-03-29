using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NewXXSY.Models
{
    public class Post
    {
        public string Author { get; set; }
        public string Pid { get; set; }
        public string Floor { get; set; }
        public HtmlWebViewSource Html { get; set; }
    }
}
