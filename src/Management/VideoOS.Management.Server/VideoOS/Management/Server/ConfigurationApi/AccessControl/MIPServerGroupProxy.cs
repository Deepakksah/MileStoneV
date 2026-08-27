namespace VideoOS.Management.Server.ConfigurationApi.AccessControl;

public class MIPServerGroupProxy : IMIPServerGroupProxy
{
	public MIPServerGroup[] QueryServerGroupsByParentGroupId(IGeminiRequestContext requestContext, string parentGroupId)
	{
		return MIPServerGroup.QueryServerGroupsByParentGroupId(requestContext, ServerGroup.EnumStructureType.MIPObjectGroup, parentGroupId);
	}

	public MIPServerGroup QueryServerGroupById(IGeminiRequestContext requestContext, string serverGroupId)
	{
		return MIPServerGroup.QueryServerGroupById(requestContext, serverGroupId);
	}
}
