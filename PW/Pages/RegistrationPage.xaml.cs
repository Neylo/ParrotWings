using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PW
{
	public partial class RegistrationPage : ContentPage
	{
		RegistrationViewModel viewModel;

		public RegistrationPage()
		{
			InitializeComponent();
			BindingContext = viewModel = new RegistrationViewModel(this);
		}
	}
}
