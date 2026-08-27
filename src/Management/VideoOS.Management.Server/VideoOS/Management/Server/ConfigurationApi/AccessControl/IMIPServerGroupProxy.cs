namespace VideoOS.Management.Server.ConfigurationApi.AccessControl;

public interface IMIPServerGroupProxy
{
	MIPServerGroup[] QueryServerGroupsByParentGroupId(IGeminiRequestContext requestContext, string parentGroupId);

	MIPServerGroup QueryServerGroupById(IGeminiRequestContext requestContext, string serverGroupId);
}
