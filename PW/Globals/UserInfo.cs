using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PW
{
	public static class UserInfo
	{
		[JsonIgnore]
		public static string Token { get; set; }

		public static int Id { get; set; }

		public static string Name { get; set; }

		public static string Email { get; set; }

		public static double Balance { get; set; }

		public static async Task UpdateInfo()
		{
			var userInfoJson = await API.GetLoggedUserInfo(Token);
			if (userInfoJson != "error")
			{
				var userinfo = await Task.Run(() => JsonConvert.DeserializeObject<LoggedUserModel>(userInfoJson));
				Name = userinfo.user_info_token.Name;
				Email = userinfo.user_info_token.Email;
				Id = userinfo.user_info_token.Id;
				Balance = userinfo.user_info_token.Balance;
			}
		}

	}


}
