using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace PW
{
	public static class API
	{
		public static string baseUrl = "http://193.124.114.46:3001";
		public static string registerUrl = baseUrl + "/users";
		public static string loginUrl = baseUrl + "/sessions/create";
		public static string transactionListUrl = baseUrl + "/api/protected/transactions";
		public static string transactionSendUrl = baseUrl + "/api/protected/transactions";
		public static string userInfoUrl = baseUrl + "/api/protected/user-info";
		public static string filteredUsersListUrl = baseUrl + "/api/protected/users/list";

		public static async Task<string> RegisterUser(string username, string email, string password)
		{
			using (var httpClient = new HttpClient())
			{
				var requestUrl = registerUrl;
				Uri requestUri = new Uri(requestUrl);
				JObject jsonContent = new JObject
					(
						new JProperty("username", username),
						new JProperty("password", password),
						new JProperty("email", email)

					);
				var content = new StringContent(jsonContent.ToString(), Encoding.UTF8, "application/json");

				var response = await httpClient.PostAsync(requestUri, content);

				if (response.StatusCode == HttpStatusCode.Created)
				{
					var responseContent = response.Content;
					var token = await Task.Run(() => responseContent.ReadAsStringAsync().Result);
					UserInfo.Token = await Task.Run(() => JsonConvert.DeserializeObject<AuthToken>(token).Id_token);
					return "success";
				}
				return "error";
			}
		}

		public static async Task<string> LoginUser(string email, string password)
		{
			using (var httpClient = new HttpClient())
			{
				var requestUrl = loginUrl;
				Uri requestUri = new Uri(requestUrl);
				JObject jsonContent = new JObject
					(
						new JProperty("password", password),
						new JProperty("email", email)
					);
				var content = new StringContent(jsonContent.ToString(), Encoding.UTF8, "application/json");

				var response = await httpClient.PostAsync(requestUri, content);

				if (response.StatusCode == HttpStatusCode.Created)
				{
					var responseContent = response.Content;
					var token = await Task.Run(() => responseContent.ReadAsStringAsync().Result);
					UserInfo.Token = await Task.Run(() => JsonConvert.DeserializeObject<AuthToken>(token).Id_token);
					return "success";
				}
				return "error";
			}

		}

		public static async Task<string> GetTransactionList(string token)
		{
			using (var httpClient = new HttpClient())
			{
				var requestUrl = transactionListUrl;
				Uri requestUri = new Uri(requestUrl);

				if (!String.IsNullOrEmpty(token))
				{
					var request = new HttpRequestMessage();
					request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
					request.Method = HttpMethod.Get;
					request.RequestUri = requestUri;
					var response = await httpClient.SendAsync(request);
					if (response.IsSuccessStatusCode)
					{
						var responseContent = response.Content;
						return await Task.Run(() => responseContent.ReadAsStringAsync().Result);
					}
					return "error";
				}
				return "error";
			}

		}

		public static async Task<string> GetFilteredUsersList(string token, string filter)
		{
			using (var httpClient = new HttpClient())
			{
				var requestUrl = filteredUsersListUrl;
				Uri requestUri = new Uri(requestUrl);
				JObject jsonContent = new JObject
					(
						new JProperty("filter", filter)
					);
				if (!String.IsNullOrEmpty(token))
				{
					var request = new HttpRequestMessage();
					request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
					request.Method = HttpMethod.Post;
					request.Content = new StringContent(jsonContent.ToString(), Encoding.UTF8, "application/json");
					request.RequestUri = requestUri;
					var response = await httpClient.SendAsync(request);
					if (response.IsSuccessStatusCode)
					{
						var responseContent = response.Content;
						return await Task.Run(() => responseContent.ReadAsStringAsync().Result);
					}
					return "error";
				}
				return "error";
			}
		}

		public static async Task<string> SendTransaction(string name, double amount, string token)
		{
			using (var httpClient = new HttpClient())
			{
				var requestUrl = transactionSendUrl;
				Uri requestUri = new Uri(requestUrl);
				JObject jsonContent = new JObject
					(
						new JProperty("name", name),
						new JProperty("amount", amount.ToString())
					);
				if (!String.IsNullOrEmpty(token))
				{
					var request = new HttpRequestMessage();
					request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
					request.Method = HttpMethod.Post;
					request.Content = new StringContent(jsonContent.ToString(), Encoding.UTF8, "application/json");
					request.RequestUri = requestUri;
					var response = await httpClient.SendAsync(request);
					if (response.IsSuccessStatusCode)
					{
						var responseContent = response.Content;
						return await Task.Run(() => responseContent.ReadAsStringAsync().Result);
					}
					return "error";
				}
				return "error";
			}
		}

		public static async Task<string> GetLoggedUserInfo(string token)
		{ 
			using (var httpClient = new HttpClient())
			{
				var requestUrl = userInfoUrl;
				Uri requestUri = new Uri(requestUrl);

				if (!String.IsNullOrEmpty(token))
				{
					var request = new HttpRequestMessage();
					request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
					request.Method = HttpMethod.Get;
					request.RequestUri = requestUri;
					var response = await httpClient.SendAsync(request);
					if (response.IsSuccessStatusCode)
					{
						var responseContent = response.Content;
						return await Task.Run(() => responseContent.ReadAsStringAsync().Result);
					}
					return "error";
				}
				return "error";
			}

		}
	}
}
