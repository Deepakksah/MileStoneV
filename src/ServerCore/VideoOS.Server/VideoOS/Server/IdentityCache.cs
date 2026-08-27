using VideoOS.Common.Integration;
using VideoOS.Management.Common;
using VideoOS.Server.DataComponents;

namespace VideoOS.Server;

public static class IdentityCache
{
	private static SecurityIdentityCache _securityIdentityCache = new SecurityIdentityCache();

	public static void ClearMemoryCache()
	{
		_securityIdentityCache.ClearMemoryCache(null);
		Server.Instance.TokenHandler.UpdateSecurityToken();
	}

	public static Identity ReadIdentityFromCache(string sid, QueryMembership queryMembership)
	{
		return _securityIdentityCache.ReadIdentityFromCache(() => new IdentityCacheComponent(), sid, queryMembership);
	}

	public static bool IsMember(string userSid, string groupSid)
	{
		return _securityIdentityCache.IsMember(() => new IdentityCacheComponent(), userSid, groupSid);
	}
}
