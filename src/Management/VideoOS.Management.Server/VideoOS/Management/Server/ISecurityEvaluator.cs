namespace VideoOS.Management.Server;

public interface ISecurityEvaluator
{
	bool HasPermission(IGeminiRequestContext requestContext, string token, ulong requestedPermission, bool alwaysAllowAdministrator);

	bool HasPermission(IGeminiRequestContext requestContext, string token, IdentityDescriptor descriptor, ulong requestedPermission, bool alwaysAllowAdministrator);

	AccessControlEntry2 QueryEffectivePermissions(IGeminiRequestContext requestContext, string token, IdentityDescriptor identity);
}
