using System;
using System.DirectoryServices.AccountManagement;
using System.Net;
using System.Security.Principal;

namespace VideoOS.Utilities.Database.Authentication;

public static class DomainHelper
{
	public static bool IsDbServerLocal(string hostname)
	{
		return IsSameSystem(Environment.MachineName, hostname);
	}

	public static bool IsLocalUser(string accountName)
	{
		using (PrincipalContext context = new PrincipalContext(ContextType.Machine))
		{
			using Principal principal = Principal.FindByIdentity(context, IdentityType.SamAccountName, accountName);
			if (principal != null)
			{
				return string.Equals(accountName, principal.Sid.Translate(typeof(NTAccount)).ToString(), StringComparison.OrdinalIgnoreCase);
			}
		}
		return false;
	}

	public static bool IsNetworkService(string accountName)
	{
		SecurityIdentifier securityIdentifier = (SecurityIdentifier)new NTAccount(accountName).Translate(typeof(SecurityIdentifier));
		if (securityIdentifier == null)
		{
			return false;
		}
		return securityIdentifier.IsWellKnown(WellKnownSidType.NetworkServiceSid);
	}

	public static string GetRemoteAccountNameForLocalUser(string hostName, string accountName)
	{
		string[] array = accountName.Split('\\');
		string text = ((array.Length == 2) ? array[1] : accountName);
		return hostName + "\\" + text;
	}

	public static string GetNetworkServiceAccountName(bool isDualServer, string computerDomain)
	{
		if (isDualServer)
		{
			return computerDomain + "\\" + Environment.MachineName + "$";
		}
		return ((NTAccount)new SecurityIdentifier(WellKnownSidType.NetworkServiceSid, null).Translate(typeof(NTAccount))).Value;
	}

	private static bool IsSameSystem(string hostName1, string hostName2)
	{
		string hostName3 = Dns.GetHostEntry(hostName1).HostName;
		string hostName4 = Dns.GetHostEntry(hostName2).HostName;
		return string.Equals(hostName3, hostName4, StringComparison.OrdinalIgnoreCase);
	}
}
