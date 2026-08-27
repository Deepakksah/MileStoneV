using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class HardwareDriverSettingsFolder : IConfigurationItem
{
	public ICollection<HardwareDriverSettings> HardwareDriverSettings
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<HardwareDriverSettings> list = new List<HardwareDriverSettings>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "HardwareDriverSettings")
				{
					list.Add(new HardwareDriverSettings(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public HardwareDriverSettingsFolder(ServerId serverId, ConfigurationItem hardwareDriverSettingsFolder)
		: base(serverId, hardwareDriverSettingsFolder)
	{
	}

	public HardwareDriverSettingsFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public HardwareDriverSettingsFolder(FQID fqid)
		: base(fqid)
	{
	}
}
