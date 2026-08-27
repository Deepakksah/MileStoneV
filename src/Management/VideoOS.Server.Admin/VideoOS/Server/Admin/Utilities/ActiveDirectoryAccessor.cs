using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Globalization;
using System.Security.Authentication;
using System.Security.Principal;
using VideoOS.Management.Common;

namespace VideoOS.Server.Admin.Utilities;

public class ActiveDirectoryAccessor(string sid, string domain, string userName, AccountType accountType) : IdentityAccessor(sid, domain, userName, accountType)
{
	public delegate bool IdentityFound(string distinguishedName);

	public delegate void IdentityToProcess(Identity identity);

	private Dictionary<string, string> _memberSids;

	private DirectoryEntry _identityEntry;

	private Identity _identity;

	private DomainProperties _domainProperties;

	private static object _searchRootLock = new object();

	private static bool _searchRootInit = false;

	private static DirectoryEntry _searchRoot = null;

	private string[] _identityAttributes = new string[6] { "objectClass", "objectSid", "displayName", "description", "sAMAccountName", "distinguishedName" };

	public override Identity QueryIdentity(bool includeMembers)
	{
		return QueryIdentity(includeMembers, IdentityInDictionary, AddIdentityToDictionary);
	}

	public Identity QueryIdentity(bool includeMembers, IdentityFound identityFound, IdentityToProcess identityToProcess)
	{
		for (int i = 0; i < 3; i++)
		{
			_domainProperties = DomainProperties.GetDomainProperties(_domain);
			try
			{
				SecurityIdentifier securityIdentifier = new SecurityIdentifier(_sid);
				byte[] array = new byte[securityIdentifier.BinaryLength];
				securityIdentifier.GetBinaryForm(array, 0);
				string path = _domainProperties.ADSIPathFromSID(array);
				if (!DirectoryEntry.Exists(path))
				{
					return null;
				}
				using (_identityEntry = _domainProperties.DirectoryEntry)
				{
					_identityEntry.Path = path;
					_identity = GetIdentityFromDirectoryEntry(_identityEntry);
					if (includeMembers && _identity != null && _identity.Type == IdentityType.WindowsGroup)
					{
						_memberSids = new Dictionary<string, string>();
						GetGroupMembers(identityFound, identityToProcess);
						_identity.Members = new string[_memberSids.Count];
						_memberSids.Values.CopyTo(_identity.Members, 0);
					}
					return _identity;
				}
			}
			catch (AuthenticationException)
			{
				DomainProperties.ClearDomainProperties();
			}
			catch (Exception)
			{
				DomainProperties.ClearDomainProperties(_domain);
			}
		}
		return null;
	}

	public void GetGroupMembers(IdentityFound identityFound, IdentityToProcess identityToProcess)
	{
		QueryGroupMembersAD(identityToProcess);
		QueryGroupMemberProperties(identityFound, identityToProcess);
		QueryGroupMemberPrimary(identityToProcess);
	}

	private static Identity GetIdentityFromDirectoryEntry(DirectoryEntry directoryEntry)
	{
		Identity identity = new Identity();
		if (directoryEntry.SchemaClassName.Equals("contact", StringComparison.OrdinalIgnoreCase) && directoryEntry.Properties.Contains("msDS-SourceObjectDN"))
		{
			string text = directoryEntry.Properties["msDS-SourceObjectDN"].Value.ToString();
			text = IdentityAccessor._LDAPToken + text.Replace("/", "\\/");
			if (DirectoryEntry.Exists(text))
			{
				directoryEntry.Path = text;
			}
		}
		if (directoryEntry.SchemaClassName.Equals("foreignSecurityPrincipal", StringComparison.OrdinalIgnoreCase) && directoryEntry.Properties.Contains("objectSid") && directoryEntry.Properties["objectSid"].Value is byte[] binarySid)
		{
			string path = DomainProperties.GetDomainProperties(SidUtility.GetDomainSid(binarySid)).ADSIPathFromSID(binarySid);
			if (DirectoryEntry.Exists(path))
			{
				directoryEntry.Path = path;
			}
		}
		if (directoryEntry.Properties.Contains("objectClass"))
		{
			PropertyValueCollection propertyValueCollection = directoryEntry.Properties["objectClass"];
			for (int num = propertyValueCollection.Count - 1; num >= 0; num--)
			{
				string text2 = propertyValueCollection[num] as string;
				if (!string.IsNullOrEmpty(text2))
				{
					if (text2.Equals("user", StringComparison.OrdinalIgnoreCase))
					{
						identity.Type = IdentityType.WindowsUser;
						break;
					}
					if (text2.Equals("group", StringComparison.OrdinalIgnoreCase))
					{
						identity.Type = IdentityType.WindowsGroup;
						break;
					}
				}
			}
		}
		if (directoryEntry.Properties.Contains("objectSid"))
		{
			QuerySidAndDomain(directoryEntry.Properties["objectSid"].Value, out var sid, out var domain);
			identity.Sid = sid;
			identity.Domain = domain;
		}
		if (directoryEntry.Properties.Contains("displayName"))
		{
			identity.DisplayName = IdentityAccessor.NormalizeString(directoryEntry.Properties["displayName"].Value);
		}
		if (directoryEntry.Properties.Contains("description"))
		{
			identity.Description = IdentityAccessor.NormalizeString(directoryEntry.Properties["description"].Value);
		}
		if (directoryEntry.Properties.Contains("sAMAccountName"))
		{
			identity.AccountName = IdentityAccessor.NormalizeString(directoryEntry.Properties["sAMAccountName"].Value);
		}
		if (directoryEntry.Properties.Contains("distinguishedName"))
		{
			identity.DistinguishedName = IdentityAccessor.NormalizeString(directoryEntry.Properties["distinguishedName"].Value);
		}
		if (string.IsNullOrEmpty(identity.Sid) || string.IsNullOrEmpty(identity.DistinguishedName) || string.IsNullOrEmpty(identity.Domain))
		{
			identity.Type = IdentityType.InvalidIdentity;
		}
		if (string.IsNullOrEmpty(identity.DisplayName))
		{
			identity.DisplayName = ((identity.AccountName.Length == 0) ? identity.Sid : identity.AccountName);
		}
		return identity;
	}

	private static Identity GetIdentityFromSearchResult(SearchResult searchResult)
	{
		Identity identity = new Identity();
		if (searchResult.Properties.Contains("objectClass"))
		{
			ResultPropertyValueCollection resultPropertyValueCollection = searchResult.Properties["objectClass"];
			for (int num = resultPropertyValueCollection.Count - 1; num >= 0; num--)
			{
				string text = resultPropertyValueCollection[num] as string;
				if (!string.IsNullOrEmpty(text))
				{
					if (text.Equals("user", StringComparison.OrdinalIgnoreCase))
					{
						identity.Type = IdentityType.WindowsUser;
						break;
					}
					if (text.Equals("group", StringComparison.OrdinalIgnoreCase))
					{
						identity.Type = IdentityType.WindowsGroup;
						break;
					}
				}
			}
		}
		if (searchResult.Properties.Contains("objectSid"))
		{
			QuerySidAndDomain(searchResult.Properties["objectSid"][0], out var sid, out var domain);
			identity.Sid = sid;
			identity.Domain = domain;
		}
		if (searchResult.Properties.Contains("displayName"))
		{
			identity.DisplayName = IdentityAccessor.NormalizeString(searchResult.Properties["displayName"][0]);
		}
		if (searchResult.Properties.Contains("description"))
		{
			identity.Description = IdentityAccessor.NormalizeString(searchResult.Properties["description"][0]);
		}
		if (searchResult.Properties.Contains("sAMAccountName"))
		{
			identity.AccountName = IdentityAccessor.NormalizeString(searchResult.Properties["sAMAccountName"][0]);
		}
		if (searchResult.Properties.Contains("distinguishedName"))
		{
			identity.DistinguishedName = IdentityAccessor.NormalizeString(searchResult.Properties["distinguishedName"][0]);
		}
		if (string.IsNullOrEmpty(identity.Sid) || string.IsNullOrEmpty(identity.DistinguishedName) || string.IsNullOrEmpty(identity.Domain))
		{
			identity.Type = IdentityType.InvalidIdentity;
		}
		if (string.IsNullOrEmpty(identity.DisplayName))
		{
			identity.DisplayName = ((identity.AccountName.Length == 0) ? identity.Sid : identity.AccountName);
		}
		return identity;
	}

	private void QueryGroupMembersAD(IdentityToProcess identityToProcess)
	{
		try
		{
			DirectoryEntry directorySearchRoot = GetDirectorySearchRoot();
			string filter = string.Format(CultureInfo.InvariantCulture, "(memberOf={0})", _identity.DistinguishedName);
			using DirectorySearcher directorySearcher = new DirectorySearcher(directorySearchRoot, filter, _identityAttributes, SearchScope.Subtree);
			directorySearcher.PageSize = 100;
			using SearchResultCollection searchResultCollection = directorySearcher.FindAll();
			foreach (SearchResult item in searchResultCollection)
			{
				Identity identityFromSearchResult = GetIdentityFromSearchResult(item);
				if (identityFromSearchResult != null && identityFromSearchResult.Type != IdentityType.InvalidIdentity)
				{
					identityToProcess(identityFromSearchResult);
				}
			}
		}
		catch (Exception)
		{
		}
	}

	private void QueryGroupMemberProperties(IdentityFound identityFound, IdentityToProcess identityToProcess)
	{
		try
		{
			using DirectoryEntry directoryEntry = _domainProperties.DirectoryEntry;
			using DirectorySearcher directorySearcher = new DirectorySearcher(_identityEntry);
			directorySearcher.Filter = "(objectClass=*)";
			int num = 999;
			int num2 = 0;
			int num3 = num2 + (num - 1);
			bool flag = false;
			while (true)
			{
				string text = ((!flag) ? string.Format(CultureInfo.InvariantCulture, "member;range={0}-{1}", num2, num3) : string.Format(CultureInfo.InvariantCulture, "member;range={0}-*", num2));
				directorySearcher.PropertiesToLoad.Clear();
				directorySearcher.PropertiesToLoad.Add(text);
				SearchResult searchResult = directorySearcher.FindOne();
				if (searchResult.Properties.Contains(text))
				{
					foreach (object item in searchResult.Properties[text])
					{
						string text2 = item as string;
						if (text2 == null || identityFound(text2))
						{
							continue;
						}
						try
						{
							if (text2.StartsWith(IdentityAccessor._LDAPToken, StringComparison.OrdinalIgnoreCase))
							{
								text2 = text2.Substring(IdentityAccessor._LDAPToken.Length);
							}
							string path = IdentityAccessor._LDAPToken + text2.Replace("/", "\\/");
							if (DirectoryEntry.Exists(path))
							{
								directoryEntry.Path = path;
								Identity identityFromDirectoryEntry = GetIdentityFromDirectoryEntry(directoryEntry);
								if (identityFromDirectoryEntry != null && identityFromDirectoryEntry.Type != IdentityType.InvalidIdentity)
								{
									identityToProcess(identityFromDirectoryEntry);
								}
							}
						}
						catch (Exception)
						{
						}
					}
					if (flag)
					{
						break;
					}
				}
				else
				{
					if (flag)
					{
						break;
					}
					flag = true;
				}
				if (!flag)
				{
					num2 = num3 + 1;
					num3 = num2 + (num - 1);
				}
			}
		}
		catch (Exception)
		{
		}
	}

	private void QueryGroupMemberPrimary(IdentityToProcess identityToProcess)
	{
		try
		{
			uint windowsSidRID = GetWindowsSidRID(_sid);
			if (windowsSidRID == 0)
			{
				return;
			}
			DirectoryEntry domainDirectoryEntry = _domainProperties.DomainDirectoryEntry;
			string filter = string.Format(CultureInfo.InvariantCulture, "(PrimaryGroupID={0})", windowsSidRID);
			using DirectorySearcher directorySearcher = new DirectorySearcher(domainDirectoryEntry, filter, _identityAttributes, SearchScope.Subtree);
			directorySearcher.PageSize = 100;
			using SearchResultCollection searchResultCollection = directorySearcher.FindAll();
			foreach (SearchResult item in searchResultCollection)
			{
				Identity identityFromSearchResult = GetIdentityFromSearchResult(item);
				if (identityFromSearchResult != null && identityFromSearchResult.Type != IdentityType.InvalidIdentity)
				{
					identityToProcess(identityFromSearchResult);
				}
			}
		}
		catch (Exception)
		{
		}
	}

	private static uint GetWindowsSidRID(string sid)
	{
		SecurityIdentifier securityIdentifier = new SecurityIdentifier(sid);
		byte[] array = new byte[securityIdentifier.BinaryLength];
		securityIdentifier.GetBinaryForm(array, 0);
		int revision;
		ulong authority;
		uint[] array2 = SidUtility.DecodeSid(array, out revision, out authority);
		if (authority != 5)
		{
			return 0u;
		}
		if (array2.Length != 5)
		{
			return 0u;
		}
		return array2[4];
	}

	private static DirectoryEntry GetDirectorySearchRoot()
	{
		if (!_searchRootInit)
		{
			lock (_searchRootLock)
			{
				if (!_searchRootInit)
				{
					try
					{
						IEnumerator enumerator = new DirectoryEntry("GC:").Children.GetEnumerator();
						enumerator.MoveNext();
						_searchRoot = (DirectoryEntry)enumerator.Current;
					}
					catch (DirectoryServicesCOMException)
					{
						_searchRoot = null;
					}
				}
			}
			_searchRootInit = true;
		}
		return _searchRoot;
	}

	private static void QuerySidAndDomain(object objectSid, out string sid, out string domain)
	{
		sid = IdentityAccessor.GetSidFromBinarySid(objectSid);
		DomainProperties domainProperties = DomainProperties.GetDomainProperties(SidUtility.GetDomainSid(objectSid as byte[]));
		domain = domainProperties.DomainName;
	}

	private bool IdentityInDictionary(string distinguishedName)
	{
		return _memberSids.ContainsKey(distinguishedName);
	}

	private void AddIdentityToDictionary(Identity member)
	{
		_memberSids[member.DistinguishedName] = member.Sid;
	}
}
