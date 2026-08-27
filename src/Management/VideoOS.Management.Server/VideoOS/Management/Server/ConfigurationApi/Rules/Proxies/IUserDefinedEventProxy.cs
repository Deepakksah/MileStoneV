namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public interface IUserDefinedEventProxy
{
	UserDefinedEvent QueryUserDefinedEvent(IGeminiRequestContext requestContext, string eventId);
}
