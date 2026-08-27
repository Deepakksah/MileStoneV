using System.Collections;
using System.Collections.Generic;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.SDK.Platform;

public static class PlatformManager
{
	public static List<PluginDefinition> PlatformContentTypes()
	{
		ArrayList pluginDefinitions = PluginManager.GetPluginDefinitions();
		List<PluginDefinition> list = new List<PluginDefinition>();
		foreach (PluginDefinition item in pluginDefinitions)
		{
			list.Add(item);
		}
		return list;
	}
}
