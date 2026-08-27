namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public interface IServerInformationProxy
{
	ServerInformation GetServerInformation(IGeminiRequestContext requestContext);
}
