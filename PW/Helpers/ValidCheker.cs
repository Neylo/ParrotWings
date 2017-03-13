using System;
using System.Text.RegularExpressions;

namespace PW
{
	public static class ValidCheker
	{
		public static bool IsMailCorrect(string email)
		{
			Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
			Match match = regex.Match(email);

			return match.Success;
		}
	}
}
