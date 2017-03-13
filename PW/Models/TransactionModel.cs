using System;
using System.Collections.Generic;
namespace PW
{

	public class TransactionModel
	{
		public TransactionToken trans_token;
	}
	public class TransactionListModel
	{
		public List<TransactionToken> trans_token;
	}
	public class TransactionToken
	{
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public string UserName { get; set; }
		public double Amount { get; set; }
		public double Balance { get; set; }
	}


}
