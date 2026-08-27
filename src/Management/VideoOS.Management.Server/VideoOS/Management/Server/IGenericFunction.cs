namespace VideoOS.Management.Server;

public interface IGenericFunction
{
	GenericObject2[] Execute(IGeminiRequestContext requestContext, string functionName, GenericObject2[] parameters, int clientVersion);
}
