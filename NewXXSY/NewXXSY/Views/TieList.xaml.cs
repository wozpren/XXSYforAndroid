﻿using NewXXSY.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NewXXSY.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TieList : ContentPage
	{
		public TieList ()
		{
			InitializeComponent ();
			BindingContext = new TieListViewModel();
		}
	}
}