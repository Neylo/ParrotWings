using System;
using Newtonsoft.Json;
namespace PW
{
	public class LoggedUserModel
	{
		public UserInfoToken user_info_token;
	}
	public class UserInfoToken
	{
		
		public int Id { get; set; }

		public string Name { get; set; }

		public string Email { get; set; }

		public double Balance { get; set; }
	}
}
