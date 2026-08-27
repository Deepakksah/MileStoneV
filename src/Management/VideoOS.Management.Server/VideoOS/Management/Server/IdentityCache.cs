using VideoOS.Common.Integration;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public static class IdentityCache
{
	private static SecurityIdentityCache _securityIdentityCache = new SecurityIdentityCache(new MemoryCachePerformanceProvider("IdentityCache"));

	public static void ClearMemoryCache()
	{
		_securityIdentityCache.ClearMemoryCache(Integration.TryClearIdentityCacheServerAsync);
	}

	public static Identity ReadIdentityFromCache(IGeminiRequestContext requestContext, string sid, QueryMembership queryMembership)
	{
		return _securityIdentityCache.ReadIdentityFromCache(requestContext.CreateIdentityCacheComponent, sid, queryMembership);
	}

	public static Identity[] ReadIdentitiesFromCache(IGeminiRequestContext requestContext, string[] sidArray, QueryMembership queryMembership)
	{
		return _securityIdentityCache.ReadIdentitiesFromCache(requestContext.CreateIdentityCacheComponent, sidArray, queryMembership);
	}

	public static bool IsMember(IGeminiRequestContext requestContext, string userSid, string groupSid)
	{
		return _securityIdentityCache.IsMember(requestContext.CreateIdentityCacheComponent, userSid, groupSid);
	}
}
