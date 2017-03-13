using System;
using MvvmHelpers;
using Xamarin.Forms;

namespace PW
{
	public class UsersListViewModel:ViewModelBase
	{
		private ObservableRangeCollection<User> filteredUsers { get; set; }
		public ObservableRangeCollection<User> FilteredUsers
		{
			get { return filteredUsers; }
			set { filteredUsers = value; OnPropertyChanged("FilteredUsers"); }
		}
		DataTransaction data;

		public UsersListViewModel(Page page) : base(page)
		{
			Title = "Select User";
			data = new DataTransaction();
		}

		private string _Filter;

		public string Filter
		{
			get
			{
				return _Filter;
			}
			set
			{
				if (_Filter != value)
				{
					_Filter = value;
					OnPropertyChanged("Filter");
					FilterUsers();
				}
			}
		}

		private async void FilterUsers()
		{
			var UsersList = await data.GetUsersAsync(Filter);
			if (UsersList != null)
				FilteredUsers = new ObservableRangeCollection<User>(UsersList);
		}

		public Action<User> ItemSelected { get; set; }


		User selectedUser;
		public User SelectedUser
		{
			get { return selectedUser; }
			set
			{
				selectedUser = value;
				OnPropertyChanged("SelectedUser");
				if (selectedUser == null)
					return;

				if (ItemSelected == null)
				{
					page.Navigation.PushAsync(new NewTransactionPage(selectedUser));
					SelectedUser = null;
				}
				else
				{
					ItemSelected.Invoke(selectedUser);
				}
			}
		}
	}
}
