namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public interface IMIPActionProxy
{
	MIPAction[] QueryMIPActions(IGeminiRequestContext requestContext);

	MIPAction QueryMIPAction(IGeminiRequestContext requestContext, string mipActionId);
}
