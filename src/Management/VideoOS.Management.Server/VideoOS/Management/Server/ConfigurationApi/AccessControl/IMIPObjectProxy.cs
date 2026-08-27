namespace VideoOS.Management.Server.ConfigurationApi.AccessControl;

public interface IMIPObjectProxy
{
	MIPObject QueryMIPObject(IGeminiRequestContext requestContext, string mipObjectId);

	MIPObject[] QueryMIPObjects(IGeminiRequestContext requestContext, string serverGroupId);

	MIPObject[] QueryMIPObjectsByKind(IGeminiRequestContext requestContext, string kindId, string subKind);
}
