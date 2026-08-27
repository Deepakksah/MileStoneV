namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public class MIPActionProxy : IMIPActionProxy
{
	public MIPAction[] QueryMIPActions(IGeminiRequestContext requestContext)
	{
		return MIPAction.QueryMIPActions(requestContext);
	}

	public MIPAction QueryMIPAction(IGeminiRequestContext requestContext, string mipActionId)
	{
		return MIPAction.QueryMIPAction(requestContext, mipActionId);
	}
}
