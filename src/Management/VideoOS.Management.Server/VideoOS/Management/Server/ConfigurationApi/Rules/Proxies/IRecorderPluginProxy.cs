namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public interface IRecorderPluginProxy
{
	RecorderPluginData QueryPluginById(IGeminiRequestContext requestContext, string id);

	RecorderPluginData[] QueryPlugins(IGeminiRequestContext requestContext);
}
