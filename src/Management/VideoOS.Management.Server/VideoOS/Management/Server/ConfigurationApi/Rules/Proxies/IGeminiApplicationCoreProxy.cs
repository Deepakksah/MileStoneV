namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public interface IGeminiApplicationCoreProxy
{
	bool IsFeatureLicensed(IGeminiRequestContext requestContext, string featureName);
}
