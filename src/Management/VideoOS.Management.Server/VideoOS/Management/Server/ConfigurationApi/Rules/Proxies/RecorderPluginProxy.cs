using System.Linq;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public class RecorderPluginProxy : IRecorderPluginProxy
{
	public RecorderPluginData QueryPluginById(IGeminiRequestContext requestContext, string id)
	{
		return RecorderPluginData.QueryPluginById(requestContext, id);
	}

	public RecorderPluginData[] QueryPlugins(IGeminiRequestContext requestContext)
	{
		return RecorderPluginData.QueryPlugins(requestContext).ToArray();
	}
}
