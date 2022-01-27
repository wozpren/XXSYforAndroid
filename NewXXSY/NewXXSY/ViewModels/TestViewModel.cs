using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewXXSY.ViewModels
{
    class TestViewModel : ViewModelBase
    {
        private RelayCommand xPath;
        public RelayCommand XPath
        {
            get 
            { 
                if(xPath == null)
                {
                    xPath = new RelayCommand(AnalysisHtml);
                }
                return xPath;
            }
            set
            {
                xPath = value;
            }
        }




        public void AnalysisHtml()
        {

        }

    }
}
