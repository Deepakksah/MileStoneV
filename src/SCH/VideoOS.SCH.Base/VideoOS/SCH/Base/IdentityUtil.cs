using System;

namespace VideoOS.SCH.Base;

public class IdentityUtil
{
	public static void ExtractUserNameAndDomain(string fullUserName, out string userName, out string domain)
	{
		if (string.IsNullOrEmpty(fullUserName))
		{
			userName = null;
			domain = null;
			return;
		}
		int num = fullUserName.IndexOf("\\", StringComparison.Ordinal);
		if (num < 0)
		{
			userName = fullUserName;
			domain = null;
		}
		else if (num == 0)
		{
			userName = fullUserName.Substring(num + 1);
			domain = null;
		}
		else
		{
			userName = fullUserName.Substring(num + 1);
			domain = fullUserName.Substring(0, num);
		}
	}

	public static string GetFullUserName(string userName, string domain)
	{
		if (string.IsNullOrEmpty(userName))
		{
			return null;
		}
		if (string.IsNullOrEmpty(domain))
		{
			return userName;
		}
		return domain + "\\" + userName;
	}
}
