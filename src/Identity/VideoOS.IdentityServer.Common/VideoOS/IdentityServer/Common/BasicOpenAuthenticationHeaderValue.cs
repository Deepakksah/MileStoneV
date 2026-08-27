using System;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;

namespace VideoOS.IdentityServer.Common;

public class BasicOpenAuthenticationHeaderValue : AuthenticationHeaderValue
{
	public BasicOpenAuthenticationHeaderValue(string userName, string password)
		: base("Basic", EncodeCredential(userName, password))
	{
	}

	private static string EncodeCredential(string userName, string password)
	{
		if (string.IsNullOrWhiteSpace(userName))
		{
			throw new ArgumentNullException("userName");
		}
		if (password == null)
		{
			password = "";
		}
		Encoding uTF = Encoding.UTF8;
		string s = string.Format(CultureInfo.InvariantCulture, "{0}:{1}", UrlEncode(userName), UrlEncode(password));
		return Convert.ToBase64String(uTF.GetBytes(s));
	}

	private static string UrlEncode(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return string.Empty;
		}
		return Uri.EscapeDataString(value).Replace("%20", "+");
	}
}
