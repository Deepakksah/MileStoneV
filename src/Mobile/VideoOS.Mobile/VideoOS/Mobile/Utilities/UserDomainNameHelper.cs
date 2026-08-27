using System;
using System.DirectoryServices.ActiveDirectory;
using System.Security.Principal;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Utilities;

public static class UserDomainNameHelper
{
	public static string GetDomain(string userDomainName, bool check = false)
	{
		int num = userDomainName.LastIndexOf('\\');
		string text = string.Empty;
		if (num >= 0)
		{
			text = userDomainName.Substring(0, num);
			if (check)
			{
				text = CheckDomain(text);
			}
		}
		return text;
	}

	public static string GetUser(string userDomainName)
	{
		int num = userDomainName.LastIndexOf('\\');
		if (num < 0)
		{
			return userDomainName;
		}
		return userDomainName.Substring(num + 1, userDomainName.Length - num - 1);
	}

	public static string CheckDomain(string domain)
	{
		string result = string.Empty;
		try
		{
			result = Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain, domain)).Name;
		}
		catch (Exception)
		{
		}
		return result;
	}

	public static string CheckDomain(string domain, string username, string password)
	{
		string result = string.Empty;
		try
		{
			result = Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain, domain, username, password)).Name;
		}
		catch (Exception)
		{
		}
		return result;
	}

	public static string Normalize(string userDomain)
	{
		string domain = GetDomain(userDomain, check: true);
		string text = GetUser(userDomain);
		if (!string.IsNullOrEmpty(domain))
		{
			text = domain + "\\" + text;
		}
		return text;
	}

	public static string DetermineSystemAccountName(WellKnownSidType type)
	{
		string result = string.Empty;
		try
		{
			result = ((NTAccount)new SecurityIdentifier(type, null).Translate(typeof(NTAccount))).ToString();
		}
		catch (Exception ex)
		{
			Log.Error("UserDomainNameHelper.DetermineSystemAccountName", ex.Message);
		}
		return result;
	}
}
