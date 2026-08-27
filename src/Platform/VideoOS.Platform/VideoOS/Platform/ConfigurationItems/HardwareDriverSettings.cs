using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class HardwareDriverSettings : IConfigurationItem
{
	public ICollection<HardwareDriverSettingsChildItem> HardwareDriverSettingsChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<HardwareDriverSettingsChildItem> list = new List<HardwareDriverSettingsChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "HardwareDriverSettings")
				{
					list.Add(new HardwareDriverSettingsChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public ICollection<PTZChildItem> PTZChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<PTZChildItem> list = new List<PTZChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "PTZ")
				{
					list.Add(new PTZChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public HardwareDriverSettings(ServerId serverId, ConfigurationItem hardwareDriverSettings)
		: base(serverId, hardwareDriverSettings)
	{
	}

	public HardwareDriverSettings(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public HardwareDriverSettings(FQID fqid)
		: base(fqid)
	{
	}
}
