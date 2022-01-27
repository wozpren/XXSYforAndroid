using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewXXSY.Models
{
    public class Ads: ViewModelBase
    {
        private string name;
        public string Name
        {
            get { return name; }
            set 
            {
                name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }


        public string Url { get; set; }
    }
}
