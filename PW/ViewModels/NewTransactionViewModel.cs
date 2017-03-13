using System;
using Xamarin.Forms;
using MvvmHelpers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;

namespace PW
{
	public class NewTransactionViewModel:ViewModelBase
	{

		DataTransaction data;

		public string Name { get; set; }

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

		private double balance;
		public double Balance
		{
			get
			{
				return balance;
			}
			set
			{
				if (balance != value)
				{
					balance = value;
					OnPropertyChanged("Balance");
				}
			}
		}

		private string amount;
		public string Amount
		{
			get
			{
				return amount;
			}
			set
			{
				double amountValue;
				if (value == "")
					ErrorText = "";
				if (amount != value)
				{
					if (Double.TryParse(value, out amountValue)&&amountValue>0)
					{
						amount = value;
						OnPropertyChanged("Amount");
					}
					else
					{
						ErrorText = "Enter positive numeric value";
					}
				}
			}
		}

		public NewTransactionViewModel(Page page, User user) : base(page)
		{
			Name = user.Name;
			data = new DataTransaction();
			Title = "New Transaction";
			UpdateBalance();

		}

		private Command sendTransaction;
		public Command SendTransaction
		{
			get
			{
				return sendTransaction = new Command(async () => await ExecuteSendTransaction());
			}
		}

		private async Task ExecuteSendTransaction()
		{
			if (IsBusy)
				return;
			SendTransaction.ChangeCanExecute();
			IsBusy = true;
			ErrorText = "";
			try
			{
				await UserInfo.UpdateInfo();
				var SumToSend = Double.Parse(Amount);
				if (SumToSend > UserInfo.Balance)
				{
					ErrorText = "You don't have enough money";
					return;
				}
				if (await page.DisplayAlert("Warning", "Are you sure you want to transfer " + SumToSend + "$ to " + Name, "Yes", "No"))
				{
					var transaction = await data.SendTransactionAsync(Name, SumToSend);
					if (transaction == null)
					{
						ErrorText = "There was an error, please try again.";
						return;
					}

					await page.DisplayAlert("Succesful", "You sent " + SumToSend + "$ to " + Name, "OK");
					await page.Navigation.PushAsync(new MainPage());
				}
				      
			}
			catch (WebException err)
			{
				await page.DisplayAlert("Error", "Internet Connection error", "OK");
			}
			catch (Exception ex)
			{
				await page.DisplayAlert("Ooops ... :(", "Cant to send transaction", "OK");
			}

			finally
			{
				IsBusy = false;
				SendTransaction.ChangeCanExecute();
			}

				
		}

		public async void UpdateBalance()
		{
			await UserInfo.UpdateInfo();
			Balance = UserInfo.Balance;
		}
	}
}
