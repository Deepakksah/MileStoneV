using System;
using System.ComponentModel;
using System.DirectoryServices;
using System.Globalization;
using System.Security.Principal;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

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

	public string Domain => _domain;

	public string DomainUserName => _domain + "\\" + _userName;

	public IdentityAccessor(string sid, string domain, string userName, AccountType accountType)
	{
		_sid = sid;
		_domain = domain;
		_userName = userName;
		_accountType = accountType;
	}

	public abstract Identity QueryIdentity(IGeminiRequestContext requestContext, bool includeMembers);

	public static Identity ReadIdentityFromSource(IGeminiRequestContext requestContext, string sid)
	{
		Identity identity = null;
		return GetAccessor(requestContext, sid)?.QueryIdentity(requestContext, includeMembers: true);
	}

	public static IdentityAccessor GetAccessor(Identity identity)
	{
		return GetAccessor(identity.Sid, identity.Type);
	}

	public static IdentityAccessor GetAccessor(IGeminiRequestContext requestContext, string sid)
	{
		IdentityType type = IdentityType.UnknownIdentityType;
		SidUtility.IsCorporateIdentity(requestContext, sid, out type);
		return GetAccessor(sid, type);
	}

	private static IdentityAccessor GetAccessor(string sid, IdentityType corpType)
	{
		switch (corpType)
		{
		case IdentityType.BasicUser:
			return new BasicUserAccessor(sid);
		case IdentityType.Role:
			return new RoleAccessor(sid);
		default:
		{
			string domainName = string.Empty;
			string accountName = string.Empty;
			AccountType accountType = AccountType.SidTypeInvalid;
			try
			{
				SidUtility.ResolveSecurityIdentifier(sid, ref domainName, ref accountName, ref accountType);
			}
			catch (Win32Exception ex)
			{
				GeminiTrace.Warning(TraceType.Security, "NativeMethods error while resolving SID {0} with NativeErrorCode {1}. Error: {2}", sid, ex.NativeErrorCode, ex.Message);
				if (ex.NativeErrorCode == 1332)
				{
					return null;
				}
				if (ex.NativeErrorCode == 1789)
				{
					return new IdleAccessor(sid);
				}
			}
			if (string.IsNullOrEmpty(domainName))
			{
				return new GenericIdentityAccessor(sid, domainName, accountName, accountType);
			}
			if (IsLocalDomain(domainName) || SidUtility.IsBuiltinAccount(sid))
			{
				return new SAMAccessor(sid, domainName, accountName, accountType);
			}
			if (SidUtility.IsNTAccount(sid))
			{
				return new ActiveDirectoryAccessor(sid, domainName, accountName, accountType);
			}
			return new GenericIdentityAccessor(sid, domainName, accountName, accountType);
		}
		}
	}

	public static string GetSidBySearchFactor(IGeminiRequestContext requestContext, SearchFactor factor, string factorValue)
	{
		switch (factor)
		{
		case SearchFactor.Sid:
		{
			if (string.IsNullOrEmpty(factorValue))
			{
				return null;
			}
			if (Guid.TryParse(factorValue, out var result))
			{
				return result.ToString();
			}
			try
			{
				return new SecurityIdentifier(factorValue).Value;
			}
			catch (ArgumentException)
			{
			}
			return null;
		}
		case SearchFactor.AccountName:
			return GetSidFromAccountName(requestContext, factorValue);
		case SearchFactor.DistinguishedName:
			return GetSidFromDistinguishedName(factorValue);
		default:
			throw new InvalidEnumArgumentException("SearchFactor out of range");
		}
	}

	public static string GetSidFromBinarySid(object sidBytes)
	{
		return new SecurityIdentifier((sidBytes as byte[]) ?? throw VmoExceptionFactory.VMO62017_ActiveDirectoryAccessException(), 0).ToString();
	}

	public static string GetSidFromAccountName(IGeminiRequestContext requestContext, string name)
	{
		SecurityManagerService.ParseIdentityName(name, out var domain, out var accountName, out var isRole, out var isBasicUser);
		try
		{
			if (isRole)
			{
				using (IRoleComponent roleComponent = requestContext.CreateRoleComponent())
				{
					try
					{
						return roleComponent.QueryRoleByName(accountName)?.Id;
					}
					catch (SecuritySubsystemException)
					{
						return null;
					}
				}
			}
			if (isBasicUser)
			{
				return requestContext.GetService<IdpClientService>().CreateUserIdpComponent().GetVmsUserByName(accountName)?.Sid;
			}
			NTAccount nTAccount = ((!IsLocalDomain(domain)) ? new NTAccount(string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", domain, accountName)) : new NTAccount(accountName));
			return nTAccount.Translate(typeof(SecurityIdentifier)).Value;
		}
		catch (ArgumentException)
		{
			return null;
		}
		catch (IdentityNotMappedException)
		{
		}
		catch (SystemException innerException)
		{
			throw VmoExceptionFactory.VMO60155_ActiveDirectoryAccessException(name, innerException);
		}
		return null;
	}

	public static string GetSidFromDistinguishedName(string distinguishedName)
	{
		if (string.IsNullOrEmpty(distinguishedName))
		{
			return null;
		}
		DirectoryEntry directoryEntry = null;
		if (distinguishedName.StartsWith("WinNT://", StringComparison.OrdinalIgnoreCase))
		{
			string userDomainName = Environment.UserDomainName;
			string path = distinguishedName;
			string[] array = distinguishedName.Substring("WinNT://".Length).Split('/');
			if (array.Length < 3 && array[0] != userDomainName)
			{
				return new NTAccount(string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", array[0], array[1])).Translate(typeof(SecurityIdentifier)).Value;
			}
			directoryEntry = new DirectoryEntry(path);
		}
		else if (distinguishedName.StartsWith("LOCAL://", StringComparison.OrdinalIgnoreCase))
		{
			directoryEntry = new DirectoryEntry(string.Format(CultureInfo.InvariantCulture, "WinNT://./{0}", distinguishedName.Substring("LOCAL://".Length)));
		}
		else
		{
			directoryEntry = new DirectoryEntry(string.Format(CultureInfo.InvariantCulture, "LDAP://{0}", distinguishedName.Replace("/", "\\/")));
			if (directoryEntry != null && directoryEntry.SchemaClassName.Equals("contact") && directoryEntry.Properties["msDS-SourceObjectDN"] != null)
			{
				distinguishedName = directoryEntry.Properties["msDS-SourceObjectDN"].Value.ToString();
				directoryEntry = new DirectoryEntry(string.Format(CultureInfo.InvariantCulture, "LDAP://{0}", distinguishedName.Replace("/", "\\/")));
			}
		}
		string result = null;
		if (directoryEntry != null && directoryEntry.Properties["objectSid"] != null)
		{
			result = GetSidFromBinarySid(directoryEntry.Properties["objectSid"].Value);
		}
		directoryEntry?.Dispose();
		return result;
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
