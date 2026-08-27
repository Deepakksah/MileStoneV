using System;
using System.Globalization;
using VideoOS.Management.Common;

namespace VideoOS.Common.Integration;

public class SecurityIdentityCache : IDisposable
{
	private GenericMemoryCache<string, Identity> _identityMemoryCache;

	private MembershipCache _membershipCache;

	public SecurityIdentityCache()
		: this(new DefaultMemoryCachePerformanceProvider())
	{
	}

	public SecurityIdentityCache(IMemoryCachePerformanceProvider performanceProvider)
	{
		_identityMemoryCache = new GenericMemoryCache<string, Identity>(performanceProvider, 50000, 10800);
		_membershipCache = new MembershipCache();
	}

	public Identity ReadIdentityFromCache(Func<IIdentityCacheDataProvider> getDataProvider, string sid, QueryMembership queryMembership)
	{
		if (getDataProvider == null)
		{
			throw new ArgumentNullException("getDataProvider");
		}
		if (string.IsNullOrEmpty(sid))
		{
			return null;
		}
		sid = sid.ToUpperInvariant();
		string text = sid;
		int num = (int)queryMembership;
		string key = text + num.ToString(CultureInfo.InvariantCulture);
		Identity identity = (Identity)_identityMemoryCache.Find(key);
		if (identity != null)
		{
			return identity;
		}
		using IIdentityCacheDataProvider identityCacheDataProvider = getDataProvider();
		Identity identity2 = identityCacheDataProvider.ReadIdentity(sid);
		if (identity2 != null)
		{
			identity2.MemberOf = _membershipCache.MemberOf(getDataProvider, sid, queryMembership);
			identity2.Members = _membershipCache.Members(getDataProvider, sid, queryMembership);
			if (_identityMemoryCache.Find(key) == null)
			{
				_identityMemoryCache.Add(key, identity2);
			}
		}
		return identity2;
	}

	public Identity[] ReadIdentitiesFromCache(Func<IIdentityCacheDataProvider> getDataProvider, string[] sidArray, QueryMembership queryMembership)
	{
		if (getDataProvider == null)
		{
			throw new ArgumentNullException("getDataProvider");
		}
		if (sidArray == null)
		{
			throw new ArgumentNullException("sidArray");
		}
		Identity[] array = new Identity[sidArray.Length];
		bool flag = false;
		for (int i = 0; i < sidArray.Length; i++)
		{
			string text = sidArray[i];
			if (string.IsNullOrEmpty(text))
			{
				array[i] = null;
				continue;
			}
			text = text.ToUpperInvariant();
			string text2 = text;
			int num = (int)queryMembership;
			string key = text2 + num.ToString(CultureInfo.InvariantCulture);
			Identity identity = (Identity)_identityMemoryCache.Find(key);
			if (identity != null)
			{
				array[i] = identity;
			}
			else
			{
				flag = true;
			}
		}
		if (flag)
		{
			using IIdentityCacheDataProvider identityCacheDataProvider = getDataProvider();
			for (int j = 0; j < array.Length; j++)
			{
				string text3 = sidArray[j];
				if (string.IsNullOrEmpty(text3))
				{
					continue;
				}
				text3 = text3.ToUpperInvariant();
				if (array[j] == null)
				{
					Identity identity2 = identityCacheDataProvider.ReadIdentity(text3);
					if (identity2 != null)
					{
						identity2.MemberOf = _membershipCache.MemberOf(getDataProvider, text3, queryMembership);
						identity2.Members = _membershipCache.Members(getDataProvider, text3, queryMembership);
						array[j] = identity2;
					}
				}
				if (array[j] != null)
				{
					string text4 = text3;
					int num = (int)queryMembership;
					string key2 = text4 + num.ToString(CultureInfo.InvariantCulture);
					if (_identityMemoryCache.Find(key2) == null)
					{
						_identityMemoryCache.Add(key2, array[j]);
					}
				}
			}
		}
		return array;
	}

	public bool IsMember(Func<IIdentityCacheDataProvider> getDataProvider, string userSid, string groupSid)
	{
		if (getDataProvider == null)
		{
			throw new ArgumentNullException("getDataProvider");
		}
		if (userSid == null)
		{
			throw new ArgumentNullException("userSid");
		}
		userSid = userSid.ToUpperInvariant();
		return _membershipCache.IsMember(getDataProvider, userSid, groupSid);
	}

	public void ClearMemoryCache(Action postAction)
	{
		_identityMemoryCache.Clear();
		_membershipCache.ForceMembshipReload();
		postAction?.Invoke();
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing && _membershipCache != null)
		{
			_membershipCache.Dispose();
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
