using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NewXXSY.Models
{
    public class Plate
    {
        public string Name { get; set; }
        public UriImageSource Image { get; set; }
        public string Group { get; set; }
        public string Fid { get; set; }


        public string Topic { get; set; }
        public string New { get; set; }
        public string Today { get; set; }
    }
}
