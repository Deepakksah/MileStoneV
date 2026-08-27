using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class MonitorPresetFolder : IConfigurationItem
{
	public ICollection<MonitorPreset> MonitorPresets
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<MonitorPreset> list = new List<MonitorPreset>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "MonitorPreset")
				{
					list.Add(new MonitorPreset(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public MonitorPresetFolder(ServerId serverId, ConfigurationItem monitorPresetFolder)
		: base(serverId, monitorPresetFolder)
	{
	}

	public MonitorPresetFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public MonitorPresetFolder(FQID fqid)
		: base(fqid)
	{
	}
}
