using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Globalization;
using System.Linq;
using System.Security.Authentication;
using System.Security.Principal;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

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

	public void InitializeDomainControllerConnection()
	{
		try
		{
			SecurityIdentifier securityIdentifier = new SecurityIdentifier(_sid);
			byte[] binaryForm = new byte[securityIdentifier.BinaryLength];
			securityIdentifier.GetBinaryForm(binaryForm, 0);
			string value = securityIdentifier.AccountDomainSid.Value;
			_domainProperties = DomainProperties.GetDomainProperties(value, forceReconnect: true);
		}
		catch (InvalidDomainException)
		{
			throw;
		}
		catch
		{
			throw new InvalidDomainException();
		}
	}

	public Identity QueryIdentity(IGeminiRequestContext requestContext, bool includeMembers, ICollection<string> failingDomainList)
	{
		return QueryIdentity(includeMembers, IdentityInDictionary, AddIdentityToDictionary, failingDomainList);
	}

	public override Identity QueryIdentity(IGeminiRequestContext requestContext, bool includeMembers)
	{
		return QueryIdentity(requestContext, includeMembers, null);
	}

	public Identity QueryIdentity(bool includeMembers, IdentityFound identityFound, IdentityToProcess identityToProcess, ICollection<string> failingDomainList)
	{
		if (failingDomainList != null && failingDomainList.Any((string s) => s.Equals(_domain, StringComparison.OrdinalIgnoreCase)))
		{
			return null;
		}
		string text = null;
		for (int num = 0; num < 3; num++)
		{
			try
			{
				SecurityIdentifier securityIdentifier = new SecurityIdentifier(_sid);
				byte[] array = new byte[securityIdentifier.BinaryLength];
				securityIdentifier.GetBinaryForm(array, 0);
				text = securityIdentifier.AccountDomainSid.Value;
				_domainProperties = DomainProperties.GetDomainProperties(text, forceReconnect: false);
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
						GetGroupMembers(identityFound, identityToProcess, failingDomainList);
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
			catch (Exception ex2)
			{
				GeminiTrace.Error("Error retrieving Identity from Active Directory", ex2);
				if (text != null)
				{
					DomainProperties.ClearDomainProperties(text);
				}
			}
		}
		return null;
	}

	public void GetGroupMembers(IdentityFound identityFound, IdentityToProcess identityToProcess, ICollection<string> failingDomainList)
	{
		QueryGroupMembersAD(identityToProcess);
		QueryGroupMemberProperties(identityFound, identityToProcess, failingDomainList);
		QueryGroupMemberPrimary(identityToProcess);
	}

	private static void SetIdentityType(Identity identity, SearchResult searchResult)
	{
		if (!searchResult.Properties.Contains("objectClass"))
		{
			return;
		}
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

	private static void SetIdentitySID(Identity identity, SearchResult searchResult)
	{
		if (searchResult.Properties.Contains("objectSid") && searchResult.Properties["objectSid"].Count > 0)
		{
			QuerySidAndDomain(searchResult.Properties["objectSid"][0], out var sid, out var domain);
			identity.Sid = sid;
			identity.Domain = domain;
		}
	}

	private static void SetIdentityDisplayName(Identity identity, SearchResult searchResult)
	{
		if (searchResult.Properties.Contains("displayName") && searchResult.Properties["displayName"].Count > 0)
		{
			identity.DisplayName = IdentityAccessor.NormalizeString(searchResult.Properties["displayName"][0]);
		}
	}

	private static void SetIdentityDescription(Identity identity, SearchResult searchResult)
	{
		if (searchResult.Properties.Contains("description") && searchResult.Properties["description"].Count > 0)
		{
			identity.Description = IdentityAccessor.NormalizeString(searchResult.Properties["description"][0]);
		}
	}

	private static void SetIdentitySAMAccountName(Identity identity, SearchResult searchResult)
	{
		if (searchResult.Properties.Contains("sAMAccountName") && searchResult.Properties["sAMAccountName"].Count > 0)
		{
			identity.AccountName = IdentityAccessor.NormalizeString(searchResult.Properties["sAMAccountName"][0]);
		}
	}

	private static void SetIdentityDistinguishedName(Identity identity, SearchResult searchResult)
	{
		if (searchResult.Properties.Contains("distinguishedName") && searchResult.Properties["distinguishedName"].Count > 0)
		{
			identity.DistinguishedName = IdentityAccessor.NormalizeString(searchResult.Properties["distinguishedName"][0]);
		}
	}

	private static void SetDirectoryEntryPath(DirectoryEntry directoryEntry, SearchResult searchResult)
	{
		if (directoryEntry.SchemaClassName.Equals("contact", StringComparison.OrdinalIgnoreCase) && searchResult.Properties.Contains("msDS-SourceObjectDN") && searchResult.Properties["msDS-SourceObjectDN"].Count > 0)
		{
			string text = searchResult.Properties["msDS-SourceObjectDN"][0].ToString();
			text = IdentityAccessor._LDAPToken + text.Replace("/", "\\/");
			if (DirectoryEntry.Exists(text))
			{
				directoryEntry.Path = text;
			}
		}
		if (directoryEntry.SchemaClassName.Equals("foreignSecurityPrincipal", StringComparison.OrdinalIgnoreCase) && searchResult.Properties.Contains("objectSid") && searchResult.Properties["objectSid"].Count > 0 && directoryEntry.Properties["objectSid"][0] is byte[] binarySid)
		{
			string path = DomainProperties.GetDomainProperties(SidUtility.GetDomainSid(binarySid)).ADSIPathFromSID(binarySid);
			if (DirectoryEntry.Exists(path))
			{
				directoryEntry.Path = path;
			}
		}
	}

	private static Identity GetIdentityFromDirectoryEntry(DirectoryEntry directoryEntry)
	{
		Identity identity = new Identity();
		using (DirectorySearcher directorySearcher = new DirectorySearcher(directoryEntry))
		{
			directorySearcher.Filter = "(objectClass=*)";
			directorySearcher.PropertiesToLoad.Add("msDS-SourceObjectDN");
			directorySearcher.PropertiesToLoad.Add("objectSid");
			directorySearcher.PropertiesToLoad.Add("displayName");
			directorySearcher.PropertiesToLoad.Add("objectClass");
			directorySearcher.PropertiesToLoad.Add("description");
			directorySearcher.PropertiesToLoad.Add("sAMAccountName");
			directorySearcher.PropertiesToLoad.Add("distinguishedName");
			directorySearcher.SearchScope = SearchScope.Subtree;
			SearchResult searchResult = directorySearcher.FindOne();
			SetDirectoryEntryPath(directoryEntry, searchResult);
			SetIdentityType(identity, searchResult);
			SetIdentitySID(identity, searchResult);
			SetIdentityDisplayName(identity, searchResult);
			SetIdentityDescription(identity, searchResult);
			SetIdentitySAMAccountName(identity, searchResult);
			SetIdentityDistinguishedName(identity, searchResult);
			if (string.IsNullOrEmpty(identity.Sid) || string.IsNullOrEmpty(identity.DistinguishedName) || string.IsNullOrEmpty(identity.Domain))
			{
				identity.Type = IdentityType.InvalidIdentity;
			}
			if (string.IsNullOrEmpty(identity.DisplayName))
			{
				identity.DisplayName = ((identity.AccountName.Length == 0) ? identity.Sid : identity.AccountName);
			}
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
				GeminiTrace.Verbose(TraceType.Security, "QueryGroupMembersAD found member " + item.Path);
				Identity identityFromSearchResult = GetIdentityFromSearchResult(item);
				if (identityFromSearchResult != null && identityFromSearchResult.Type != IdentityType.InvalidIdentity)
				{
					identityToProcess(identityFromSearchResult);
				}
			}
		}
		catch (Exception ex)
		{
			GeminiTrace.Error(TraceType.Security, "Error in QueryGroupMembersAD for {0}. Error: {1}", _identity.DistinguishedName, ex.Message);
		}
	}

	private void QueryGroupMemberProperties(IdentityFound identityFound, IdentityToProcess identityToProcess, ICollection<string> failingDomainList)
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
						GeminiTrace.Verbose("ADAccessor.GetGroupMemberProperties expanding member " + text2);
						try
						{
							if (text2.StartsWith(IdentityAccessor._LDAPToken, StringComparison.OrdinalIgnoreCase))
							{
								text2 = text2.Substring(IdentityAccessor._LDAPToken.Length);
							}
							string path = IdentityAccessor._LDAPToken + text2.Replace("/", "\\/");
							if (!DirectoryEntry.Exists(path))
							{
								continue;
							}
							directoryEntry.Path = path;
							Identity identity = GetIdentityFromDirectoryEntry(directoryEntry);
							if (identity != null)
							{
								if (identity.Type == IdentityType.InvalidIdentity && identity.Domain != _domainProperties.DomainName)
								{
									identity = RetrieveForeignIdentity(identity, identityFound, identityToProcess, failingDomainList);
								}
								if (identity.Type != IdentityType.InvalidIdentity)
								{
									identityToProcess(identity);
								}
							}
						}
						catch (Exception ex)
						{
							GeminiTrace.Error("Error in ActiveDirectoryAccessor.GetGroupMemberProperties for memberDN " + text2, ex);
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
		catch (Exception ex2)
		{
			GeminiTrace.Error("Error in ActiveDirectoryAccessor.GetGroupMemberProperties for memberDN " + _identity.DistinguishedName, ex2);
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
				GeminiTrace.Verbose("QueryGroupMembersPrimary found member " + item.Path);
				Identity identityFromSearchResult = GetIdentityFromSearchResult(item);
				if (identityFromSearchResult != null && identityFromSearchResult.Type != IdentityType.InvalidIdentity)
				{
					identityToProcess(identityFromSearchResult);
				}
			}
		}
		catch (Exception ex)
		{
			GeminiTrace.Error(TraceType.Security, "Error in QueryGroupMembersPrimary for {0}.", _identity.DistinguishedName);
			GeminiTrace.TraceException(ex);
		}
	}

	private static Identity RetrieveForeignIdentity(Identity identityToResolve, IdentityFound identityFound, IdentityToProcess identityToProcess, ICollection<string> failingDomainList)
	{
		return ((ActiveDirectoryAccessor)IdentityAccessor.GetAccessor(identityToResolve)).QueryIdentity(includeMembers: true, identityFound, identityToProcess, failingDomainList);
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
					catch (DirectoryServicesCOMException ex)
					{
						GeminiTrace.Error("Error getting Active Directory search root", ex);
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
