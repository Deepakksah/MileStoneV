using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class HardwarePtzSettingsFolder : IConfigurationItem
{
	public ICollection<HardwarePtzSettings> HardwarePtzSettings
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<HardwarePtzSettings> list = new List<HardwarePtzSettings>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "HardwarePtzSettings")
				{
					list.Add(new HardwarePtzSettings(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public HardwarePtzSettingsFolder(ServerId serverId, ConfigurationItem hardwarePtzSettingsFolder)
		: base(serverId, hardwarePtzSettingsFolder)
	{
	}

	public HardwarePtzSettingsFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public HardwarePtzSettingsFolder(FQID fqid)
		: base(fqid)
	{
	}
}
