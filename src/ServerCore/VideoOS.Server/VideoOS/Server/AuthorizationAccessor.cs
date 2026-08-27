using VideoOS.Common.Integration.Security;
using VideoOS.Management.Common;

namespace VideoOS.Server;

public sealed class AuthorizationAccessor
{
	public static bool IsAdministrator(string userSid)
	{
		return IdentityCache.IsMember(userSid, SecurityManager.AdministratorRoleId.ToString());
	}

	public static bool IsMember(string userSid)
	{
		return IdentityCache.ReadIdentityFromCache(userSid, QueryMembership.Direct) != null;
	}
}
