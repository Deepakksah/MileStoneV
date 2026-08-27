namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public interface ISiteProxy
{
	Site QuerySite(IGeminiRequestContext requestContext, string siteId);

	Site QueryHomeSite(IGeminiRequestContext requestContext);
}
