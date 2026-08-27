namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public class ServerInformationProxy : IServerInformationProxy
{
	public ServerInformation GetServerInformation(IGeminiRequestContext requestContext)
	{
		return ServerInformation.GetServerInformation(requestContext);
	}
}
