using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PW
{
	public partial class MainPage : ContentPage
	{
		MainPageViewModel viewModel;
		public Action<TransactionViewModel> ItemSelected
		{
			get { return viewModel.ItemSelected; }
			set { viewModel.ItemSelected = value; }
		}

		public MainPage()
		{
			InitializeComponent();
			BindingContext = viewModel = new MainPageViewModel(this);
		}

		protected override void OnAppearing()
		{

			base.OnAppearing();
			viewModel.UpdateBalance();
			if (viewModel.Transactions.Count > 0 || viewModel.IsBusy)
				return;
			viewModel.GetTransactionsCommand.Execute(null);

		}

	}
}
