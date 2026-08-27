namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public class GeminiApplicationCoreProxy : IGeminiApplicationCoreProxy
{
	public bool IsFeatureLicensed(IGeminiRequestContext requestContext, string featureName)
	{
		return GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, featureName);
	}
}
