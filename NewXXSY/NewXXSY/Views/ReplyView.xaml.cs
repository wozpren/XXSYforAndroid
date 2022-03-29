using NewXXSY.Server;
using NewXXSY.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NewXXSY.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReplyView : ContentPage
    {

		public ReplyView()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<ReplyViewModel>(this, "回帖开始", (sender) =>
            {
                text.Text = "回帖中";
                busy.IsBusy = true;
            });

            MessagingCenter.Subscribe<ReplyViewModel>(this, "回帖结束", (sender) =>
            {
                text.Text = "回帖";
                busy.IsBusy = false;

                Shell.Current.SendBackButtonPressed();
            });

            BindingContext = new ReplyViewModel();
        }



	}
}