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
        public string Url { get; set; }
    }
}
