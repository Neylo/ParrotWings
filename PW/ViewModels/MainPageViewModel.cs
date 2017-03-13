using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MvvmHelpers;
using Xamarin.Forms;


namespace PW
{
	public class MainPageViewModel:ViewModelBase
	{
		DataTransaction data;
		private ObservableRangeCollection<TransactionViewModel> transactions;
		public ObservableRangeCollection<TransactionViewModel> Transactions
		{
			get { return transactions; }
			set { transactions = value; OnPropertyChanged("Transactions"); }
		}
		private ObservableRangeCollection<TransactionViewModel> transactionsFiltered;
		public ObservableRangeCollection<TransactionViewModel> TransactionsFiltered
		{
			get { return transactionsFiltered; }
			set { transactionsFiltered = value; OnPropertyChanged("TransactionsFiltered"); }
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


		public MainPageViewModel(Page page) : base(page)
		{
			Title = "My transactions";
			data = new DataTransaction();
			Transactions = new ObservableRangeCollection<TransactionViewModel>();
			Balance = UserInfo.Balance;

		}

		private string filterByName;

		public string FilterByName
		{
			get
			{
				return filterByName;
			}
			set
			{
				if (filterByName != value)
				{
					filterByName = value;
					OnPropertyChanged("FilterByName");
					this.FilterTransactionsByName();
				}
			}
		}

		private void FilterTransactionsByName()
		{
			if (Transactions != null)
			{
				if (String.IsNullOrEmpty(filterByName))
				{
					TransactionsFiltered = new ObservableRangeCollection<TransactionViewModel>(SortTransactionsByDate());
				}
				else
				{
					var sortedByName = Transactions.Where((arg) => arg.UserName.ToLower().Contains(filterByName.ToLower()));
					TransactionsFiltered.ReplaceRange(sortedByName);
				}
			}
		}

		private Command newTransaction;
		public Command NewTransaction
		{
			get
			{
				return newTransaction = new Command(async () => await page.Navigation.PushAsync(new UsersListPage()));
			}
		}

		private Command getTransactionsCommand;
		public Command GetTransactionsCommand
		{
			get
			{
				return getTransactionsCommand = new Command(async () => await ExecuteGetTransactionsCommand());
			}
		}

		private async Task ExecuteGetTransactionsCommand()
		{
			if (IsBusy)
				return;

			IsBusy = true;
			GetTransactionsCommand.ChangeCanExecute();
			var showAlert = false;
			try
			{
				UpdateBalance();
				Transactions.Clear();

				var trans = await data.GetTransactionsAsync();
				foreach (var transaction in trans)
				{
					Transactions.Add(new TransactionViewModel(page, transaction));
				}
				TransactionsFiltered = new ObservableRangeCollection<TransactionViewModel>(SortTransactionsByDate());
			}
			catch (WebException err)
			{
				await page.DisplayAlert("Ошибка", "У вас отсутствует подключение к интернету", "OK");
			}
			catch (Exception ex)
			{
				showAlert = true;

			}
			finally
			{
				IsBusy = false;
				GetTransactionsCommand.ChangeCanExecute();
			}

			if (showAlert)
				await page.DisplayAlert("Упс ... :(", "Не получается получить мероприятия.", "OK");


		}

		public Action<TransactionViewModel> ItemSelected { get; set; }

		TransactionViewModel selectedTransaction;
		public TransactionViewModel SelectedTransaction
		{
			get { return selectedTransaction; }
			set
			{
				selectedTransaction = value;
				OnPropertyChanged("SelectedTransaction");
				if (selectedTransaction == null)
					return;

				if (ItemSelected == null)
				{
					page.Navigation.PushAsync(new TransactionPage(selectedTransaction));
					SelectedTransaction = null;
				}
				else
				{
					ItemSelected.Invoke(selectedTransaction);
				}
			}
		}

		Command refreshCommand;
		public Command RefreshCommand
		{
			get
			{
				return refreshCommand ??
				(refreshCommand = new Command(async () => await ExecuteGetTransactionsCommand(), () => { return !IsBusy; }));
			}
		}

		private IEnumerable<TransactionViewModel> SortTransactionsByDate()
		{
			var sorted = Transactions.OrderByDescending((arg) => arg.Date);
			return sorted as IEnumerable<TransactionViewModel>;
		}

		public async void UpdateBalance()
		{
			await UserInfo.UpdateInfo();
			Balance = UserInfo.Balance;
		}

	}
}
