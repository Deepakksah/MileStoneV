using System;
using System.Security.Principal;
using System.ServiceProcess;

namespace VideoOS.Server.Admin.Utilities;

public class AccountInfo
{
	private static readonly SecurityIdentifier _networkServiceSid = new SecurityIdentifier(WellKnownSidType.NetworkServiceSid, null);

	private static readonly SecurityIdentifier _localServiceSid = new SecurityIdentifier(WellKnownSidType.LocalServiceSid, null);

	private static readonly SecurityIdentifier _localSystemSid = new SecurityIdentifier(WellKnownSidType.LocalSystemSid, null);

	private static string _computerDomain = null;

	private SecurityIdentifier _securityIdentifier;

	private string _accountName;

	public string AccountName
	{
		get
		{
			return _accountName;
		}
		private set
		{
			if (!string.IsNullOrEmpty(value) && value.StartsWith(".\\", StringComparison.OrdinalIgnoreCase))
			{
				_accountName = value.Substring(2, value.Length - 2);
			}
			else
			{
				_accountName = value;
			}
		}
	}

	public string Password { get; private set; }

	public string Domain
	{
		get
		{
			int num = AccountName.IndexOf('\\');
			if (num > 0 && num != AccountName.Length - 1)
			{
				return AccountName.Substring(0, num);
			}
			return string.Empty;
		}
	}

	public ServiceAccount ServiceAccountType
	{
		get
		{
			if (IsLocalService)
			{
				return ServiceAccount.LocalService;
			}
			if (IsLocalSystem)
			{
				return ServiceAccount.LocalSystem;
			}
			if (IsNetworkService)
			{
				return ServiceAccount.NetworkService;
			}
			return ServiceAccount.User;
		}
	}

	public string UserName
	{
		get
		{
			int num = AccountName.IndexOf('\\');
			if (num > 0 && num != AccountName.Length - 1)
			{
				return AccountName.Substring(num + 1);
			}
			return string.Empty;
		}
	}

	public string Sid => new NTAccount(AccountName).Translate(typeof(SecurityIdentifier)).Value;

	public bool IsServiceAccount
	{
		get
		{
			if (IsNetworkService || IsLocalService || IsLocalSystem)
			{
				return true;
			}
			return false;
		}
	}

	public bool IsNetworkService
	{
		get
		{
			if (_securityIdentifier == null)
			{
				return false;
			}
			return _securityIdentifier.Equals(_networkServiceSid);
		}
	}

	public bool IsLocalService
	{
		get
		{
			if (_securityIdentifier == null)
			{
				return false;
			}
			return _securityIdentifier.Equals(_localServiceSid);
		}
	}

	public bool IsLocalSystem
	{
		get
		{
			if (_securityIdentifier == null)
			{
				return false;
			}
			return _securityIdentifier.Equals(_localSystemSid);
		}
	}

	public static string ComputerDomain
	{
		get
		{
			if (_computerDomain == null)
			{
				string text = null;
				string computerNameEx = NativeMethods.GetComputerNameEx(NativeMethods.COMPUTER_NAME_FORMAT.ComputerNameDnsDomain);
				if (computerNameEx != null)
				{
					text = NativeMethods.GetNetBIOSDomainName(computerNameEx);
					if (text == null)
					{
						NativeMethods.DnsHostnameToComputerName(computerNameEx);
					}
				}
				if (text == null)
				{
					text = Environment.MachineName;
				}
				_computerDomain = text;
			}
			return _computerDomain;
		}
	}

	private string LocalizedName { get; set; }

	public AccountInfo(string accountName, string password, bool requirePassword)
	{
		AccountName = accountName;
		Password = password;
		_securityIdentifier = (SecurityIdentifier)new NTAccount(AccountName).Translate(typeof(SecurityIdentifier));
		NTAccount nTAccount = (NTAccount)_securityIdentifier.Translate(typeof(NTAccount));
		LocalizedName = nTAccount.Value.ToString();
		if (!IsServiceAccount && requirePassword && string.IsNullOrEmpty(Password))
		{
			throw new ServerAdminException("Missing Password");
		}
	}

	public static string GetNetworkServiceRemoteCredentials()
	{
		return ComputerDomain + "\\" + Environment.MachineName + "$";
	}

	public static bool IsLegalAccountName(string accountName)
	{
		int num = accountName.IndexOf('\\');
		if (num > 0 && num != accountName.Length - 1)
		{
			accountName = accountName.Substring(num + 1);
		}
		if (!string.IsNullOrEmpty(accountName))
		{
			return accountName.Length < 256;
		}
		return false;
	}

	public bool IsValid()
	{
		if (!string.IsNullOrEmpty(AccountName))
		{
			return IsLegalAccountName(AccountName);
		}
		return false;
	}

	public string GetDatabaseAccount(bool isDualServer)
	{
		if (IsNetworkService && isDualServer)
		{
			return ComputerDomain + "\\" + Environment.MachineName + "$";
		}
		return LocalizedName;
	}

	public string GetLocalizedName()
	{
		return LocalizedName;
	}
}
