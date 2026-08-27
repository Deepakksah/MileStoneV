using VideoOS.Platform.Util;

namespace VideoOS.Platform.SDK.Facades;

public class PluginManagerFacade : IPluginManagerFacade
{
	public void CloseCheckForStandalone()
	{
		PluginManager.PluginCloseCheckForStandalone();
	}
}
