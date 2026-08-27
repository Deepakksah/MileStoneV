using System;

namespace VideoOS.Utilities.Security;

[CLSCompliant(true)]
public interface IAccount
{
	bool AddAccountRightToAccount(string systemName, string accountName, string accountRight, out string domainName);

	string GetServiceLogOnUser(string serviceName);

	bool HasAccountAccountRight(string systemName, string accountName, string accountRight);

	bool IsAccountValid(string userName, string domain, string password);

	bool IsCurrentUserAdministrator();

	bool IsServiceAccountValid(string userName, string domain, string password);

	bool IsUserAdministrator(string userName);

	string LookupAccountName(string systemName, string accountName);

	string LookupSid(string systemName, string accountName);

	string GetProcessOwner(int processId);
}
