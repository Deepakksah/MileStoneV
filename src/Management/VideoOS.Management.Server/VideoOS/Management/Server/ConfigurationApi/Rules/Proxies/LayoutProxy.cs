namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public class LayoutProxy : ILayoutProxy
{
	public ViewLayout QueryLayout(IGeminiRequestContext requestContext, string layoutId)
	{
		return ViewLayout.QueryViewLayoutById(requestContext, layoutId);
	}
}
