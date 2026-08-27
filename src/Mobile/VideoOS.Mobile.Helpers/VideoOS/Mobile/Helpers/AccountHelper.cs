using System;
using System.Management;
using System.Security.Principal;

namespace VideoOS.Mobile.Helpers;

public static class AccountHelper
{
	public static WellKnownSidType MapCommonAccountNameToSid(string username)
	{
		username = username ?? throw new ArgumentNullException("username");
		username = username.Trim().Replace(" ", "");
		if (IsLocalSystem(username))
		{
			return WellKnownSidType.LocalSystemSid;
		}
		if (string.Equals(username, "NetworkService", StringComparison.OrdinalIgnoreCase))
		{
			return WellKnownSidType.NetworkServiceSid;
		}
		if (string.Equals(username, "LocalService", StringComparison.OrdinalIgnoreCase))
		{
			return WellKnownSidType.LocalServiceSid;
		}
		return WellKnownSidType.NullSid;
	}

	public static bool IsLocalSystem(string username)
	{
		if (!string.Equals(username, "LocalSystem", StringComparison.OrdinalIgnoreCase))
		{
			return string.Equals(username, "System", StringComparison.OrdinalIgnoreCase);
		}
		return true;
	}

	public static string GetSystemAccountName(WellKnownSidType type)
	{
		NTAccount nTAccount = (NTAccount)new SecurityIdentifier(type, null).Translate(typeof(NTAccount));
		if (!(nTAccount != null))
		{
			return string.Empty;
		}
		return nTAccount.ToString();
	}

	public static string GetAccountNameUnderWhichServiceRuns(string serviceName)
	{
		SelectQuery query = new SelectQuery("select name, startname from Win32_Service where name = '" + serviceName + "'");
		string text = null;
		using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(query))
		{
			using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = managementObjectSearcher.Get().GetEnumerator();
			if (managementObjectEnumerator.MoveNext())
			{
				text = managementObjectEnumerator.Current["startname"].ToString();
				if (text.StartsWith(".", StringComparison.Ordinal))
				{
					text = Environment.MachineName + text.Substring(1);
				}
			}
		}
		return text;
	}
}
