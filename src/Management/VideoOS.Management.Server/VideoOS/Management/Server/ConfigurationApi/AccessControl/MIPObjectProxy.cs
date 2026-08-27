namespace VideoOS.Management.Server.ConfigurationApi.AccessControl;

public class MIPObjectProxy : IMIPObjectProxy
{
	public MIPObject QueryMIPObject(IGeminiRequestContext requestContext, string mipObjectId)
	{
		return MIPObject.QueryMIPObject(requestContext, mipObjectId);
	}

	public MIPObject[] QueryMIPObjects(IGeminiRequestContext requestContext, string serverGroupId)
	{
		return MIPObject.QueryMIPObjects(requestContext, serverGroupId, checkApplicationPermission: false);
	}

	public MIPObject[] QueryMIPObjectsByKind(IGeminiRequestContext requestContext, string kindId, string subKind)
	{
		return MIPObject.QueryMIPObjectsByKind(requestContext, kindId, subKind);
	}
}
