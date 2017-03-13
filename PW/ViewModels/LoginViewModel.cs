using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace PW
{



	public class LoginViewModel: ViewModelBase
	{

		public LoginViewModel(Page page) : base(page)
		{
			Title = "Parrot Wings";
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

		private Command signUp;
		public Command SignUp
		{
			get
			{
				return signUp = new Command(async () => await page.Navigation.PushAsync(new RegistrationPage()));
			}
		}


		private Command signIn;
		public Command SignIn
		{
			get
			{
				return signIn = new Command(async () => await ExecuteSignInComand());
			}
		}

		private async Task ExecuteSignInComand()
		{
			if (IsBusy)
				return;
			IsBusy = true;
			SignIn.ChangeCanExecute();
			try
			{
				if (String.IsNullOrEmpty(Email))
				{
					await page.DisplayAlert("Wrond Data", "Please, enter your email", "Ok");
					return;
				}
				if (String.IsNullOrEmpty(Password))
				{
					await page.DisplayAlert("Wrond Data", "Please, enter your password", "Ok");
					return;
				}
				var LoginStatus = await API.LoginUser(Email, Password);

				if (LoginStatus != "success")
				{

				await page.DisplayAlert("Error", "Invalid email or password", "Ok");
					return;

				}

				await UserInfo.UpdateInfo();
				await page.Navigation.PushAsync(new MainPage());
			

			}
			catch (WebException err)
			{
				await page.DisplayAlert("Error", "No internet connection", "Ок");
			}

			catch (Exception ex)
			{
				await page.DisplayAlert("Error", "Try to login again", "Ок");
			}
			finally
			{
				IsBusy = false;
				signIn.ChangeCanExecute();
			}

		}



		}


	}

