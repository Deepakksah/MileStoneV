namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public interface ILayoutProxy
{
	ViewLayout QueryLayout(IGeminiRequestContext requestContext, string layoutId);
}
