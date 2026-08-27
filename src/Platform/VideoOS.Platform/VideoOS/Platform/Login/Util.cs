using System;
using System.Net;
using System.Net.Sockets;
using System.Security;

namespace VideoOS.Platform.Login;

public static class Util
{
	private static string _domainPostFix;

	static Util()
	{
		_domainPostFix = "";
		try
		{
			string hostName = Dns.GetHostEntry("").HostName;
			string hostName2 = Dns.GetHostName();
			if (hostName.Length > hostName2.Length)
			{
				_domainPostFix = hostName.Substring(hostName2.Length);
			}
		}
		catch (SocketException)
		{
		}
	}

	public static CredentialCache BuildCredentialCache(Uri uri, string username, string password, string authenticationType)
	{
		if (password == null)
		{
			throw new ArgumentNullException("password");
		}
		if (authenticationType == "External")
		{
			return new CredentialCache();
		}
		SecureString secureString = new SecureString();
		foreach (char c in password)
		{
			secureString.AppendChar(c);
		}
		secureString.MakeReadOnly();
		return BuildCredentialCache(uri, username, secureString, authenticationType);
	}

	public static CredentialCache BuildCredentialCache(Uri uri, string userName, SecureString securePassword, string authenticationType)
	{
		if (uri == null)
		{
			throw new ArgumentNullException("uri");
		}
		if (userName == null)
		{
			throw new ArgumentNullException("userName");
		}
		CredentialCache credentialCache = new CredentialCache();
		Uri uriPrefix = ((!(uri.Scheme == Uri.UriSchemeHttps)) ? new UriBuilder(Uri.UriSchemeHttp, uri.Host, uri.Port).Uri : ((!uri.IsDefaultPort) ? new UriBuilder(Uri.UriSchemeHttps, uri.Host, uri.Port).Uri : new UriBuilder(Uri.UriSchemeHttps, uri.Host).Uri));
		switch (authenticationType)
		{
		case "Basic":
			if (userName.StartsWith("[BASIC]", StringComparison.OrdinalIgnoreCase))
			{
				uriPrefix = ((uri.Port == 0 || uri.Port == 80) ? new UriBuilder(Uri.UriSchemeHttps, uri.Host).Uri : new UriBuilder(Uri.UriSchemeHttps, uri.Host, uri.Port).Uri);
				credentialCache.Add(uriPrefix, "Basic", new NetworkCredential(userName.Substring("[BASIC]\\".Length), securePassword, "[BASIC]"));
			}
			else
			{
				credentialCache.Add(uriPrefix, "Basic", new NetworkCredential(userName, securePassword));
			}
			break;
		case "Negotiate":
		{
			bool flag = false;
			string[] array = userName.Split('\\');
			string domain = "";
			if (array.Length == 1 && array[0].Length > 0)
			{
				userName = array[0];
			}
			else if (array.Length == 2)
			{
				domain = array[0];
				userName = array[1];
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				credentialCache.Add(uriPrefix, "Negotiate", CredentialCache.DefaultNetworkCredentials);
			}
			else
			{
				credentialCache.Add(uriPrefix, "Negotiate", new NetworkCredential(userName, securePassword, domain));
			}
			break;
		}
		}
		return credentialCache;
	}

	public static SecureString ToSecureString(this string unsecureString)
	{
		if (unsecureString == null)
		{
			return null;
		}
		SecureString secureString = new SecureString();
		foreach (char c in unsecureString)
		{
			secureString.AppendChar(c);
		}
		secureString.MakeReadOnly();
		return secureString;
	}

	public static NetworkCredential BuildNetworkCredential(Uri uri, string username, string password, string authenticationType)
	{
		if (password == null)
		{
			throw new ArgumentNullException("password");
		}
		return BuildNetworkCredential(username, password.ToSecureString(), authenticationType);
	}

	public static NetworkCredential BuildNetworkCredential(string userName, SecureString securePassword, string authenticationType)
	{
		if (userName == null)
		{
			throw new ArgumentNullException("userName");
		}
		NetworkCredential result = CredentialCache.DefaultNetworkCredentials;
		switch (authenticationType)
		{
		case "Basic":
			result = new NetworkCredential(userName, securePassword);
			break;
		case "Negotiate":
		{
			bool flag = false;
			string[] array = userName.Split('\\');
			string domain = "";
			if (array.Length == 1 && array[0].Length > 0)
			{
				userName = array[0];
			}
			else if (array.Length == 2)
			{
				domain = array[0];
				userName = array[1];
			}
			else if (string.IsNullOrEmpty(userName))
			{
				flag = true;
			}
			result = ((!flag) ? new NetworkCredential(userName, securePassword, domain) : CredentialCache.DefaultNetworkCredentials);
			break;
		}
		case "External":
			result = null;
			break;
		}
		return result;
	}

	public static NetworkCredential GetCredential(ServerId serverId)
	{
		LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(serverId);
		if (loginSettings != null)
		{
			return loginSettings.NetworkCredential;
		}
		return EnvironmentManager.Instance.LoginNetworkCredential;
	}

	public static string GetHostName(string mixedUrl)
	{
		if (mixedUrl == null)
		{
			return "localhost";
		}
		mixedUrl = mixedUrl.ToLowerInvariant();
		if (mixedUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
		{
			mixedUrl = mixedUrl.Substring("http://".Length);
		}
		if (mixedUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
		{
			mixedUrl = mixedUrl.Substring("https://".Length);
		}
		if (mixedUrl.EndsWith("/", StringComparison.OrdinalIgnoreCase))
		{
			mixedUrl = mixedUrl.Substring(0, mixedUrl.Length - 1);
		}
		int num = mixedUrl.Split(':').Length;
		if (mixedUrl.StartsWith("[", StringComparison.Ordinal) || num > 2)
		{
			if (mixedUrl.StartsWith("[", StringComparison.Ordinal) && mixedUrl.EndsWith("]", StringComparison.Ordinal))
			{
				mixedUrl = mixedUrl.Substring(1, mixedUrl.Length - 2);
			}
			return mixedUrl.ToUpperInvariant();
		}
		int num2 = mixedUrl.IndexOf(":", StringComparison.Ordinal);
		if (num2 > 0)
		{
			mixedUrl = mixedUrl.Substring(0, num2);
		}
		return mixedUrl;
	}

	public static string GetHostNameWithoutDomain(string mixedUrl)
	{
		string text = GetHostName(mixedUrl);
		if (text.EndsWith(_domainPostFix, StringComparison.OrdinalIgnoreCase))
		{
			text = text.Substring(0, text.Length - _domainPostFix.Length);
		}
		return text;
	}
}
