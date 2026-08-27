namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public class UserDefinedEventProxy : IUserDefinedEventProxy
{
	public UserDefinedEvent QueryUserDefinedEvent(IGeminiRequestContext requestContext, string eventId)
	{
		return UserDefinedEvent.QueryUserDefinedEvent(requestContext, eventId);
	}
}
