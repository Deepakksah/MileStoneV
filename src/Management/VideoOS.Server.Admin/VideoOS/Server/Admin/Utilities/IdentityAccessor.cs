using System;
using System.ComponentModel;
using System.Security.Principal;
using VideoOS.Management.Common;

namespace VideoOS.Server.Admin.Utilities;

public abstract class IdentityAccessor
{
	protected string _sid;

	protected string _domain;

	protected string _userName;

	protected AccountType _accountType;

	public static readonly string _LDAPToken = "LDAP://";

	public static readonly string _WinNTToken = "WinNT://";

	public static readonly string _LocalToken = "LOCAL://";

	private static string _localMachineName = Environment.MachineName;

	public IdentityAccessor(string sid, string domain, string userName, AccountType accountType)
	{
		_sid = sid;
		_domain = domain;
		_userName = userName;
		_accountType = accountType;
	}

	public abstract Identity QueryIdentity(bool includeMembers);

	public static Identity ReadIdentityFromSource(string sid, Role role, string[] members)
	{
		Identity result = null;
		IdentityAccessor accessor = GetAccessor(sid, role, members);
		if (accessor != null)
		{
			result = accessor.QueryIdentity(includeMembers: true);
		}
		return result;
	}

	public static IdentityAccessor GetAccessor(string sid, Role role, string[] members)
	{
		if (SidUtility.IsRoleIdentity(sid))
		{
			return new RoleAccessor(sid, role, members);
		}
		string domainName = string.Empty;
		string empty = string.Empty;
		AccountType accountType = AccountType.SidTypeInvalid;
		try
		{
			SecurityIdentifier securityIdentifier = new SecurityIdentifier(sid);
			byte[] array = new byte[securityIdentifier.BinaryLength];
			securityIdentifier.GetBinaryForm(array, 0);
			empty = NativeMethods.SidToName(array, out domainName, out accountType);
		}
		catch (Win32Exception)
		{
			return null;
		}
		if (string.IsNullOrEmpty(domainName))
		{
			return new GenericIdentityAccessor(sid, domainName, empty, accountType);
		}
		if (IsLocalDomain(domainName) || SidUtility.IsBuiltinAccount(sid))
		{
			return new SAMAccessor(sid, domainName, empty, accountType);
		}
		if (SidUtility.IsNTAccount(sid))
		{
			return new ActiveDirectoryAccessor(sid, domainName, empty, accountType);
		}
		return new GenericIdentityAccessor(sid, domainName, empty, accountType);
	}

	public static string GetSidFromBinarySid(object sidBytes)
	{
		return new SecurityIdentifier((sidBytes as byte[]) ?? throw new FormatException(), 0).ToString();
	}

	public static void ParseIdentityName(string name, out string domain, out string accountName, out bool isRole)
	{
		domain = null;
		accountName = null;
		isRole = false;
		int num = name.IndexOfAny(new char[2] { '\\', '/' });
		if (num < 0)
		{
			accountName = name;
			return;
		}
		domain = name.Substring(0, num).Trim();
		accountName = name.Substring(num + 1);
		if (domain.Length > 2 && domain[0] == '[' && domain[domain.Length - 1] == ']')
		{
			isRole = true;
		}
	}

	public static bool IsLocalDomain(string domainName)
	{
		return _localMachineName.Equals(domainName, StringComparison.OrdinalIgnoreCase);
	}

	public static string NormalizeString(object value)
	{
		return (!(value is string text)) ? string.Empty : text.Trim();
	}
}
