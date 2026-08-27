namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public class TimeProfileProxy : ITimeProfileProxy
{
	public TimeProfile QueryTimeProfile(IGeminiRequestContext requestContext, string timeProfileId)
	{
		return TimeProfile.QueryTimeProfile(requestContext, timeProfileId);
	}
}
