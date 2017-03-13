using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PW
{
	public partial class UsersListPage : ContentPage
	{
		UsersListViewModel viewModel;
		public Action<User> ItemSelected
		{
			get { return viewModel.ItemSelected; }
			set { viewModel.ItemSelected = value; }
		}

		public UsersListPage()
		{
			InitializeComponent();
			BindingContext = viewModel = new UsersListViewModel(this);
		}
	}
}
