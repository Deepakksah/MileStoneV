using System;
using System.Linq;
using VideoOS.Common.Integration.Security;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public abstract class SecurityEvaluatorBase : ISecurityEvaluator
{
	protected GeminiSecurityNamespace SecurityNamespace { get; set; }

	protected IIdentityService IdentityService { get; set; }

	public abstract AccessControlEntry2 QueryEffectivePermissions(IGeminiRequestContext requestContext, string token, IdentityDescriptor identity);

	public bool HasPermission(IGeminiRequestContext requestContext, string token, ulong requestedPermission, bool alwaysAllowAdministrator)
	{
		return HasPermission(requestContext, token, requestContext.UserContext, requestedPermission, alwaysAllowAdministrator);
	}

	public bool HasPermission(IGeminiRequestContext requestContext, string token, IdentityDescriptor descriptor, ulong requestedPermission, bool alwaysAllowAdministrator)
	{
		bool flag = false;
		AccessControlEntry2 accessControlEntry = QueryEffectivePermissions(requestContext, token, descriptor);
		if (accessControlEntry != null)
		{
			flag = (accessControlEntry.Allow & requestedPermission) == requestedPermission;
		}
		if (!flag && alwaysAllowAdministrator)
		{
			Identity identity = IdentityCache.ReadIdentityFromCache(requestContext, descriptor.Identifier, QueryMembership.Expanded);
			flag = identity != null && identity.MemberOfRoles != null && identity.MemberOfRoles.Any((Guid f) => f == SecurityManager.AdministratorRoleId);
		}
		return flag;
	}
}
