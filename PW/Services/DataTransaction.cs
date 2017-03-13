using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PW
{
	public class DataTransaction
	{
		public async Task<IEnumerable<User>> GetUsersAsync(string filter)
		{
			var json = await API.GetFilteredUsersList(UserInfo.Token, filter);
			if(json!="error")
				return await Task.Run(() => JsonConvert.DeserializeObject<List<User>>(json));
			return null;
		}

		public async Task<IEnumerable<TransactionToken>> GetTransactionsAsync()
		{
			var json = await API.GetTransactionList(UserInfo.Token);
			if (json != "error")
				return await Task.Run(() => (IEnumerable<TransactionToken>)JsonConvert.DeserializeObject<TransactionListModel>(json).trans_token);
			return null;
		}

		public async Task<TransactionModel> SendTransactionAsync(string name, double amount)
		{
			var json = await API.SendTransaction(name, amount, UserInfo.Token);
			if (json != "error")
				return await Task.Run(() => JsonConvert.DeserializeObject<TransactionModel>(json));
			return null;
		}

	}
}
