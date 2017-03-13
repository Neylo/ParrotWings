using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Net;

namespace PW
{
	public class RegistrationViewModel : ViewModelBase
	{

		private string userName;

		public string UserName 
		{
			get
			{
				return userName;
			}
			set
			{
				if (userName != value)
				{
					userName = value;
					OnPropertyChanged("UserName");
				}
			}
		}
		private string password;

		public string Password
		{
			get
			{
				return password;
			}
			set
			{
				if (password != value)
				{
					password = value;
					OnPropertyChanged("Password");
				}
			}
		}

		private string passwordRepeat;

		public string PasswordRepeat
		{
			get
			{
				return passwordRepeat;
			}
			set
			{
				if (passwordRepeat != value)
				{
					passwordRepeat = value;
					OnPropertyChanged("PasswordRepeat");
				}
			}
		}

		private string email;

		public string Email
		{
			get
			{
				return email;
			}
			set
			{
				if (email != value)
				{
					email = value;
					OnPropertyChanged("Email");
				}
			}
		}



		private string errorText;

		public string ErrorText
		{
			get
			{
				return errorText;
			}
			set
			{
				if (errorText != value)
				{
					errorText = value;
					OnPropertyChanged("ErrorText");
				}
			}
		}

		private Command registrationCommand;
		public Command RegistrationCommand

		{
			get
			{
				return registrationCommand ??
					(registrationCommand = new Command(async () => await ExecuteRegistrationCommand(), () => { return !IsBusy; }));
			}
		}

		private async Task ExecuteRegistrationCommand()
		{ 
			if (IsBusy)
				return;

			IsBusy = true;


			RegistrationCommand.ChangeCanExecute();

			try
			{
				if (UserName == null)
				{
					ErrorText = "Enter User name";
					return;
				}

				if (!ValidCheker.IsMailCorrect(Email))
				{
					ErrorText = "Enter Correct Email";
					return;
				}

				if (Password == null)
				{
					ErrorText = "Enter Password";
					return;
				}

				if (PasswordRepeat == null)
				{
					ErrorText = "Repeat Password";
					return;
				}

				if (Password != PasswordRepeat)
				{
					ErrorText = "Passwords do not match";
					return;
				}

				var registerStatus = await API.RegisterUser(UserName, Email, Password);
				if (registerStatus == "success")
				{
					ErrorText = "Registration Complete";
					await page.Navigation.PushAsync(new MainPage());
				}
				
				ErrorText = "Registration failed";

			}
			catch (WebException err)
			{
				await page.DisplayAlert("Error", "Internet Connection error", "OK");
			}
			catch (Exception ex)
			{
				await page.DisplayAlert("Ooops ... :(", "Cant to registration", "OK");
			}

			finally
			{
				IsBusy = false;
				RegistrationCommand.ChangeCanExecute();
			}

		}



		public RegistrationViewModel(Page page): base(page)
		{
			Title = "Registration";

		}

	}
}
