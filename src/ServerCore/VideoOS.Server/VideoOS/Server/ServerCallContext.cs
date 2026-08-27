using VideoOS.Management.Common;

namespace VideoOS.Server;

public static class ServerCallContext
{
	public static Identity GetCallingUser(string userSid)
	{
		return IdentityCache.ReadIdentityFromCache(userSid, QueryMembership.Expanded);
	}
}
