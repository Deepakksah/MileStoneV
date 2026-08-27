using VideoOS.Management.Server.ConfigurationApi.AccessControl;

namespace VideoOS.Management.Server.ConfigurationApi;

public class AccessControlGroupSecurity
{
	public static void RegisterSecurityObject(IGeminiRequestContext context, string parentObjectId, string objectId)
	{
		context.GetSecurityManagerService().RegisterSecurityObject(context, VideoOS.Management.Server.ConfigurationApi.AccessControl.Constants.AccessControlSecurityNamespace, objectId.ToUpperInvariant(), parentObjectId.ToUpperInvariant());
	}

	public static void UnregisterSecurityObject(IGeminiRequestContext context, string objectId)
	{
		context.GetSecurityManagerService().UnregisterSecurityObject(context, VideoOS.Management.Server.ConfigurationApi.AccessControl.Constants.AccessControlSecurityNamespace, objectId.ToUpperInvariant());
	}
}
