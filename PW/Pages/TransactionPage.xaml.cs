using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PW
{
	public partial class TransactionPage : ContentPage
	{
		TransactionViewModel viewmodel;
		public TransactionPage(TransactionViewModel viewmodel)
		{
			
			InitializeComponent();
			this.viewmodel = viewmodel;
			BindingContext = this.viewmodel;
		}
	}
}
