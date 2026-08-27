using System;
using System.DirectoryServices.AccountManagement;

namespace VideoOS.Installer.UI.Framework;

public class UserAccountHandling
{
	public static string CheckDomainOrLocalUser(string username, string machineName, string domainName)
	{
		try
		{
			if (UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Domain, domainName), IdentityType.SamAccountName, username) != null)
			{
				return $"{domainName}\\{username}";
			}
		}
		catch (Exception)
		{
		}
		try
		{
			if (UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Machine, machineName), IdentityType.SamAccountName, username) != null)
			{
				return $"{machineName}\\{username}";
			}
		}
		catch (Exception)
		{
		}
		return username;
	}
}
