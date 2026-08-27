namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public interface ITimeProfileProxy
{
	TimeProfile QueryTimeProfile(IGeminiRequestContext requestContext, string timeProfileId);
}
