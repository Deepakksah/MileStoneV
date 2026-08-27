namespace VideoOS.Management.Server.ConfigurationApi;

public interface ISiteRepository
{
	Site[] QuerySlaveSites(IGeminiRequestContext requestContext);

	Site[] QuerySlaveSites(IGeminiRequestContext requestContext, string parentId);

	Site QuerySite(IGeminiRequestContext requestContext, string id);

	Site QueryHomeSite(IGeminiRequestContext requestContext);

	ServerInformation GetServerInformation(IGeminiRequestContext requestContext);
}
