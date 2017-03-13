using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PW
{
	public partial class NewTransactionPage : ContentPage
	{
		NewTransactionViewModel viewModel;

		public NewTransactionPage(User user)
		{
			InitializeComponent();
			BindingContext = viewModel = new NewTransactionViewModel(this,user);
		}
	}
}
