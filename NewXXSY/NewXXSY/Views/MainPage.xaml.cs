using NewXXSY.Models;
using NewXXSY.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NewXXSY.Views
{


    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        HttpClient client = new HttpClient();

        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = new MainViewModel();
            platelist.DataSource.GroupDescriptors.Add(new Syncfusion.DataSource.GroupDescriptor()
            {
                PropertyName = "Group",
                KeySelector = o => (o as Plate).Group
            });

        }
    }
}