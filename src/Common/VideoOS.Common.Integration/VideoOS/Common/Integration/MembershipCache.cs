using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VideoOS.Management.Common;

namespace VideoOS.Common.Integration;

public class MembershipCache : IDisposable, IMembershipCache
{
	private Membership[] _memberships;

	private bool _membershipsLoaded;

	private ReaderWriterLockSlim _accessLock = new ReaderWriterLockSlim();

	private Dictionary<string, HashSet<string>> _groupSidMembersDictionary;

	private Dictionary<string, HashSet<string>> _userSidMemberOfDictionary;

	private Dictionary<string, HashSet<string>> _userSidMemberOfDictionaryWithDirectRelations;

	private Dictionary<string, HashSet<string>> _groupSidMembersDictionaryWithDirectRelations;

	public MembershipCache()
	{
		_membershipsLoaded = false;
	}

	private void InitializeMembersTable()
	{
		_groupSidMembersDictionary = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
		foreach (KeyValuePair<string, HashSet<string>> item in _userSidMemberOfDictionary)
		{
			foreach (string item2 in item.Value)
			{
				if (_groupSidMembersDictionary.ContainsKey(item2))
				{
					_groupSidMembersDictionary[item2].Add(item.Key);
					continue;
				}
				HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
				hashSet.Add(item.Key);
				_groupSidMembersDictionary.Add(item2, hashSet);
			}
		}
	}

	private void InitializeMemberOfTable()
	{
		CreateMemberOfDictionaryWithDirectRelations();
		UpdateMemberOfDictionaryWithSecondaryRelations();
	}

	private void UpdateMemberOfDictionaryWithSecondaryRelations()
	{
		bool flag = true;
		int num = 0;
		while (flag)
		{
			flag = false;
			foreach (KeyValuePair<string, HashSet<string>> item in _userSidMemberOfDictionary)
			{
				if (item.Value.Count > num)
				{
					flag = true;
					ExpandRelations(num, item);
				}
			}
			num++;
		}
	}

	private void ExpandRelations(int parentIndex, KeyValuePair<string, HashSet<string>> item)
	{
		string text = item.Value.ElementAtOrDefault(parentIndex);
		if (string.IsNullOrEmpty(text) || !_userSidMemberOfDictionary.TryGetValue(text, out var value))
		{
			return;
		}
		foreach (string item2 in value)
		{
			if (item2 != item.Key && !item.Value.Contains(item2))
			{
				item.Value.Add(item2);
			}
		}
	}

	private void CreateMemberOfDictionaryWithDirectRelations()
	{
		_userSidMemberOfDictionary = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
		_groupSidMembersDictionaryWithDirectRelations = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
		Membership[] memberships = _memberships;
		foreach (Membership membership in memberships)
		{
			if (_userSidMemberOfDictionary.TryGetValue(membership.MemberSid, out var value))
			{
				value.Add(membership.Container);
			}
			else
			{
				HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
				hashSet.Add(membership.Container);
				_userSidMemberOfDictionary.Add(membership.MemberSid, hashSet);
				if (!_userSidMemberOfDictionary.ContainsKey(membership.Container))
				{
					hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
					_userSidMemberOfDictionary.Add(membership.Container, hashSet);
				}
			}
			if (_groupSidMembersDictionaryWithDirectRelations.TryGetValue(membership.Container, out value))
			{
				value.Add(membership.MemberSid);
				continue;
			}
			HashSet<string> hashSet2 = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			hashSet2.Add(membership.MemberSid);
			_groupSidMembersDictionaryWithDirectRelations.Add(membership.Container, hashSet2);
		}
		_userSidMemberOfDictionaryWithDirectRelations = new Dictionary<string, HashSet<string>>(_userSidMemberOfDictionary.Count, _userSidMemberOfDictionary.Comparer);
		foreach (KeyValuePair<string, HashSet<string>> item in _userSidMemberOfDictionary)
		{
			HashSet<string> hashSet3 = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (string item2 in item.Value)
			{
				hashSet3.Add(item2);
			}
			_userSidMemberOfDictionaryWithDirectRelations.Add(item.Key, hashSet3);
		}
	}

	private void EnsureMembershipsLoaded(Func<IIdentityCacheDataProvider> getDataProvider)
	{
		if (_membershipsLoaded)
		{
			return;
		}
		try
		{
			_accessLock.EnterWriteLock();
			if (!_membershipsLoaded)
			{
				using (IIdentityCacheDataProvider identityCacheDataProvider = getDataProvider())
				{
					_memberships = identityCacheDataProvider.GetMemberships();
				}
				if (_memberships != null)
				{
					InitializeMemberOfTable();
					InitializeMembersTable();
				}
				_membershipsLoaded = true;
			}
		}
		finally
		{
			_accessLock.ExitWriteLock();
		}
	}

	public void ForceMembshipReload()
	{
		try
		{
			_accessLock.EnterWriteLock();
			_membershipsLoaded = false;
		}
		finally
		{
			_accessLock.ExitWriteLock();
		}
	}

	public bool IsMember(Func<IIdentityCacheDataProvider> getDataProvider, string userSid, string groupSid)
	{
		EnsureMembershipsLoaded(getDataProvider);
		bool result = false;
		try
		{
			_accessLock.EnterReadLock();
			if (_memberships == null)
			{
				return result;
			}
			if (_groupSidMembersDictionary.TryGetValue(groupSid, out var value))
			{
				result = value.Contains(userSid);
			}
		}
		finally
		{
			_accessLock.ExitReadLock();
		}
		return result;
	}

	public string[] MemberOf(Func<IIdentityCacheDataProvider> getDataProvider, string userSid, QueryMembership queryMembership)
	{
		EnsureMembershipsLoaded(getDataProvider);
		string[] array = new string[0];
		return queryMembership switch
		{
			QueryMembership.None => array, 
			QueryMembership.Direct => DirectMemberOf(userSid), 
			QueryMembership.Expanded => ExpandedMemberOf(userSid), 
			_ => array, 
		};
	}

	private string[] ExpandedMemberOf(string userSid)
	{
		string[] result = new string[0];
		try
		{
			_accessLock.EnterReadLock();
			if (_memberships == null)
			{
				return result;
			}
			if (_userSidMemberOfDictionary.TryGetValue(userSid, out var value))
			{
				result = value.ToArray();
			}
		}
		finally
		{
			_accessLock.ExitReadLock();
		}
		return result;
	}

	private string[] DirectMemberOf(string userSid)
	{
		string[] result = new string[0];
		try
		{
			_accessLock.EnterReadLock();
			if (_memberships == null)
			{
				return result;
			}
			if (_userSidMemberOfDictionaryWithDirectRelations.TryGetValue(userSid, out var value))
			{
				result = value.ToArray();
			}
		}
		finally
		{
			_accessLock.ExitReadLock();
		}
		return result;
	}

	public string[] Members(Func<IIdentityCacheDataProvider> getDataProvider, string groupSid, QueryMembership queryMembership)
	{
		EnsureMembershipsLoaded(getDataProvider);
		string[] array = new string[0];
		return queryMembership switch
		{
			QueryMembership.None => array, 
			QueryMembership.Direct => DirectMembers(groupSid), 
			QueryMembership.Expanded => ExpandedMembers(groupSid), 
			_ => array, 
		};
	}

	private string[] ExpandedMembers(string groupSid)
	{
		string[] result = new string[0];
		try
		{
			_accessLock.EnterReadLock();
			if (_memberships == null)
			{
				return result;
			}
			if (_groupSidMembersDictionary.TryGetValue(groupSid, out var value))
			{
				result = value.ToArray();
			}
		}
		finally
		{
			_accessLock.ExitReadLock();
		}
		return result;
	}

	private string[] DirectMembers(string groupSid)
	{
		string[] result = new string[0];
		try
		{
			_accessLock.EnterReadLock();
			if (_memberships == null)
			{
				return result;
			}
			if (_groupSidMembersDictionaryWithDirectRelations.TryGetValue(groupSid, out var value))
			{
				result = value.ToArray();
			}
		}
		finally
		{
			_accessLock.ExitReadLock();
		}
		return result;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing && _accessLock != null)
		{
			_accessLock.Dispose();
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
