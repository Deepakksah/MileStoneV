namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public class SiteProxy : ISiteProxy
{
	public Site QuerySite(IGeminiRequestContext requestContext, string siteId)
	{
		return Site.QuerySite(requestContext, siteId);
	}

	public Site QueryHomeSite(IGeminiRequestContext requestContext)
	{
		return Site.QueryHomeSite(requestContext);
	}
}
