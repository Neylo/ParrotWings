using System;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PW
{
	public class TransactionViewModel : ViewModelBase
	{
		private double loggedUserBalance;
		public double LoggedUserBalance
		{
			get
			{
				return loggedUserBalance;
			}
			set
			{
				if (loggedUserBalance != value)
				{
					loggedUserBalance = value;
					OnPropertyChanged("LoggedUserBalance");
				}
			}
		}
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public string UserName { get; set; }
		public double Amount { get; set; }
		public double Balance { get; set; }

		public bool IsCanRepeatTransacion
		{
			get
			{
				if (Amount > 0)
					return false;
				return true;
			}
		}

		public string Status
		{
			get
			{
				if (Amount > 0)
					return "From: ";
				return "To: ";
			}
		}

		public string ShortDate
		{
			get
			{
				return Date.ToString("g");
			}
		}

		public string TitleStatus
		{
			get
			{
				if (Amount > 0)
					return "Incoming Transaction";
				return "Outgoing Transaction";
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

		DataTransaction data;
				

		public TransactionViewModel(Page page, TransactionToken transaction) : base(page)
		{
			Id = transaction.Id;
			Date = transaction.Date;
			UserName = transaction.UserName;
			Amount = transaction.Amount;
			Balance = transaction.Balance;
			Title = "Transaction № " + Id.ToString();
			data = new DataTransaction();
			UpdateBalance();
		}

		private Command repeatTransaction;
		public Command RepeatTransaction
		{
			get
			{
				return repeatTransaction = new Command(async () => await ExecuteRepeatTransaction());
			}
		}

		private async Task ExecuteRepeatTransaction()
		{
			if (IsBusy)
				return;
			RepeatTransaction.ChangeCanExecute();
			IsBusy = true;
			ErrorText = "";
			try
			{
				await UserInfo.UpdateInfo();
				var SumToSend = Amount*-1;
				if (SumToSend > UserInfo.Balance)
				{
					ErrorText = "You don't have enough money";
					return;
				}
				var transaction = new TransactionModel();
				if (await page.DisplayAlert("Warning", "Are you sure you want to transfer " + SumToSend + "$ to " + UserName, "Yes", "No"))
				{
					transaction = await data.SendTransactionAsync(UserName, SumToSend);
					if (transaction == null)
					{
						ErrorText = "There was an error, please try again.";
						return;
					}
					await page.DisplayAlert("Succesful", "You sent " + SumToSend + "$ to " + UserName, "OK");
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
				RepeatTransaction.ChangeCanExecute();
			}


		}
		public async void UpdateBalance()
		{
			await UserInfo.UpdateInfo();
			LoggedUserBalance = UserInfo.Balance;
		}

	}
}
