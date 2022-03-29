using NewXXSY.Models;
using NewXXSY.Server;
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

            MessagingCenter.Subscribe<MainViewModel>(this, "签到开始", (sender) =>
            {
                QianText.Text = "签到中";
                QianBar.IsBusy = true;
            });

            MessagingCenter.Subscribe<MainViewModel, bool>(this, "签到", async (sender, e) =>
            {
                QianText.Text = "签到";
                QianBar.IsBusy = false;

                if (e)
                    await DisplayAlert("签到", "签到成功", "了解");
                else
                    await DisplayAlert("签到", "今日已签", "了解");
            });

            this.BindingContext = new MainViewModel();
            platelist.DataSource.GroupDescriptors.Add(new Syncfusion.DataSource.GroupDescriptor()
            {
                PropertyName = "Group",
                KeySelector = o => (o as Plate).Group
            });

        }


        private async void SfButton_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("退出账号", "确定要退出当前账号吗？\n退出后将会关闭应用。", "是", "否");
            if (answer)
            {
                HttpServer.Instance.RemoveCookie();
                Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            }
        }
    }
}