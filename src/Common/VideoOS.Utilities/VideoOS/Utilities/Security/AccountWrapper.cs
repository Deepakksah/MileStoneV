using System;

namespace VideoOS.Utilities.Security;

[CLSCompliant(true)]
public class AccountWrapper : IAccount
{
	public bool AddAccountRightToAccount(string systemName, string accountName, string accountRight, out string domainName)
	{
		return Account.AddAccountRightToAccount(systemName, accountName, accountRight, out domainName);
	}

	public string GetServiceLogOnUser(string serviceName)
	{
		return Account.GetServiceLogOnUser(serviceName);
	}

	public string GetProcessOwner(int processId)
	{
		return Account.GetProcessOwner(processId);
	}

	public bool HasAccountAccountRight(string systemName, string accountName, string accountRight)
	{
		return Account.HasAccountAccountRight(systemName, accountName, accountRight);
	}

	public bool IsAccountValid(string userName, string domain, string password)
	{
		return Account.IsAccountValid(userName, domain, password);
	}

	public bool IsCurrentUserAdministrator()
	{
		return Account.IsCurrentUserAdministrator();
	}

	public bool IsServiceAccountValid(string userName, string domain, string password)
	{
		return IsServiceAccountValid(userName, domain, password);
	}

	public bool IsUserAdministrator(string userName)
	{
		return Account.IsUserAdministrator(userName);
	}

	public string LookupAccountName(string systemName, string accountName)
	{
		return LookupAccountName(systemName, accountName);
	}

	public string LookupSid(string systemName, string accountName)
	{
		return LookupSid(systemName, accountName);
	}
}
