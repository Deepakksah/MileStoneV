namespace VideoOS.Management.Server;

public interface IGeminiSecurityNamespace
{
	SecurityNamespaceInfo NamespaceInfo { get; }

	bool IsHierachical { get; }

	void CheckPermission(IGeminiRequestContext requestContext, string token, string requestedPermission);

	bool HasPermission(IGeminiRequestContext requestContext, string token, ulong requestedPermission, bool checkAdministrator);

	bool HasPermission(IGeminiRequestContext requestContext, string identitySid, string actionId, string objectId, bool checkAdministrator);

	AccessControlEntry2 QueryPermissionMask(IGeminiRequestContext requestContext, string token);

	AccessControlEntry2 QueryPermissionMaskForOther(IGeminiRequestContext requestContext, string token, string userSid);

	void ForceNamespaceReload(IGeminiRequestContext requestContext);

	bool HasAction(string actionId);
}
